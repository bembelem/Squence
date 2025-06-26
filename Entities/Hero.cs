using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Squence.Core;
using Squence.Core.Interfaces;
using Squence.Core.Services;
using System;


namespace Squence.Entities
{
    internal class Hero(GraphicsDevice graphicsDevice) : IRenderable, ICollidable
    {
        public Guid Guid { get; } = Guid.NewGuid();
        public string TextureName { get => TextureStore.GetHeroTextureName(DirectionType); }
        public Vector2 TexturePosition { get => _texturePosition; }
        private Vector2 _texturePosition = new(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
        public int TextureWidth { get; } = 64;
        public int TextureHeight { get; } = 64;

        public Vector2 Center { get => new(TexturePosition.X + TextureWidth / 2, TexturePosition.Y + TextureHeight / 2); }
        public float Radius { get; } = 64 / 2;

        private readonly float _heroSpeed = 200f;
        public DirectionType DirectionType { get; private set; } = DirectionType.Down;
        public BulletType BulletType { get; private set; } = BulletType.None;
        public int LevelBuilding { get; private set; } = 0;

        public void Move(Vector2 direction, GameTime gameTime)
        {
            var updatedHeroSpeed = _heroSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            _texturePosition += direction * updatedHeroSpeed;
        }

        public void SetAttack(BulletType bulletType, int levelBuilding) {
            BulletType = bulletType;
            LevelBuilding = levelBuilding;
        }

        public void SetDirectionType(DirectionType directionType)
        {
            DirectionType = directionType;
        }
    }
}
