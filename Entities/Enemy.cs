using Microsoft.Xna.Framework;
using Squence.Core;
using Squence.Core.Interfaces;
using Squence.Core.Services;
using System;
using System.Collections.Generic;

namespace Squence.Entities
{

    public enum EnemyType
    {
        Fire,
        Water,
        Metal
    }

    internal class Enemy(List<Point> enemyPath, int tileSize, EnemyType enemyType): IRenderable, IUpdatable, ICollidable
    {
        public Guid Guid { get; } = Guid.NewGuid();
        public string TextureName { get => TextureStore.GetEnemyTextureName(EnemyType, _directionType); }
        public Vector2 TexturePosition { get => _texturePosition; }
        private Vector2 _texturePosition = new Vector2(enemyPath[0].X, enemyPath[0].Y) * tileSize;
        public int TextureWidth { get; } = tileSize;
        public int TextureHeight { get; } = tileSize;

        public Vector2 Center { get => new(_texturePosition.X + 64 / 2, _texturePosition.Y + 64 / 2); }
        public float Radius { get; } = tileSize / 2;

        
        private int _currentTargetIndex = 1;
        private readonly int _tileSize = tileSize;
        public bool IsReachGoal { get; private set; } = false;
        public float HealthPoints { get; private set; } = 30f;
        private readonly float _enemySpeed = 100f;
        private DirectionType _directionType = DirectionType.Left;
        public readonly EnemyType EnemyType = enemyType;

        public void Update(GameTime gameTime)
        {
            Move(gameTime);
        }

        private void Move(GameTime gameTime)
        {
            // если враг пришёл к цели, то завершаем движение
            if (_currentTargetIndex >= enemyPath.Count)
            {
                IsReachGoal = true;
                return;
            }

            Vector2 target = new Vector2(enemyPath[_currentTargetIndex].X, enemyPath[_currentTargetIndex].Y) * _tileSize;
            Vector2 direction = target - _texturePosition;
            float distanceToTarget = direction.Length();

            // если дистанция до цели минимальна, то переключаемся на следующую
            if (distanceToTarget < 0.1f)
            {
                _currentTargetIndex++;
                if (_currentTargetIndex >= enemyPath.Count)
                    return;
                target = new Vector2(enemyPath[_currentTargetIndex].X, enemyPath[_currentTargetIndex].Y) * _tileSize;
                direction = target - _texturePosition;
                distanceToTarget = direction.Length();
            }

            _directionType = GetDirectionType(direction);

            direction.Normalize();
            float distanceToMove = _enemySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // чтобы не перескочить точку
            if (distanceToMove > distanceToTarget)
                distanceToMove = distanceToTarget;

            _texturePosition += direction * distanceToMove;
        }

        private DirectionType GetDirectionType(Vector2 direction)
        {
            var directionType = _directionType;
           
            if (direction.Y <= 0)
            {
                directionType = DirectionType.Up;
            }
            if (Math.Abs(direction.X) > Math.Abs(direction.Y))
            {
                directionType = direction.X > 0 ? DirectionType.Right : DirectionType.Left;
            }
            return directionType;
        }

        public void Hit(float damage)
        {
            HealthPoints -= damage;
        }
    }
}
