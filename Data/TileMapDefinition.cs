using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Squence.Data
{
    // данные окружения уровня
    public readonly record struct TileMapDefinition(
        int TileSize,
        int Width,
        int Height,
        List<Point> RoadTiles,
        List<Point> BuildZoneTiles,
        List<List<Point>> EnemyPathesList,
        List<Wave> WavesList
    );
}
