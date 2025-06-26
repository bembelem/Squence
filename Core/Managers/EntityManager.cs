using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Squence.Core.Interfaces;
using Squence.Core.States;
using Squence.Entities;
using System;
using System.Collections.Generic;

namespace Squence.Core.Managers
{
    internal class EntityManager(GameState gameState, GraphicsDevice graphicsDevice) : IUpdatable, Interfaces.IDrawable
    {
        public Hero Hero { get; private set; } = new Hero(graphicsDevice);
        public readonly Dictionary<Guid, Bullet> Bullets = [];
        public readonly Dictionary<Guid, Enemy> Enemies = [];
        public readonly Dictionary<Guid, Coin> Coins = [];
        private readonly GameState _gameState = gameState;
        private readonly Random _random = new();

        public void Update(GameTime gameTime)
        {
            UpdateBullets(gameTime);
            UpdateEnemies(gameTime);
            UpdateCoins(gameTime);
        }

        public void Draw(DrawingManager drawingManager)
        {
            drawingManager.DrawRenderableEntity(Hero);
            drawingManager.DrawRenderableEntities(Bullets.Values);
            drawingManager.DrawRenderableEntities(Enemies.Values);
            drawingManager.DrawRenderableEntities(Coins.Values);
        }

        private void UpdateBullets(GameTime gameTime)
        {
            foreach (var bullet in Bullets.Values)
            {
                bullet.Update(gameTime);
            }
        }

        private void UpdateEnemies(GameTime gameTime)
        {
            foreach (var enemy in Enemies.Values)
            {
                if (enemy.HealthPoints <= 0)
                {
                    _gameState.HandleKillEnemy();
                    RemoveEnemy(enemy.Guid);
                    ScatterCoins(enemy);
                }
                else if (enemy.IsReachGoal)
                {
                    RemoveEnemy(enemy.Guid);
                    _gameState.HandleEnemyBreakthrough();
                }
                else
                {
                    enemy.Update(gameTime);
                }
            }
        }

        private void UpdateCoins(GameTime gameTime)
        {
            foreach (var coin in Coins.Values)
            {
                if (coin.IsDisappeared)
                {
                    RemoveCoin(coin.Guid);
                }
                else
                {
                    coin.Update(gameTime);
                }
            }
        }

        public void AddBullet(Bullet bullet)
        {
            Bullets[bullet.Guid] = bullet;
        }

        public void AddEnemy(Enemy enemy)
        {
            Enemies[enemy.Guid] = enemy;
        }

        public void AddCoin(Coin coin)
        {
            Coins[coin.Guid] = coin;
        }

        public void RemoveBullet(Guid guid)
        {
            Bullets.Remove(guid);
        }

        public void RemoveEnemy(Guid guid)
        {
            Enemies.Remove(guid);
        }

        public void RemoveCoin(Guid guid)
        {
            Coins.Remove(guid);
        }

        public void MoveHero(Vector2 direction, GameTime gameTime)
        {
            if (direction != Vector2.Zero)
            {
                direction.Normalize();
                Hero.Move(direction, gameTime);
            }
        }

        public void SetHeroDirectionType(DirectionType direction)
        {
            Hero.SetDirectionType(direction);
        }

        public void HitEnemy(Guid guid, float damage)
        {
            Enemies[guid].Hit(damage);
        }

        private void ScatterCoins(Enemy enemy)
        {
            int coinCount = _random.Next(2, 4);
            for (int i = 0; i < coinCount; i++)
            {
                var offset = RandomInCircle(enemy.Radius);
                var coinPosition = enemy.TexturePosition + offset;
                AddCoin(new Coin(coinPosition));
            }
        }

        private Vector2 RandomInCircle(float radius)
        {
            var angle = _random.NextDouble() * MathF.PI * 2;
            var r = _random.NextDouble() * radius;
            return new Vector2(
                (float)(Math.Cos(angle) * r),
                (float)(Math.Sin(angle) * r)
            );
        }
    }
}
