using System.Collections.Generic;
using OpenTK.Mathematics;

namespace Riddle_Shutova_PRI122.Objects
{
    public class CoinExplosion
    {
        private List<CoinParticle> particles;
        private bool active;

        public bool IsActive => active && particles.Count > 0;

        public CoinExplosion(Vector3 position, System.Random rand, int particleCount = 20, float lifeTime = 1.2f, float speed = 3.0f)
        {
            particles = new List<CoinParticle>();
            active = true;

            for (int i = 0; i < particleCount; i++)
            {
                float theta = (float)(rand.NextDouble() * 2 * System.Math.PI);
                float phi = (float)(rand.NextDouble() * System.Math.PI);
                Vector3 dir = new Vector3(
                    (float)(System.Math.Sin(phi) * System.Math.Cos(theta)),
                    (float)(System.Math.Sin(phi) * System.Math.Sin(theta)),
                    (float)(System.Math.Cos(phi))
                );
                float vel = (float)(rand.NextDouble() * speed + 1.0);
                Vector3 velocity = dir * vel;

                float size = (float)(rand.NextDouble() * 0.1 + 0.05);

                float brightness = (float)(rand.NextDouble() * 0.5 + 0.5);
                Vector3 color = new Vector3(1.0f, 0.84f * brightness, 0.0f);

                float partLife = (float)(rand.NextDouble() * lifeTime + 0.2);

                var particle = new CoinParticle(position, velocity, partLife, size, color, rand);
                particles.Add(particle);
            }
        }

        public void Update(float deltaTime)
        {
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                particles[i].Update(deltaTime);
                if (!particles[i].IsAlive)
                    particles.RemoveAt(i);
            }
            if (particles.Count == 0)
                active = false;
        }

        public void Draw()
        {
            foreach (var p in particles)
                p.Draw();
        }
    }
}