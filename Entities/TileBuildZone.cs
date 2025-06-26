using Microsoft.Xna.Framework;
using Squence.Core.Services;

namespace Squence.Entities
{
    internal class TileBuildZone(Vector2 tilePosition, int tileSize): Tile(TileType.BuildZone, tilePosition, tileSize)
    {
        public override string TextureName { get => TextureStore.GetBuildZoneTextureName(BulletType, LevelBuilding); }
        public BulletType BulletType { get; private set; } = BulletType.None;
        public int LevelBuilding { get; private set; } = 0;

        public void BuildZone(BulletType bulletType, int levelBuilding)
        {
            BulletType = bulletType;
            LevelBuilding = levelBuilding;
        }

        public void DestroyZone()
        {
            BulletType = BulletType.None;
            LevelBuilding = 0;
        }
    }
}
