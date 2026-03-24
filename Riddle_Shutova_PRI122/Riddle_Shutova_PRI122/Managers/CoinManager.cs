using Riddle_Shutova_PRI122.Objects;
using System;
using System.Collections.Generic;

namespace Riddle_Shutova_PRI122.Managers
{
    public class CoinManager
    {
        private List<Coin> coins;
        private Random random;
        private float groundStartX;
        private float groundEndX;

        private int spawnCounter = 0;
        private int nextSpawnInterval;

        private float minHeight = 0.5f;
        private float maxHeight = 2.5f;

        public List<Coin> Coins => coins;

        public CoinManager(float startX, float endX, float startZ, float endZ)
        {
            coins = new List<Coin>();
            random = new Random();
            groundStartX = startX;
            groundEndX = endX;

            GenerateNewSpawnInterval();
        }

        public void Update()
        {
            for (int i = coins.Count - 1; i >= 0; i--)
            {
                coins[i].Update();

                if (coins[i].PositionX < groundStartX)
                {
                    coins.RemoveAt(i);
                }
            }

            spawnCounter++;
            if (spawnCounter >= nextSpawnInterval)
            {
                SpawnCoin();
                spawnCounter = 0;
                GenerateNewSpawnInterval();
            }
        }

        private void SpawnCoin()
        {
            float randomY = (float)(random.NextDouble() * (maxHeight - minHeight) + minHeight);

            Coin coin = new Coin(groundEndX, randomY);
            coins.Add(coin);
        }

        private void GenerateNewSpawnInterval()
        {
            nextSpawnInterval = random.Next(100, 251);
        }

        public void Draw()
        {
            foreach (var coin in coins)
            {
                coin.Draw();
            }
        }
    }
}