using OpenTK.Graphics.OpenGL;
using StbImageSharp;
using System.IO;
namespace Riddle_Shutova_PRI122.Objects
{
    public class ObstacleTexture
    {
        public int ID { get; private set; }
        public ObstacleTexture(string filepath)
        {
            if (!File.Exists(filepath)) return;
            StbImage.stbi_set_flip_vertically_on_load(1);
            using (var stream = File.OpenRead(filepath))
            {
                var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                ID = GL.GenTexture();
                GL.BindTexture(TextureTarget.Texture2D, ID);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0,
                PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            }
        }
        public void Delete()
        {
            GL.DeleteTexture(ID);
        }
    }
}