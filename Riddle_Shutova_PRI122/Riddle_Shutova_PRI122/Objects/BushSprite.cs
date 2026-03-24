using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using StbImageSharp;
using System;
using System.IO;

namespace Riddle_Shutova_PRI122.Objects
{
    public class BushSprite
    {
        private static int textureId = -1;
        private static int frameCount = 7;
        private static int frameWidth = 600;
        private static int frameHeight = 295;
        private static float animationSpeed = 0.1f; 
        public Vector3 Position { get; private set; }

        private Vector3 position;
        private float width;
        private float height;
        private int currentFrame;
        private float frameTime;

        public BushSprite(Vector3 pos, float scale = 1.0f)
        {
            Position = pos;
            position = pos;
            width = 0.8f * scale;
            height = 0.4f * scale;
            currentFrame = 0;
            frameTime = 0;
        }

        public static void LoadTexture(string filepath)
        {
            if (!File.Exists(filepath))
            {
                return;
            }

            StbImage.stbi_set_flip_vertically_on_load(1);
            using (var stream = File.OpenRead(filepath))
            {
                var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                textureId = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, textureId);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                    image.Width, image.Height, 0,
                    PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                    (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                    (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                    (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                    (int)TextureWrapMode.ClampToEdge);
            }
        }

        public void Update(float deltaTime)
        {
            if (textureId == -1) return;
            frameTime += deltaTime;
            if (frameTime >= animationSpeed)
            {
                frameTime -= animationSpeed;
                currentFrame = (currentFrame + 1) % frameCount;
            }
        }

        public void Draw(Vector3 cameraRight, Vector3 cameraUp)
        {
            if (textureId == -1) return;

            float texLeft = (float)currentFrame / frameCount;
            float texRight = (float)(currentFrame + 1) / frameCount;
            float texTop = 1.0f;
            float texBottom = 0.0f;

            Vector3[] localVertices = new Vector3[4];
            localVertices[0] = new Vector3(-width / 2, -height / 2, 0);
            localVertices[1] = new Vector3(width / 2, -height / 2, 0);
            localVertices[2] = new Vector3(width / 2, height / 2, 0);
            localVertices[3] = new Vector3(-width / 2, height / 2, 0);

            Vector3[] worldVertices = new Vector3[4];
            for (int i = 0; i < 4; i++)
            {
                worldVertices[i] = position + localVertices[i].X * cameraRight + localVertices[i].Y * cameraUp;
            }

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.BindTexture(TextureTarget.Texture2D, textureId);

            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(texLeft, texBottom); GL.Vertex3(worldVertices[0]);
            GL.TexCoord2(texRight, texBottom); GL.Vertex3(worldVertices[1]);
            GL.TexCoord2(texRight, texTop); GL.Vertex3(worldVertices[2]);
            GL.TexCoord2(texLeft, texTop); GL.Vertex3(worldVertices[3]);
            GL.End();

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.Texture2D);
        }
    }
}