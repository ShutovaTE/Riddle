using OpenTK.Graphics.OpenGL;
using System;

namespace Riddle_Shutova_PRI122.Objects
{
    public class Daughter
    {
        public float JumpOffset { get; set; }
        public float RotationY { get; set; } = 0f;

        public void Draw()
        {
            GL.Enable(EnableCap.Normalize);

            GL.PushMatrix();
            GL.Translate(0, JumpOffset, 0);
            GL.Rotate(RotationY, 0, 1, 0);

            // 1. Голова - полушар телесного цвета 
            GL.Color3(1.0f, 0.9f, 0.8f);
            GL.PushMatrix();
            GL.Translate(0, 0.4f, 0);
            GL.Rotate(-90, 1, 0, 0);
            DrawHemisphere(0.4f, 20, 20);

            // 2. Волосы - оранжевый цилиндр
            GL.Color3(1.0f, 0.5f, 0.0f);
            GL.Translate(0, 0, 0.1f);
            DrawCylinder(0.4f, 0.1f, 20);
            GL.PopMatrix();

            // 3. Хвостики - оранжевые конусы

            // Левый хвостик
            GL.PushMatrix();
            GL.Translate(-0.65f, 0.8f, 0f);
            GL.Rotate(220, 0, 0, 1);
            GL.Rotate(-90, 1, 0, 0);
            DrawCone(0.2f, 0.5f, 20);
            GL.PopMatrix();

            // Правый хвостик
            GL.PushMatrix();
            GL.Translate(0.7f, 0.75f, 0f);
            GL.Rotate(130, 0, 0, 1);
            GL.Rotate(-90, 1, 0, 0);
            DrawCone(0.2f, 0.5f, 20);
            GL.PopMatrix();

            // 4. Тело - белый конус
            GL.Color3(1.0f, 1.0f, 1.0f);
            GL.PushMatrix();
            GL.Translate(0, -0.8f, 0);
            GL.Rotate(270, 1, 0, 0);
            DrawCone(0.4f, 0.9f, 20);
            GL.PopMatrix();

            // 5. Руки - вытянутые по высоте параллелепипеды телесного цвета 
            GL.Color3(1.0f, 0.9f, 0.8f);

            // Левая рука 
            GL.PushMatrix();
            GL.Translate(-0.35f, -0.3f, 0f);
            GL.Rotate(-60, 0, 0, 1);
            GL.Scale(0.05f, 0.4f, 0.05f);
            DrawCube();
            GL.PopMatrix();

            // Правая рука 
            GL.PushMatrix();
            GL.Translate(0.35f, -0.3f, 0f);
            GL.Rotate(60, 0, 0, 1);
            GL.Scale(0.05f, 0.4f, 0.05f);
            DrawCube();
            GL.PopMatrix();

            // 6. Ноги - красные конусы
            GL.Color3(1.0f, 0.0f, 0.0f);

            // Левая нога
            GL.PushMatrix();
            GL.Translate(-0.12f, -0.8f, 0f);
            GL.Rotate(90, 1, 0, 0);
            DrawCone(0.12f, 0.6f, 20);
            GL.PopMatrix();

            // Правая нога
            GL.PushMatrix();
            GL.Translate(0.12f, -0.8f, 0f);
            GL.Rotate(90, 1, 0, 0);
            DrawCone(0.12f, 0.6f, 20);
            GL.PopMatrix();

            GL.PopMatrix(); 
            GL.Disable(EnableCap.Normalize);
        }

        private void DrawCylinder(float radius, float height, int segments)
        {
            double angle = 2 * Math.PI / segments;

            // Боковая поверхность цилиндра
            GL.Begin(PrimitiveType.QuadStrip);
            for (int i = 0; i <= segments; i++)
            {
                double x = radius * Math.Cos(i * angle);
                double y = radius * Math.Sin(i * angle);

                GL.Normal3(Math.Cos(i * angle), Math.Sin(i * angle), 0);

                GL.Vertex3(x, y, 0);
                GL.Vertex3(x, y, -height);
            }
            GL.End();

            // Верхнее основание цилиндра
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Normal3(0, 0, 1);
            GL.Vertex3(0, 0, 0);

            for (int i = 0; i <= segments; i++)
            {
                double x = radius * Math.Cos(i * angle);
                double y = radius * Math.Sin(i * angle);
                GL.Vertex3(x, y, 0);
            }
            GL.End();

            // Нижнее основание цилиндра
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Normal3(0, 0, -1);
            GL.Vertex3(0, 0, -height);

            for (int i = 0; i <= segments; i++)
            {
                double x = radius * Math.Cos(i * angle);
                double y = radius * Math.Sin(i * angle);
                GL.Vertex3(x, y, -height);
            }
            GL.End();
        }

        private void DrawHemisphere(float radius, int slices, int stacks)
        {
            for (int i = 0; i <= stacks / 2; i++)
            {
                double lat0 = Math.PI * (-0.5 + (double)(i - 1) / stacks);
                double z0 = Math.Sin(lat0) * radius;
                double zr0 = Math.Cos(lat0) * radius;

                double lat1 = Math.PI * (-0.5 + (double)i / stacks);
                double z1 = Math.Sin(lat1) * radius;
                double zr1 = Math.Cos(lat1) * radius;

                GL.Begin(PrimitiveType.QuadStrip);
                for (int j = 0; j <= slices; j++)
                {
                    double lng = 2 * Math.PI * (double)j / slices;
                    double x = Math.Cos(lng);
                    double y = Math.Sin(lng);

                    GL.Normal3(x * zr1, y * zr1, z1);
                    GL.Vertex3(x * zr1, y * zr1, z1);
                    GL.Normal3(x * zr0, y * zr0, z0);
                    GL.Vertex3(x * zr0, y * zr0, z0);
                }
                GL.End();
            }
        }

        private void DrawCone(float baseRadius, float height, int segments)
        {
            double angle = 2 * Math.PI / segments;

            GL.Begin(PrimitiveType.TriangleFan);
            GL.Normal3(0, 0, 1);
            GL.Vertex3(0, 0, height);
            for (int i = 0; i <= segments; i++)
            {
                double x = baseRadius * Math.Cos(i * angle);
                double y = baseRadius * Math.Sin(i * angle);
                GL.Normal3(Math.Cos(i * angle), Math.Sin(i * angle), 0);
                GL.Vertex3(x, y, 0);
            }
            GL.End();

            GL.Begin(PrimitiveType.TriangleFan);
            GL.Normal3(0, 0, -1);
            GL.Vertex3(0, 0, 0);
            for (int i = 0; i <= segments; i++)
            {
                double x = baseRadius * Math.Cos(i * angle);
                double y = baseRadius * Math.Sin(i * angle);
                GL.Vertex3(x, y, 0);
            }
            GL.End();
        }

        private void DrawCube()
        {
            GL.Begin(PrimitiveType.Quads);

            GL.Normal3(0, 0, 1);
            GL.Vertex3(-1, -1, 1);
            GL.Vertex3(1, -1, 1);
            GL.Vertex3(1, 1, 1);
            GL.Vertex3(-1, 1, 1);

            GL.Normal3(0, 0, -1);
            GL.Vertex3(-1, -1, -1);
            GL.Vertex3(-1, 1, -1);
            GL.Vertex3(1, 1, -1);
            GL.Vertex3(1, -1, -1);

            GL.Normal3(0, 1, 0);
            GL.Vertex3(-1, 1, -1);
            GL.Vertex3(-1, 1, 1);
            GL.Vertex3(1, 1, 1);
            GL.Vertex3(1, 1, -1);

            GL.Normal3(0, -1, 0);
            GL.Vertex3(-1, -1, -1);
            GL.Vertex3(1, -1, -1);
            GL.Vertex3(1, -1, 1);
            GL.Vertex3(-1, -1, 1);

            GL.Normal3(1, 0, 0);
            GL.Vertex3(1, -1, -1);
            GL.Vertex3(1, 1, -1);
            GL.Vertex3(1, 1, 1);
            GL.Vertex3(1, -1, 1);

            GL.Normal3(-1, 0, 0);
            GL.Vertex3(-1, -1, -1);
            GL.Vertex3(-1, -1, 1);
            GL.Vertex3(-1, 1, 1);
            GL.Vertex3(-1, 1, -1);

            GL.End();
        }
    }
}