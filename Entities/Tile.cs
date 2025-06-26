using Microsoft.Xna.Framework;
using Squence.Core.Interfaces;
using Squence.Core.Services;
using System;

namespace Squence.Entities
{
    public enum TileType
    {
        Grass,
        Road,
        BuildZone,
    }

    internal class Tile(TileType tileType, Vector2 tilePosition, int tileSize): IRenderable
    {
        private readonly TileType _tileType = tileType;
        public Guid Guid { get; } = Guid.NewGuid();
        public virtual string TextureName { get => TextureStore.GetTileTextureName(_tileType); }
        public Vector2 TexturePosition { get; private set; } = tilePosition;
        public int TextureWidth { get; } = tileSize;
        public int TextureHeight { get; } = tileSize;
    }
}
