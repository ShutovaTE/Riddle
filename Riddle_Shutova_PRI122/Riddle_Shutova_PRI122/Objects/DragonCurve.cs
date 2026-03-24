using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Riddle_Shutova_PRI122.Objects
{
    public class DragonCurve
    {
        private List<Vector3> points;
        private float yOffset;
        private float posX, posZ;
        private float scale;
        private float rotation; 

        public DragonCurve(int iterations, float step, float groundYLevel, float centerX, float centerZ, float scale = 1.0f, float rotation = 0f)
        {
            this.yOffset = groundYLevel + 0.02f;
            this.posX = centerX;
            this.posZ = centerZ;
            this.scale = scale;
            this.rotation = rotation;

            var rawPoints = GeneratePoints(iterations, step);
            TransformPoints(rawPoints);
        }

        private List<Vector3> GeneratePoints(int iterations, float step)
        {
            string axiom = "FX";
            string current = axiom;
            for (int i = 0; i < iterations; i++)
            {
                string next = "";
                foreach (char c in current)
                {
                    if (c == 'X')
                        next += "X+YF+";
                    else if (c == 'Y')
                        next += "-FX-Y";
                    else
                        next += c;
                }
                current = next;
            }

            List<Vector3> pts = new List<Vector3>();
            float x = 0, z = 0;
            float angle = 0; 

            pts.Add(new Vector3(x, yOffset, z));

            foreach (char c in current)
            {
                if (c == 'F')
                {
                    float rad = MathHelper.DegreesToRadians(angle);
                    x += step * (float)Math.Cos(rad);
                    z += step * (float)Math.Sin(rad);
                    pts.Add(new Vector3(x, yOffset, z));
                }
                else if (c == '+')
                {
                    angle += 90;
                }
                else if (c == '-')
                {
                    angle -= 90;
                }
            }
            return pts;
        }

        private void TransformPoints(List<Vector3> rawPoints)
        {
            if (rawPoints.Count == 0) return;

            float minX = float.MaxValue, maxX = float.MinValue;
            float minZ = float.MaxValue, maxZ = float.MinValue;
            foreach (var p in rawPoints)
            {
                if (p.X < minX) minX = p.X;
                if (p.X > maxX) maxX = p.X;
                if (p.Z < minZ) minZ = p.Z;
                if (p.Z > maxZ) maxZ = p.Z;
            }

            float width = maxX - minX;
            float depth = maxZ - minZ;
            float maxDim = Math.Max(width, depth);

            float localScale = (maxDim > 0) ? (1.0f / maxDim) : 1.0f;

            float centerX_local = (minX + maxX) / 2;
            float centerZ_local = (minZ + maxZ) / 2;

            points = new List<Vector3>();
            float cosRot = (float)Math.Cos(MathHelper.DegreesToRadians(rotation));
            float sinRot = (float)Math.Sin(MathHelper.DegreesToRadians(rotation));

            foreach (var p in rawPoints)
            {
                float tx = p.X - centerX_local;
                float tz = p.Z - centerZ_local;

                tx *= localScale;
                tz *= localScale;

                tx *= scale;
                tz *= scale;

                float rx = tx * cosRot - tz * sinRot;
                float rz = tx * sinRot + tz * cosRot;

                rx += posX;
                rz += posZ;

                points.Add(new Vector3(rx, yOffset, rz));
            }
        }

        public void Draw()
        {
            GL.Color3(0.0f, 0.5f, 0.0f); 
            GL.LineWidth(2.0f);
            GL.Begin(PrimitiveType.LineStrip);
            foreach (var p in points)
            {
                GL.Vertex3(p.X, p.Y, p.Z);
            }
            GL.End();
            GL.LineWidth(1.0f);
        }
    }
}