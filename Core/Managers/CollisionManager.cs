using Microsoft.Xna.Framework;
using Squence.Core.Interfaces;
using Squence.Core.Services;
using Squence.Core.States;
using Squence.Entities;

namespace Squence.Core.Managers
{
    internal class CollisionManager(
        EntityManager entityManager, 
        TileMapManager tileMapManager, 
        GameState gameState
        ) // Updatable (gameTime не требуется)
    {
        private readonly EntityManager _entityManager = entityManager;
        private readonly TileMapManager _tileMapManager = tileMapManager;
        private readonly GameState _gameState = gameState;

        public void Update()
        {
            HandleBulletEnemyCollisions();
            HandleHeroCoinCollisions();
            HandleHeroBuildZoneCollisions();
        }

        private void HandleBulletEnemyCollisions()
        {
            foreach (var bullet in _entityManager.Bullets.Values)
            {
                foreach (var enemy in _entityManager.Enemies.Values)
                {
                    if (IsRadiusColliding(bullet, enemy))
                    {
                        var damage = DamageCalculator.GetDamage(bullet.BulletType, enemy.EnemyType, bullet.LevelBuilding);
                        _entityManager.HitEnemy(enemy.Guid, damage);
                        _entityManager.RemoveBullet(bullet.Guid);
                    }
                }
            }
        }

        private void HandleHeroCoinCollisions()
        {
            foreach (var coin in _entityManager.Coins.Values)
            {
                if (IsRadiusColliding(_entityManager.Hero, coin))
                {
                    _entityManager.RemoveCoin(coin.Guid);
                    _gameState.HandleCoinCollection();
                }
            }
        }

        private void HandleHeroBuildZoneCollisions()
        {
            foreach (var buildZone in _tileMapManager.GetBuildZones())
            {
                if (IsPointColliding(_entityManager.Hero, buildZone)) {
                    _entityManager.Hero.SetAttack(buildZone.BulletType, buildZone.LevelBuilding);
                    return;
                }
            }

            _entityManager.Hero.SetAttack(BulletType.None, 0);
        }

        private static bool IsRadiusColliding(ICollidable aEntity, ICollidable bEntity)
        {
            var posA = aEntity.Center;
            var posB = bEntity.Center;
            var radiusA = aEntity.Radius;
            var radiusB = bEntity.Radius;

            return Vector2.Distance(posA, posB) < radiusA + radiusB;
        }

        private static bool IsPointColliding(ICollidable pointEntity, IRenderable zoneEntity)
        {
            var pointEntityCenter = pointEntity.Center;
            var zoneEntityRectangle = new Rectangle(
                (int)zoneEntity.TexturePosition.X,
                (int)zoneEntity.TexturePosition.Y,
                zoneEntity.TextureWidth,
                zoneEntity.TextureHeight
                );

            return zoneEntityRectangle.Contains(pointEntityCenter);
        }
    }
}
