using OpenTK.Graphics.OpenGL;
using System;
using System.IO;

namespace Riddle_Shutova_PRI122.Objects
{
    public class Obstacle
    {
        public float PositionX { get; set; }
        public float PositionY { get; set; } = -1.35f;
        public float Width { get; set; } = 0.2f;
        public float Height { get; set; } = 0.5f;
        public float Depth { get; set; } = 0.5f;
        public float Speed { get; set; } = 0.05f;
        public bool IsActive { get; set; } = true;

        private static ObstacleTexture texture = null;

        public Obstacle(float startX)
        {
            PositionX = startX;
        }

        public static void LoadSharedTexture()
        {
            if (texture != null) return;

            string texturePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "obstacle.jpg");
            if (File.Exists(texturePath))
            {
                texture = new ObstacleTexture(texturePath);
            }
        }

        public void Update()
        {
            PositionX -= Speed;
        }

        public void Draw()
        {
            if (!IsActive) return;

            GL.Disable(EnableCap.Lighting);
            GL.Enable(EnableCap.Texture2D);

            if (texture != null)
            {
                GL.BindTexture(TextureTarget.Texture2D, texture.ID);
                GL.Color3(1f, 1f, 1f);
            }
            else
            {
                GL.Color3(1f, 0f, 0f);
            }

            GL.PushMatrix();
            GL.Translate(PositionX, PositionY + Height / 2, 0);
            GL.Scale(Width, Height, Depth);
            DrawTexturedCube();
            GL.PopMatrix();

            GL.Disable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Lighting);
        }

        private void DrawTexturedCube()
        {
            GL.Begin(PrimitiveType.Quads);

            // Передняя грань (Z = 1)
            GL.TexCoord2(0, 0); GL.Vertex3(-1, -1, 1);
            GL.TexCoord2(1, 0); GL.Vertex3(1, -1, 1);
            GL.TexCoord2(1, 1); GL.Vertex3(1, 1, 1);
            GL.TexCoord2(0, 1); GL.Vertex3(-1, 1, 1);

            // Задняя грань (Z = -1) 
            GL.TexCoord2(0, 0); GL.Vertex3(-1, -1, -1);
            GL.TexCoord2(0, 1); GL.Vertex3(-1, 1, -1);
            GL.TexCoord2(1, 1); GL.Vertex3(1, 1, -1);
            GL.TexCoord2(1, 0); GL.Vertex3(1, -1, -1);

            // Верхняя грань (Y = 1)
            GL.TexCoord2(0, 0); GL.Vertex3(-1, 1, -1);
            GL.TexCoord2(1, 0); GL.Vertex3(-1, 1, 1);
            GL.TexCoord2(1, 1); GL.Vertex3(1, 1, 1);
            GL.TexCoord2(0, 1); GL.Vertex3(1, 1, -1);

            // Нижняя грань (Y = -1)
            GL.TexCoord2(0, 0); GL.Vertex3(-1, -1, -1);
            GL.TexCoord2(1, 0); GL.Vertex3(1, -1, -1);
            GL.TexCoord2(1, 1); GL.Vertex3(1, -1, 1);
            GL.TexCoord2(0, 1); GL.Vertex3(-1, -1, 1);

            // Правая грань (X = 1)
            GL.TexCoord2(0, 0); GL.Vertex3(1, -1, -1);
            GL.TexCoord2(1, 0); GL.Vertex3(1, 1, -1);
            GL.TexCoord2(1, 1); GL.Vertex3(1, 1, 1);
            GL.TexCoord2(0, 1); GL.Vertex3(1, -1, 1);

            // Левая грань (X = -1)
            GL.TexCoord2(0, 0); GL.Vertex3(-1, -1, -1);
            GL.TexCoord2(1, 0); GL.Vertex3(-1, -1, 1);
            GL.TexCoord2(1, 1); GL.Vertex3(-1, 1, 1);
            GL.TexCoord2(0, 1); GL.Vertex3(-1, 1, -1);

            GL.End();
        }
    }
}