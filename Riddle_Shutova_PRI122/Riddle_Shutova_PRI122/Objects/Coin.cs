using OpenTK.Graphics.OpenGL;
using System;

namespace Riddle_Shutova_PRI122.Objects
{
    public class Coin
    {
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; } 
        public float Speed { get; set; } = 0.05f;
        public bool IsActive { get; set; } = true;

        private float rotationAngle;
        private float rotationSpeed = 3f;
        private float scale;
        private float scaleDirection;
        private const float MinScale = 0.7f;
        private const float MaxScale = 1.3f;
        private const float ScaleSpeed = 0.02f;

        private float radius = 0.2f;
        private float thickness = 0.05f;

        public Coin(float startX, float startY)
        {
            PositionX = startX;
            PositionY = startY;
            PositionZ = 0f; 

            Random random = new Random();
            rotationAngle = (float)(random.NextDouble() * 360); 
            scale = (float)(random.NextDouble() * (MaxScale - MinScale) + MinScale); 
            scaleDirection = random.Next(2) == 0 ? 1 : -1; 
        }

        public void Update()
        {
            PositionX -= Speed;

            rotationAngle += rotationSpeed;
            if (rotationAngle > 360) rotationAngle -= 360;

            scale += scaleDirection * ScaleSpeed;
            if (scale > MaxScale)
            {
                scale = MaxScale;
                scaleDirection = -1;
            }
            else if (scale < MinScale)
            {
                scale = MinScale;
                scaleDirection = 1;
            }
        }

        public void Draw()
        {
            if (!IsActive) return;

            GL.Enable(EnableCap.Normalize);

            GL.Color3(1.0f, 0.84f, 0.0f);
            GL.PushMatrix();

            GL.Translate(PositionX, PositionY, PositionZ);
            GL.Rotate(90, 1, 0, 0);
            GL.Rotate(rotationAngle, 0, 0, 1);
            GL.Scale(scale, scale, scale);

            DrawCoin();

            GL.PopMatrix();

            GL.Disable(EnableCap.Normalize);
        }

        private void DrawCoin()
        {
            int segments = 32;

            // Боковая поверхность монетки 
            GL.Begin(PrimitiveType.QuadStrip);
            for (int i = 0; i <= segments; i++)
            {
                double angle = 2 * Math.PI * i / segments;
                double x = radius * Math.Cos(angle);
                double z = radius * Math.Sin(angle);

                GL.Normal3(Math.Cos(angle), 0, Math.Sin(angle));
                GL.Vertex3(x, thickness / 2, z);
                GL.Vertex3(x, -thickness / 2, z);
            }
            GL.End();

            // Верхняя крышка 
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Normal3(0, 1, 0);
            GL.Vertex3(0, thickness / 2, 0);

            for (int i = 0; i <= segments; i++)
            {
                double angle = 2 * Math.PI * i / segments;
                double x = radius * Math.Cos(angle);
                double z = radius * Math.Sin(angle);
                GL.Vertex3(x, thickness / 2, z);
            }
            GL.End();

            // Нижняя крышка
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Normal3(0, -1, 0);
            GL.Vertex3(0, -thickness / 2, 0);

            for (int i = 0; i <= segments; i++)
            {
                double angle = 2 * Math.PI * i / segments;
                double x = radius * Math.Cos(angle);
                double z = radius * Math.Sin(angle);
                GL.Vertex3(x, -thickness / 2, z);
            }
            GL.End();
        }
    }
}