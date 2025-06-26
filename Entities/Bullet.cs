using Microsoft.Xna.Framework;
using Squence.Core;
using Squence.Core.Interfaces;
using Squence.Core.Services;
using System;

namespace Squence.Entities
{
    public enum BulletType
    {
        None,
        Fire,
        Ice,
        Lightning
    }

    internal class Bullet(
        Vector2 bulletPosition,
        Vector2 direction, 
        BulletType bulletType, 
        int levelBuilding
        ) : IRenderable, IUpdatable, ICollidable
    {
        public Guid Guid { get; } = Guid.NewGuid();
        public string TextureName { get => TextureStore.GetBulletTextureName(BulletType); }
        public Vector2 TexturePosition { get => _texturePosition; }
        private Vector2 _texturePosition = bulletPosition;
        public int TextureWidth { get; } = 64;
        public int TextureHeight { get; } = 64;
        public float Rotation { get => (float)Math.Atan2(direction.Y, direction.X) + MathF.PI / -2f; }

        public Vector2 Center { get => new(_texturePosition.X + TextureWidth / 2, _texturePosition.Y + TextureHeight / 2); }
        public float Radius { get; } = 32;

        private readonly float _bulletSpeed = 500f;
        public readonly BulletType BulletType = bulletType;
        public readonly int LevelBuilding = levelBuilding;

        public void Update(GameTime gameTime)
        {
            Move(gameTime);
        }

        private void Move(GameTime gameTime)
        {
            var updatedHeroSpeed = _bulletSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _texturePosition += direction * updatedHeroSpeed;
        }

        public static Vector2 GetStartPosition(Hero hero)
        {
            if (hero.DirectionType == DirectionType.Down || hero.DirectionType == DirectionType.Right)
            {
                return new Vector2(hero.TexturePosition.X + hero.TextureWidth, hero.TexturePosition.Y);
            }

            return hero.TexturePosition;
        }
    }
}
