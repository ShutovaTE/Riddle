using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Riddle_Shutova_PRI122.Objects
{
    public class CloudCluster
    {
        private List<SurfaceOfRevolution> parts;

        public CloudCluster(Vector3 center, Random rand)
        {
            parts = new List<SurfaceOfRevolution>();

            // Три типа профилей: приплюснутый, сферический, маленький округлый
            Vector3[][] profiles = new Vector3[][]
            {
                // Приплюснутый (низ) – широкий, низкий
                new Vector3[] {
                    new Vector3(0.0f, 0.0f, 0),
                    new Vector3(0.8f, 0.2f, 0),
                    new Vector3(1.0f, 0.4f, 0),
                    new Vector3(0.7f, 0.5f, 0)
                },
                // Сферический (середина)
                new Vector3[] {
                    new Vector3(0.0f, 0.0f, 0),
                    new Vector3(0.6f, 0.3f, 0),
                    new Vector3(0.8f, 0.7f, 0),
                    new Vector3(0.5f, 1.0f, 0)
                },
                // Маленький округлый (верх)
                new Vector3[] {
                    new Vector3(0.0f, 0.0f, 0),
                    new Vector3(0.4f, 0.2f, 0),
                    new Vector3(0.5f, 0.6f, 0),
                    new Vector3(0.3f, 0.8f, 0)
                }
            };

            int partCount = rand.Next(5, 8); 

            for (int i = 0; i < partCount; i++)
            {
                int type;
                float r = (float)rand.NextDouble();
                if (r < 0.3)
                    type = 0;
                else if (r < 0.7)
                    type = 1;
                else
                    type = 2;

                Vector3[] profile = profiles[type];

                float baseScale;
                if (type == 0)
                    baseScale = 1.2f + (float)rand.NextDouble() * 0.5f; 
                else if (type == 1)
                    baseScale = 0.9f + (float)rand.NextDouble() * 0.4f; 
                else
                    baseScale = 0.6f + (float)rand.NextDouble() * 0.3f; 

                float offsetX = (float)(rand.NextDouble() * 4 - 2);     
                float offsetY;
                if (type == 0)
                    offsetY = (float)(rand.NextDouble() * 0.5 - 0.2);    
                else if (type == 1)
                    offsetY = (float)(rand.NextDouble() * 0.5 + 0.3);    
                else
                    offsetY = (float)(rand.NextDouble() * 0.5 + 0.8);   

                float offsetZ = (float)(rand.NextDouble() * 1.0 - 0.5);  

                Vector3 partPos = center + new Vector3(offsetX, offsetY, offsetZ);

                float rotY = (float)(rand.NextDouble() * 360);

                float brightness = 0.8f + (float)rand.NextDouble() * 0.2f;
                Vector3 color = new Vector3(brightness, brightness, brightness);
                float alpha = 0.7f + (float)rand.NextDouble() * 0.2f;

                var part = new SurfaceOfRevolution(profile, 12, 8, partPos, baseScale, rotY, color, alpha);
                parts.Add(part);
            }
        }

        public void Draw()
        {
            foreach (var part in parts)
                part.Draw();
        }
    }
}