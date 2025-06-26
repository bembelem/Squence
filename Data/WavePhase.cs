using Microsoft.Xna.Framework;
using Squence.Entities;
using System.Collections.Generic;

namespace Squence.Data
{
    public readonly record struct WavePhase(
        int EnemyCount,
        EnemyType EnemyType,
        List<Point> EnemyPath,
        float EnemySpawnDelay
    );
}
