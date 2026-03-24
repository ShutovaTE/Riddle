using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Riddle_Shutova_PRI122.Objects
{
    public class SurfaceOfRevolution
    {
        private List<Vector3> vertices;
        private List<Vector3> normals;
        private List<uint> indices;

        private Vector3 position;
        private float scale;
        private float rotationY;
        private Vector3 color;
        private float alpha;

        public SurfaceOfRevolution(Vector3[] controlPoints, int steps, int slices,
            Vector3 pos, float scale, float rotY, Vector3 col, float alpha = 1.0f)
        {
            this.position = pos;
            this.scale = scale;
            this.rotationY = rotY;
            this.color = col;
            this.alpha = alpha;

            BuildMesh(controlPoints, steps, slices);
        }

        private void BuildMesh(Vector3[] controlPoints, int steps, int slices)
        {
            vertices = new List<Vector3>();
            normals = new List<Vector3>();
            indices = new List<uint>();

            List<Vector2> profile = new List<Vector2>();
            for (int i = 0; i <= steps; i++)
            {
                float t = i / (float)steps;
                Vector2 point = Bezier(controlPoints, t);
                profile.Add(point);
            }

            for (int i = 0; i <= steps; i++)
            {
                float radius = profile[i].X;
                float height = profile[i].Y;

                for (int j = 0; j <= slices; j++)
                {
                    float angle = j * 2 * (float)Math.PI / slices;
                    float cosA = (float)Math.Cos(angle);
                    float sinA = (float)Math.Sin(angle);

                    float x = radius * cosA;
                    float z = radius * sinA;
                    float y = height;

                    vertices.Add(new Vector3(x, y, z));

                    normals.Add(new Vector3(cosA, 0, sinA).Normalized());
                }
            }

            for (int i = 0; i < steps; i++)
            {
                for (int j = 0; j < slices; j++)
                {
                    uint a = (uint)(i * (slices + 1) + j);
                    uint b = (uint)(i * (slices + 1) + j + 1);
                    uint c = (uint)((i + 1) * (slices + 1) + j);
                    uint d = (uint)((i + 1) * (slices + 1) + j + 1);

                    indices.Add(a);
                    indices.Add(b);
                    indices.Add(c);

                    indices.Add(b);
                    indices.Add(d);
                    indices.Add(c);
                }
            }
        }

        private Vector2 Bezier(Vector3[] points, float t)
        {
            // Кубическая кривая Безье
            float x = (float)(
                Math.Pow(1 - t, 3) * points[0].X +
                3 * Math.Pow(1 - t, 2) * t * points[1].X +
                3 * (1 - t) * Math.Pow(t, 2) * points[2].X +
                Math.Pow(t, 3) * points[3].X
            );
            float y = (float)(
                Math.Pow(1 - t, 3) * points[0].Y +
                3 * Math.Pow(1 - t, 2) * t * points[1].Y +
                3 * (1 - t) * Math.Pow(t, 2) * points[2].Y +
                Math.Pow(t, 3) * points[3].Y
            );
            return new Vector2(x, y);
        }

        public void Draw()
        {
            GL.PushMatrix();
            GL.Translate(position);
            GL.Rotate(rotationY, 0, 1, 0);
            GL.Scale(scale, scale, scale);

            GL.Color4(color.X, color.Y, color.Z, alpha);
            GL.Enable(EnableCap.Normalize); 

            GL.Begin(PrimitiveType.Triangles);
            for (int i = 0; i < indices.Count; i += 3)
            {
                Vector3 v0 = vertices[(int)indices[i]];
                Vector3 v1 = vertices[(int)indices[i + 1]];
                Vector3 v2 = vertices[(int)indices[i + 2]];

                Vector3 n0 = normals[(int)indices[i]];
                Vector3 n1 = normals[(int)indices[i + 1]];
                Vector3 n2 = normals[(int)indices[i + 2]];

                GL.Normal3(n0.X, n0.Y, n0.Z);
                GL.Vertex3(v0);
                GL.Normal3(n1.X, n1.Y, n1.Z);
                GL.Vertex3(v1);
                GL.Normal3(n2.X, n2.Y, n2.Z);
                GL.Vertex3(v2);
            }
            GL.End();

            GL.Disable(EnableCap.Normalize);
            GL.PopMatrix();
        }
    }
}