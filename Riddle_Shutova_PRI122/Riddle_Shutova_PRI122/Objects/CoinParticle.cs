using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Riddle_Shutova_PRI122.Objects
{
    public class CoinParticle
    {
        private Vector3 position;
        private Vector3 velocity;
        private float size;
        private float life;
        private float rotationAngle;
        private Vector3 rotationAxis;
        private float rotationSpeed;
        private Vector3 color;

        public bool IsAlive => life > 0;

        public CoinParticle(Vector3 startPos, Vector3 startVelocity, float startLife, float startSize, Vector3 startColor, System.Random rand)
        {
            position = startPos;
            velocity = startVelocity;
            life = startLife;
            size = startSize;
            color = startColor;

            rotationAngle = 0;
            rotationAxis = new Vector3((float)rand.NextDouble() * 2 - 1, (float)rand.NextDouble() * 2 - 1, (float)rand.NextDouble() * 2 - 1);
            if (rotationAxis.LengthSquared > 0) rotationAxis.Normalize();
            else rotationAxis = Vector3.UnitY;
            rotationSpeed = (float)rand.NextDouble() * 180f + 90f; 
        }

        public void Update(float deltaTime)
        {
            life -= deltaTime;
            position += velocity * deltaTime;
            velocity += new Vector3(0, -9.8f, 0) * deltaTime;
            rotationAngle += rotationSpeed * deltaTime;
        }

        public void Draw()
        {
            GL.PushMatrix();
            GL.Translate(position);
            GL.Rotate(rotationAngle, rotationAxis);
            GL.Scale(size, size, size);
            GL.Color3(color);
            DrawUnitCube();
            GL.PopMatrix();
        }

        private void DrawUnitCube()
        {
            GL.Begin(PrimitiveType.Quads);
            // Передняя грань
            GL.Vertex3(-0.5f, -0.5f, 0.5f);
            GL.Vertex3(0.5f, -0.5f, 0.5f);
            GL.Vertex3(0.5f, 0.5f, 0.5f);
            GL.Vertex3(-0.5f, 0.5f, 0.5f);
            // Задняя грань
            GL.Vertex3(-0.5f, -0.5f, -0.5f);
            GL.Vertex3(-0.5f, 0.5f, -0.5f);
            GL.Vertex3(0.5f, 0.5f, -0.5f);
            GL.Vertex3(0.5f, -0.5f, -0.5f);
            // Верхняя грань
            GL.Vertex3(-0.5f, 0.5f, -0.5f);
            GL.Vertex3(-0.5f, 0.5f, 0.5f);
            GL.Vertex3(0.5f, 0.5f, 0.5f);
            GL.Vertex3(0.5f, 0.5f, -0.5f);
            // Нижняя грань
            GL.Vertex3(-0.5f, -0.5f, -0.5f);
            GL.Vertex3(0.5f, -0.5f, -0.5f);
            GL.Vertex3(0.5f, -0.5f, 0.5f);
            GL.Vertex3(-0.5f, -0.5f, 0.5f);
            // Левая грань
            GL.Vertex3(-0.5f, -0.5f, -0.5f);
            GL.Vertex3(-0.5f, -0.5f, 0.5f);
            GL.Vertex3(-0.5f, 0.5f, 0.5f);
            GL.Vertex3(-0.5f, 0.5f, -0.5f);
            // Правая грань
            GL.Vertex3(0.5f, -0.5f, -0.5f);
            GL.Vertex3(0.5f, 0.5f, -0.5f);
            GL.Vertex3(0.5f, 0.5f, 0.5f);
            GL.Vertex3(0.5f, -0.5f, 0.5f);
            GL.End();
        }
    }
}