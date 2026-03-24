using OpenTK.Graphics.OpenGL;
using System;

namespace Riddle_Shutova_PRI122.Objects
{
    public class Dog
    {

        public float JumpOffset { get; set; }
        public float RotationY { get; set; } = 0f;
        public void Draw()
        {
            GL.Enable(EnableCap.Normalize);

            // Правее дочки на 2.5f
            GL.PushMatrix();
            GL.Translate(2.5f, -0.3f + JumpOffset, 0);
            GL.Rotate(RotationY, 0, 1, 0);

            // 1. Тело - большая капсула белого цвета
            GL.Color3(1.0f, 1.0f, 1.0f);
            GL.PushMatrix();
            GL.Translate(0, -0.3f, 0);
            GL.Rotate(90, 0, 1, 0);
            DrawCapsule(0.5f, 2.0f, 20);
            GL.PopMatrix();

            // 2. Лапы - четыре капсулы среднего размера
            // Передние лапы
            GL.PushMatrix();
            GL.Translate(0.7f, -0.8f, 0.3f);
            GL.Rotate(90, 1, 0, 0);
            DrawCapsule(0.15f, 0.6f, 20);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(0.7f, -0.8f, -0.3f);
            GL.Rotate(90, 1, 0, 0);
            DrawCapsule(0.15f, 0.6f, 20);
            GL.PopMatrix();

            // Задние лапы
            GL.PushMatrix();
            GL.Translate(-0.7f, -0.8f, 0.3f);
            GL.Rotate(90, 1, 0, 0);
            DrawCapsule(0.15f, 0.6f, 20);
            GL.PopMatrix();

            GL.PushMatrix();
            GL.Translate(-0.7f, -0.8f, -0.3f);
            GL.Rotate(90, 1, 0, 0);
            DrawCapsule(0.15f, 0.6f, 20);
            GL.PopMatrix();

            // 3. Голова - шар белого цвета
            GL.PushMatrix();
            GL.Translate(1.2f, 0.2f, 0);
            DrawSphere(0.4f, 20, 20);
            GL.PopMatrix();

            // 4. Уши - эллипсы коричневого цвета
            GL.Color3(0.6f, 0.4f, 0.2f);

            // Левое ухо
            GL.PushMatrix();
            GL.Translate(1.1f, 0.1f, 0.4f);
            GL.Rotate(-190, 0, 0, 1);
            GL.Rotate(-170, 1, 0, 0);
            DrawEllipsoid(0.3f, 0.3f, 0.05f, 20, 20);
            GL.PopMatrix();

            // Правое ухо
            GL.PushMatrix();
            GL.Translate(1.1f, 0.1f, -0.4f);
            GL.Rotate(220, 0, 0, 1);
            GL.Rotate(160, 1, 0, 0);
            DrawEllipsoid(0.3f, 0.3f, 0.05f, 20, 20);
            GL.PopMatrix();

            // 5. Нос - белый эллипсоид с отсечённой верхней половиной и закрытый сверху диском
            GL.Color3(1.0f, 1.0f, 1.0f);
            GL.PushMatrix();
            GL.Translate(1.5f, 0.2f, 0);
            GL.Rotate(90, 1, 0, 0);

            DrawHalfEllipsoid(0.4f, 0.3f, 0.3f, 20, 20);

            GL.PushMatrix();
            GL.Translate(0, 0, -0.046f); 
            DrawEllipticalDisk(0.4f, 0.3f, 20);
            GL.PopMatrix();

            GL.PopMatrix();

            // 6. Хвост - коричневая усечённая пирамида с небольшой толщиной
            GL.Color3(0.6f, 0.4f, 0.2f);
            GL.PushMatrix();
            GL.Translate(-1.6f, -0.7f, 0); 
            GL.Rotate(-45, 0, 0, 1); 
            GL.Rotate(-90, 0, 1, 0);
            DrawTrapezoid(0.5f, 0.1f, 1.1f, 0.05f, 20); 
            GL.PopMatrix();

            GL.PopMatrix();
            GL.Disable(EnableCap.Normalize);
        }

        private void DrawTrapezoid(float bottomWidth, float topWidth, float height, float thickness, int segments)
        {
            // Передняя грань
            GL.Begin(PrimitiveType.Quads);
            GL.Normal3(0, 0, 1);
            GL.Vertex3(-bottomWidth / 2, 0, thickness / 2);
            GL.Vertex3(bottomWidth / 2, 0, thickness / 2);
            GL.Vertex3(topWidth / 2, height, thickness / 2);
            GL.Vertex3(-topWidth / 2, height, thickness / 2);
            GL.End();

            // Задняя грань
            GL.Begin(PrimitiveType.Quads);
            GL.Normal3(0, 0, -1);
            GL.Vertex3(-bottomWidth / 2, 0, -thickness / 2);
            GL.Vertex3(-topWidth / 2, height, -thickness / 2);
            GL.Vertex3(topWidth / 2, height, -thickness / 2);
            GL.Vertex3(bottomWidth / 2, 0, -thickness / 2);
            GL.End();

            // Боковые грани
            GL.Begin(PrimitiveType.Quads);
            // Левая боковая грань
            GL.Normal3(-1, 0, 0);
            GL.Vertex3(-bottomWidth / 2, 0, -thickness / 2);
            GL.Vertex3(-bottomWidth / 2, 0, thickness / 2);
            GL.Vertex3(-topWidth / 2, height, thickness / 2);
            GL.Vertex3(-topWidth / 2, height, -thickness / 2);

            // Правая боковая грань
            GL.Normal3(1, 0, 0);
            GL.Vertex3(bottomWidth / 2, 0, -thickness / 2);
            GL.Vertex3(topWidth / 2, height, -thickness / 2);
            GL.Vertex3(topWidth / 2, height, thickness / 2);
            GL.Vertex3(bottomWidth / 2, 0, thickness / 2);
            GL.End();

            // Верхняя грань
            GL.Begin(PrimitiveType.Quads);
            GL.Normal3(0, 1, 0);
            GL.Vertex3(-topWidth / 2, height, -thickness / 2);
            GL.Vertex3(-topWidth / 2, height, thickness / 2);
            GL.Vertex3(topWidth / 2, height, thickness / 2);
            GL.Vertex3(topWidth / 2, height, -thickness / 2);
            GL.End();

            // Нижняя грань
            GL.Begin(PrimitiveType.Quads);
            GL.Normal3(0, -1, 0);
            GL.Vertex3(-bottomWidth / 2, 0, -thickness / 2);
            GL.Vertex3(bottomWidth / 2, 0, -thickness / 2);
            GL.Vertex3(bottomWidth / 2, 0, thickness / 2);
            GL.Vertex3(-bottomWidth / 2, 0, thickness / 2);
            GL.End();
        }

        private void DrawCapsule(float radius, float height, int segments)
        {
            // Центральный цилиндр 
            float cylinderHeight = height - 2 * radius;
            GL.PushMatrix();
            GL.Translate(0, 0, -cylinderHeight / 2);
            DrawCylinder(radius, cylinderHeight, segments);
            GL.PopMatrix();

            // Передняя сфера
            GL.PushMatrix();
            GL.Translate(0, 0, cylinderHeight / 2);
            DrawSphere(radius, segments, segments);
            GL.PopMatrix();

            // Задняя сфера
            GL.PushMatrix();
            GL.Translate(0, 0, -cylinderHeight / 2);
            DrawSphere(radius, segments, segments);
            GL.PopMatrix();
        }

        private void DrawSphere(float radius, int slices, int stacks)
        {
            for (int i = 0; i <= stacks; i++)
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

        private void DrawEllipsoid(float radiusX, float radiusY, float radiusZ, int slices, int stacks)
        {
            for (int i = 0; i <= stacks; i++)
            {
                double lat0 = Math.PI * (-0.5 + (double)(i - 1) / stacks);
                double z0 = Math.Sin(lat0) * radiusZ;
                double zr0 = Math.Cos(lat0);

                double lat1 = Math.PI * (-0.5 + (double)i / stacks);
                double z1 = Math.Sin(lat1) * radiusZ;
                double zr1 = Math.Cos(lat1);

                GL.Begin(PrimitiveType.QuadStrip);
                for (int j = 0; j <= slices; j++)
                {
                    double lng = 2 * Math.PI * (double)j / slices;
                    double x = Math.Cos(lng) * radiusX;
                    double y = Math.Sin(lng) * radiusY;

                    GL.Normal3(x * zr1, y * zr1, z1);
                    GL.Vertex3(x * zr1, y * zr1, z1);
                    GL.Normal3(x * zr0, y * zr0, z0);
                    GL.Vertex3(x * zr0, y * zr0, z0);
                }
                GL.End();
            }
        }

        private void DrawHalfEllipsoid(float radiusX, float radiusY, float radiusZ, int slices, int stacks)
        {
            for (int i = stacks / 2; i <= stacks; i++)
            {
                double lat0 = Math.PI * (-0.5 + (double)(i - 1) / stacks);
                double z0 = Math.Sin(lat0) * radiusZ;
                double zr0 = Math.Cos(lat0);

                double lat1 = Math.PI * (-0.5 + (double)i / stacks);
                double z1 = Math.Sin(lat1) * radiusZ;
                double zr1 = Math.Cos(lat1);

                GL.Begin(PrimitiveType.QuadStrip);
                for (int j = 0; j <= slices; j++)
                {
                    double lng = 2 * Math.PI * (double)j / slices;
                    double x = Math.Cos(lng) * radiusX;
                    double y = Math.Sin(lng) * radiusY;

                    GL.Normal3(x * zr1, y * zr1, z1);
                    GL.Vertex3(x * zr1, y * zr1, z1);
                    GL.Normal3(x * zr0, y * zr0, z0);
                    GL.Vertex3(x * zr0, y * zr0, z0);
                }
                GL.End();
            }
        }

        private void DrawEllipticalDisk(float radiusX, float radiusY, int segments)
        {
            double angle = 2 * Math.PI / segments;

            GL.Begin(PrimitiveType.TriangleFan);
            GL.Normal3(0, 0, 1); 
            GL.Vertex3(0, 0, 0); 

            for (int i = 0; i <= segments; i++)
            {
                double x = radiusX * Math.Cos(i * angle);
                double y = radiusY * Math.Sin(i * angle);
                GL.Vertex3(x, y, 0);
            }
            GL.End();
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
                GL.Vertex3(x, y, height);
            }
            GL.End();

            // Верхнее основание цилиндра
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Normal3(0, 0, 1);
            GL.Vertex3(0, 0, height);
            for (int i = 0; i <= segments; i++)
            {
                double x = radius * Math.Cos(i * angle);
                double y = radius * Math.Sin(i * angle);
                GL.Vertex3(x, y, height);
            }
            GL.End();

            // Нижнее основание цилиндра
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Normal3(0, 0, -1);
            GL.Vertex3(0, 0, 0);
            for (int i = 0; i <= segments; i++)
            {
                double x = radius * Math.Cos(i * angle);
                double y = radius * Math.Sin(i * angle);
                GL.Vertex3(x, y, 0);
            }
            GL.End();
        }

    }
}