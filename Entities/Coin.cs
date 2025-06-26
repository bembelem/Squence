using Microsoft.Xna.Framework;
using Squence.Core.Interfaces;
using System;

namespace Squence.Entities
{
    internal class Coin(Vector2 texturePosition) : IRenderable, IUpdatable, ICollidable
    {
        public Guid Guid { get; } = Guid.NewGuid();
        public string TextureName { get; } = "Content/coin.png";
        public Vector2 TexturePosition { get; } = texturePosition;
        public int TextureWidth { get; } = 48;
        public int TextureHeight { get; } = 48;

        public Vector2 Center { get => new (TexturePosition.X + TextureWidth / 2, TexturePosition.Y + TextureHeight / 2); }
        public float Radius { get; } = 12;

        public bool IsDisappeared { get; private set; } = false;
        private readonly float _disappearenceTimeDelay = 10f;
        private float _disappearenceTimer = 0f;

        public void Update(GameTime gameTime)
        {
            if (_disappearenceTimer >= _disappearenceTimeDelay)
            {
                IsDisappeared = true;
                return;
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _disappearenceTimer += deltaTime;
        }
    }
}
