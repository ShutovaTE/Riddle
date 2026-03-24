using OpenTK.Graphics.OpenGL;
using System;

namespace Riddle_Shutova_PRI122.Objects
{
    public class Dad
    {
        public float JumpOffset {  get; set; }   
        public float RotationY { get; set; } = 0f;   
        public void Draw()
        {
            GL.Enable(EnableCap.Normalize);

            // Левее дочки на 3f
            GL.PushMatrix();
            GL.Translate(-3.0f, 0 + JumpOffset, 0);
            GL.Rotate(RotationY, 0, 1, 0);

            // 1. Голова - вытянутый по высоте параллелепипед телесного цвета
            GL.Color3(1.0f, 0.9f, 0.8f);
            GL.PushMatrix();
            GL.Translate(0, 1.0f, 0);
            GL.Scale(0.2f, 0.3f, 0.2f); 
            DrawCube();
            GL.PopMatrix();

            // 2. Усы - вытянутый по ширине оранжевый параллелепипед
            GL.Color3(1.0f, 0.5f, 0.0f);
            GL.PushMatrix();
            GL.Translate(0, 1.0f, 0.2f); 
            GL.Scale(0.15f, 0.05f, 0.01f); 
            DrawCube();
            GL.PopMatrix();

            // 3. Шляпа
            GL.Color3(0.0f, 0.0f, 0.0f); 

            // 3.1) Дно шляпы - чёрный цилиндр очень маленькой толщины
            GL.PushMatrix();
            GL.Translate(0, 1.3f, 0); 
            GL.Rotate(-90, 1, 0, 0); 
            DrawCylinder(0.5f, 0.05f, 20); 
            GL.PopMatrix();

            // 3.2) Верх шляпы - чёрный куб
            GL.PushMatrix();
            GL.Translate(0, 1.5f, 0); 
            GL.Scale(0.2f, 0.15f, 0.2f);
            DrawCube();
            GL.PopMatrix();

            // 4. Тело - оранжевый параллелепипед
            GL.Color3(1.0f, 0.5f, 0.0f);
            GL.PushMatrix();
            GL.Translate(0, 0.4f, 0);
            GL.Scale(0.2f, 0.3f, 0.2f); 
            DrawCube();
            GL.PopMatrix();

            // 5. Руки - длинные параллелепипеды
            // Левая рука 
            GL.PushMatrix();
            GL.Translate(-0.4f, 0.1f, 0);

            // Оранжевая часть руки (верх)
            GL.Color3(1.0f, 0.5f, 0.0f);
            GL.PushMatrix();
            GL.Translate(0.05f, 0.4f, 0.2f);
            GL.Rotate(-60, 1, 0, 0);
            GL.Rotate(-30, 0, 0, 1);
            GL.Scale(0.08f, 0.3f, 0.08f);
            DrawCube();
            GL.PopMatrix();

            // Телесная часть руки (низ)
            GL.Color3(1.0f, 0.9f, 0.8f);
            GL.PushMatrix();
            GL.Translate(-0.22f, 0.16f, 0.6f);
            GL.Rotate(-60, 1, 0, 0);
            GL.Rotate(-30, 0, 0, 1);
            GL.Scale(0.07f, 0.3f, 0.07f);
            DrawCube();
            GL.PopMatrix();

            GL.PopMatrix();

            // Правая рука 
            GL.PushMatrix();
            GL.Translate(0.4f, 0.1f, 0);

            // Оранжевая часть руки (верх)
            GL.Color3(1.0f, 0.5f, 0.0f);
            GL.PushMatrix();
            GL.Translate(0, 0.4f, 0);
            GL.Rotate(60, 0, 0, 1);
            GL.Scale(0.08f, 0.3f, 0.08f);
            DrawCube();
            GL.PopMatrix();

            // Телесная часть руки (низ)
            GL.Color3(1.0f, 0.9f, 0.8f);
            GL.PushMatrix();
            GL.Translate(0.5f, 0.1f, 0);
            GL.Rotate(60, 0, 0, 1);
            GL.Scale(0.07f, 0.3f, 0.07f);
            DrawCube();
            GL.PopMatrix();

            GL.PopMatrix();

            // 6. Ноги - вытянутые по высоте параллелепипеды серого цвета
            GL.Color3(0.5f, 0.5f, 0.5f);

            // Левая нога
            GL.PushMatrix();
            GL.Translate(-0.12f, -0.65f, 0);
            GL.Scale(0.07f, 0.75f, 0.07f);
            DrawCube();
            GL.PopMatrix();

            // Правая нога
            GL.PushMatrix();
            GL.Translate(0.12f, -0.65f, 0);
            GL.Scale(0.08f, 0.75f, 0.07f);
            DrawCube();
            GL.PopMatrix();

            // 7. Газета - два больших параллелограмма белого цвета
            GL.Color3(1.0f, 1.0f, 1.0f);

            // Первый лист газеты
            GL.PushMatrix();
            GL.Translate(0.15f, -0.2f, 0.9f); 
            GL.Rotate(20, 0, 1, 0); 
            DrawParallelogram(1.0f, 1.3f, 0.01f);
            GL.PopMatrix();

            // Второй лист газеты 
            GL.PushMatrix();
            GL.Translate(-0.8f, -0.1f, 0.8f); 
            GL.Rotate(-10, 0, 1, 1); 
            DrawParallelogram(1.0f, 1.3f, 0.01f);
            GL.PopMatrix();

            GL.PopMatrix();
            GL.Disable(EnableCap.Normalize);
        }

        private void DrawParallelogram(float width, float height, float thickness)
        {
            // Передняя грань
            GL.Begin(PrimitiveType.Quads);
            GL.Normal3(0, 0, 1);
            GL.Vertex3(0, 0, thickness / 2);
            GL.Vertex3(width, 0, thickness / 2);
            GL.Vertex3(width * 0.8f, height, thickness / 2); 
            GL.Vertex3(0, height, thickness / 2);
            GL.End();

            // Задняя грань
            GL.Begin(PrimitiveType.Quads);
            GL.Normal3(0, 0, -1);
            GL.Vertex3(0, 0, -thickness / 2);
            GL.Vertex3(0, height, -thickness / 2);
            GL.Vertex3(width * 0.8f, height, -thickness / 2);
            GL.Vertex3(width, 0, -thickness / 2);
            GL.End();

            // Боковые грани
            GL.Begin(PrimitiveType.Quads);
            // Левая боковая грань
            GL.Normal3(-1, 0, 0);
            GL.Vertex3(0, 0, -thickness / 2);
            GL.Vertex3(0, 0, thickness / 2);
            GL.Vertex3(0, height, thickness / 2);
            GL.Vertex3(0, height, -thickness / 2);

            // Правая боковая грань 
            GL.Normal3(0.8f, 0.6f, 0); 
            GL.Vertex3(width, 0, -thickness / 2);
            GL.Vertex3(width * 0.8f, height, -thickness / 2);
            GL.Vertex3(width * 0.8f, height, thickness / 2);
            GL.Vertex3(width, 0, thickness / 2);

            // Верхняя грань
            GL.Normal3(0, 1, 0);
            GL.Vertex3(0, height, -thickness / 2);
            GL.Vertex3(0, height, thickness / 2);
            GL.Vertex3(width * 0.8f, height, thickness / 2);
            GL.Vertex3(width * 0.8f, height, -thickness / 2);

            // Нижняя грань
            GL.Normal3(0, -1, 0);
            GL.Vertex3(0, 0, -thickness / 2);
            GL.Vertex3(width, 0, -thickness / 2);
            GL.Vertex3(width, 0, thickness / 2);
            GL.Vertex3(0, 0, thickness / 2);
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