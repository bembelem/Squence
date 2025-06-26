using Squence.Entities;
using System.Collections.Generic;

namespace Squence.Core.Services
{
    internal class DamageCalculator
    {
        private static readonly int _baseDamage = 10;
        private static readonly Dictionary<BulletType, Dictionary<EnemyType, float>> _damageMatrix = new()
        {
            [BulletType.None] = new()
            {
                [EnemyType.Fire] = 1.0f,
                [EnemyType.Water] = 1.0f,
                [EnemyType.Metal] = 1.0f
            },
            [BulletType.Fire] = new()
            {
                [EnemyType.Fire] = 0.2f,
                [EnemyType.Water] = 0.5f,
                [EnemyType.Metal] = 2.0f
            },
            [BulletType.Ice] = new()
            {
                [EnemyType.Fire] = 2.0f,
                [EnemyType.Water] = 0.5f,
                [EnemyType.Metal] = 1.0f
            },
            [BulletType.Lightning] = new()
            {
                [EnemyType.Fire] = 1.0f,
                [EnemyType.Water] = 2.0f,
                [EnemyType.Metal] = 0.5f
            },
        };
        private static readonly Dictionary<int, float> _levelBuildingDamageBonus = new()
        {
            { 0, 1.0f },
            { 1, 1.25f },
            { 2, 1.5f },
            { 3, 2.0f }
        };

        public static float GetDamage(BulletType bulletType, EnemyType enemyType, int levelBuilding)
        {
            var damageTypeMultiplier = _damageMatrix[bulletType][enemyType];
            var damageLevelMultiplier = _levelBuildingDamageBonus[levelBuilding];

            return _baseDamage * damageTypeMultiplier * damageLevelMultiplier;
        }
    }
}
