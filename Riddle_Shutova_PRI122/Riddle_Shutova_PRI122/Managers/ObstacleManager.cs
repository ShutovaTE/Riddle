using Riddle_Shutova_PRI122.Objects;
using System;
using System.Collections.Generic;

namespace Riddle_Shutova_PRI122.Managers
{
    public class ObstacleManager
    {
        private List<Obstacle> obstacles;
        private Random random;
        private float groundStartX;
        private float groundEndX;

        private int spawnCounter = 0;
        private int nextSpawnInterval;

        public List<Obstacle> Obstacles => obstacles;

        public ObstacleManager(float startX, float endX, float startZ, float endZ)
        {
            obstacles = new List<Obstacle>();
            random = new Random();
            groundStartX = startX;
            groundEndX = endX;

            GenerateNewSpawnInterval();
        }

        public void Update()
        {
            for (int i = obstacles.Count - 1; i >= 0; i--)
            {
                obstacles[i].Update();

                if (obstacles[i].PositionX < groundStartX)
                {
                    obstacles.RemoveAt(i);
                }
            }

            spawnCounter++;
            if (spawnCounter >= nextSpawnInterval)
            {
                SpawnObstacle();
                spawnCounter = 0;
                GenerateNewSpawnInterval();
            }
        }

        private void SpawnObstacle()
        {
            Obstacle obstacle = new Obstacle(groundEndX);
            obstacles.Add(obstacle);
        }

        private void GenerateNewSpawnInterval()
        {
            nextSpawnInterval = random.Next(60, 181);
        }

        public void Draw()
        {
            foreach (var obstacle in obstacles)
            {
                obstacle.Draw();
            }
        }
    }
}