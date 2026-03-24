using System;
using OpenTK.Graphics.OpenGL;

namespace Riddle_Shutova_PRI122.Managers
{
    public class FilterManager
    {
        private int fboId1, fboId2;          
        private int colorTex1, colorTex2;    
        private int blurProgram, sharpenProgram;
        private int vaoId, vboId;

        private int width, height;
        private bool isInitialized = false;

        private int texUniform, texelSizeUniform; 
        private int inputTexUniform;              

        public void Initialize(int w, int h)
        {
            if (isInitialized) Cleanup();

            width = w;
            height = h;

            colorTex1 = CreateColorTexture(w, h);
            colorTex2 = CreateColorTexture(w, h);

            fboId1 = CreateFbo(colorTex1);
            fboId2 = CreateFbo(colorTex2);

            CreateShaders();

            float[] vertices = {
                -1f, -1f, 0f, 0f,
                 1f, -1f, 1f, 0f,
                 1f,  1f, 1f, 1f,
                -1f, -1f, 0f, 0f,
                 1f,  1f, 1f, 1f,
                -1f,  1f, 0f, 1f
            };

            vboId = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboId);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            vaoId = GL.GenVertexArray();
            GL.BindVertexArray(vaoId);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 4 * sizeof(float), 2 * sizeof(float));
            GL.BindVertexArray(0);

            isInitialized = true;
        }

        private int CreateColorTexture(int w, int h)
        {
            int tex = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, tex);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, w, h, 0,
                PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            return tex;
        }

        private int CreateFbo(int colorTex)
        {
            int fbo = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fbo);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0,
                TextureTarget.Texture2D, colorTex, 0);
            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
                throw new Exception("FBO initialization failed");
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            return fbo;
        }

        private void CreateShaders()
        {
            // ----- Шейдер размытия (матрица M) -----
            string blurVS = @"
                #version 330
                layout(location = 0) in vec2 aPosition;
                layout(location = 1) in vec2 aTexCoord;
                out vec2 vTexCoord;
                void main() {
                    gl_Position = vec4(aPosition, 0.0, 1.0);
                    vTexCoord = aTexCoord;
                }";

            string blurFS = @"
                #version 330
                uniform sampler2D sceneTex;
                uniform vec2 texelSize;
                in vec2 vTexCoord;
                out vec4 fragColor;

                const float kernel[9] = float[](
                    1.0/16.0, 2.0/16.0, 1.0/16.0,
                    2.0/16.0, 4.0/16.0, 2.0/16.0,
                    1.0/16.0, 2.0/16.0, 1.0/16.0
                );

                void main() {
                    vec4 color = vec4(0.0);
                    for (int i = -1; i <= 1; i++) {
                        for (int j = -1; j <= 1; j++) {
                            vec2 offset = vec2(i, j) * texelSize;
                            float k = kernel[(i+1)*3 + (j+1)];
                            color += texture(sceneTex, vTexCoord + offset) * k;
                        }
                    }
                    fragColor = color;
                }";

            blurProgram = CreateProgram(blurVS, blurFS, "Blur");

            // ----- SШейдер резкости (матрица R) -----
            string sharpenFS = @"
                #version 330
                uniform sampler2D sceneTex;
                uniform vec2 texelSize;
                in vec2 vTexCoord;
                out vec4 fragColor;

                const float kernel[9] = float[](
                    -0.5, -0.5, -0.5,
                    -0.5,  5.0, -0.5,
                    -0.5, -0.5, -0.5
                );

                void main() {
                    vec4 color = vec4(0.0);
                    for (int i = -1; i <= 1; i++) {
                        for (int j = -1; j <= 1; j++) {
                            vec2 offset = vec2(i, j) * texelSize;
                            float k = kernel[(i+1)*3 + (j+1)];
                            color += texture(sceneTex, vTexCoord + offset) * k;
                        }
                    }
                    fragColor = color;
                }";

            sharpenProgram = CreateProgram(blurVS, sharpenFS, "Sharpen");
        }

        private int CreateProgram(string vs, string fs, string name)
        {
            int vShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vShader, vs);
            GL.CompileShader(vShader);
            CheckShaderCompileStatus(vShader, $"{name} Vertex");

            int fShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fShader, fs);
            GL.CompileShader(fShader);
            CheckShaderCompileStatus(fShader, $"{name} Fragment");

            int prog = GL.CreateProgram();
            GL.AttachShader(prog, vShader);
            GL.AttachShader(prog, fShader);
            GL.LinkProgram(prog);
            CheckProgramLinkStatus(prog, name);

            GL.DetachShader(prog, vShader);
            GL.DetachShader(prog, fShader);
            GL.DeleteShader(vShader);
            GL.DeleteShader(fShader);

            return prog;
        }

        private void CheckShaderCompileStatus(int shader, string name)
        {
            GL.GetShader(shader, ShaderParameter.CompileStatus, out int status);
            if (status == 0)
                throw new Exception($"{name} shader compilation error: {GL.GetShaderInfoLog(shader)}");
        }

        private void CheckProgramLinkStatus(int program, string name)
        {
            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out int status);
            if (status == 0)
                throw new Exception($"{name} program link error: {GL.GetProgramInfoLog(program)}");
        }
        public void BeginRenderToTexture()
        {
            if (!isInitialized) return;
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboId1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, width, height);
        }
        public void EndRenderAndApplyWatercolor()
        {
            if (!isInitialized) return;

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, fboId2);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Viewport(0, 0, width, height);

            GL.UseProgram(sharpenProgram);
            GL.Uniform1(GL.GetUniformLocation(sharpenProgram, "sceneTex"), 0);
            GL.Uniform2(GL.GetUniformLocation(sharpenProgram, "texelSize"), 1.0f / width, 1.0f / height);

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, colorTex1);

            GL.BindVertexArray(vaoId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);

            GL.UseProgram(0);

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, width, height);

            GL.UseProgram(blurProgram); 
            GL.Uniform1(GL.GetUniformLocation(blurProgram, "sceneTex"), 0);
            GL.Uniform2(GL.GetUniformLocation(blurProgram, "texelSize"), 1.0f / width, 1.0f / height);

            GL.BindTexture(TextureTarget.Texture2D, colorTex2);

            GL.BindVertexArray(vaoId);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
            GL.BindVertexArray(0);

            GL.UseProgram(0);
        }

        public void Resize(int w, int h)
        {
            if (!isInitialized) return;
            width = w;
            height = h;
            GL.BindTexture(TextureTarget.Texture2D, colorTex1);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0,
                PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
            GL.BindTexture(TextureTarget.Texture2D, colorTex2);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, width, height, 0,
                PixelFormat.Bgra, PixelType.UnsignedByte, IntPtr.Zero);
        }

        public void Cleanup()
        {
            if (!isInitialized) return;
            GL.DeleteFramebuffer(fboId1);
            GL.DeleteFramebuffer(fboId2);
            GL.DeleteTexture(colorTex1);
            GL.DeleteTexture(colorTex2);
            GL.DeleteProgram(blurProgram);
            GL.DeleteProgram(sharpenProgram);
            GL.DeleteBuffer(vboId);
            GL.DeleteVertexArray(vaoId);
            isInitialized = false;
        }
    }
}