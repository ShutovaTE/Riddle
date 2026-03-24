using Riddle_Shutova_PRI122.Objects;
using System.Collections.Generic;

namespace Riddle_Shutova_PRI122.Managers
{
    public class CollisionManager
    {
        private Puppy puppy;
        private List<Obstacle> obstacles;

        private float puppyWidth = 0.4f;
        private float puppyHeight = 0.6f;
        private float puppyDepth = 0.3f;

        public CollisionManager(Puppy puppy, ref List<Obstacle> obstacles)
        {
            this.puppy = puppy;
            this.obstacles = obstacles;
        }

        public bool CheckPuppyCollision()
        {
            if (obstacles == null || puppy == null)
                return false;

            float puppyCenterX = 5.0f; 
            float puppyCenterY = -1.0f + puppy.JumpOffset;
            float puppyCenterZ = 0.0f;

            float puppyMinX = puppyCenterX - puppyWidth / 2;
            float puppyMaxX = puppyCenterX + puppyWidth / 2;
            float puppyMinY = puppyCenterY - puppyHeight / 2;
            float puppyMaxY = puppyCenterY + puppyHeight / 2;
            float puppyMinZ = puppyCenterZ - puppyDepth / 2;
            float puppyMaxZ = puppyCenterZ + puppyDepth / 2;

            foreach (var obstacle in obstacles)
            {
                if (!obstacle.IsActive) continue;

                float obstacleMinX = obstacle.PositionX - obstacle.Width / 2;
                float obstacleMaxX = obstacle.PositionX + obstacle.Width / 2;
                float obstacleMinY = obstacle.PositionY - obstacle.Height / 2;
                float obstacleMaxY = obstacle.PositionY + obstacle.Height / 2;
                float obstacleMinZ = -obstacle.Depth / 2; 
                float obstacleMaxZ = obstacle.Depth / 2;

                bool collisionX = puppyMaxX > obstacleMinX && puppyMinX < obstacleMaxX;
                bool collisionY = puppyMaxY > obstacleMinY && puppyMinY < obstacleMaxY;
                bool collisionZ = puppyMaxZ > obstacleMinZ && puppyMinZ < obstacleMaxZ;

                bool feetCollision = puppyMinY <= obstacleMaxY &&
                                   puppyMinY >= obstacleMaxY - 0.1f && 
                                   collisionX && collisionZ;

                if ((collisionX && collisionY && collisionZ) || feetCollision)
                {
                    return true; 
                }
            }

            return false;
        }
    }
}