using Microsoft.Xna.Framework;
using Squence.Entities;
using System.Collections.Generic;

namespace Squence.Data
{
    // описание уровня
    internal static class LevelMap
    {
        public static TileMapDefinition GetTileMapDefinition()
        {
            var tileSize = 96;
            var width = 14;
            var height = 11;

            List<Point> roadTiles = 
            [
                new Point(10, 0), new Point(10, 1), new Point(10, 2), new Point(9, 2),
                new Point(8, 2), new Point(7, 2), new Point(6, 2), new Point(5, 2),
                new Point(4, 2), new Point(3, 2), new Point(2, 2), new Point(2, 3),
                new Point(2, 4), new Point(3, 4), new Point(4, 4), new Point(4, 5),
                new Point(4, 6), new Point(3, 6), new Point(2, 6), new Point(1, 6),
                new Point(0, 6), new Point(8, 3), new Point(8, 4), new Point(8, 5),
                new Point(8, 6), new Point(7, 6), new Point(6, 6), new Point(5, 6),
                new Point(13, 5), new Point(12, 5), new Point(11, 5), new Point(10, 5),
                new Point(10, 6), new Point(10, 7), new Point(11, 7), new Point(12, 7),
                new Point(12, 8), new Point(12, 9), new Point(11, 9), new Point(10, 9),
                new Point(9, 9), new Point(8, 9), new Point(7, 9), new Point(7, 8),
                new Point(6, 8), new Point(5, 8), new Point(4, 8), new Point(4, 7)
            ];
            List<Point> buildZoneTiles = [
                new Point(3, 3), new Point(7, 3), new Point(7, 5),
                new Point(11, 6), new Point(5, 7), new Point(11, 8)
            ];
            
            List<List<Point>> enemyPathesList = [
                [ 
                    new Point(10, -1), new Point(10, 2), new Point(2, 2),
                    new Point(2, 4), new Point(4, 4), new Point(4, 6),
                    new Point(-1, 6)
                ],
                [
                    new Point(10, -1), new Point(10, 2), new Point(8, 2),
                    new Point(8, 6), new Point(-1, 6),
                ],
                [
                    new Point(14, 5), new Point(10, 5), new Point(10, 7),
                    new Point(12, 7), new Point(12, 9), new Point(7, 9),
                    new Point(7, 8), new Point(4, 8), new Point(4, 6),
                    new Point(-1, 6)
                ],
                ];

            List<Wave> wavesList = [
                new Wave(
                    [
                        new WavePhase(5, EnemyType.Metal, enemyPathesList[0], 1f),
                        new WavePhase(4, EnemyType.Metal, enemyPathesList[1], 1f)
                    ],
                    2f
                ),
                new Wave(
                    [
                        new WavePhase(6, EnemyType.Water, enemyPathesList[2], 1f),
                        new WavePhase(5, EnemyType.Fire, enemyPathesList[0], 2f),
                        new WavePhase(5, EnemyType.Metal, enemyPathesList[1], 1f)
                    ],
                    3f
                ),
                new Wave(
                    [
                        new WavePhase(8, EnemyType.Water, enemyPathesList[2], 2f),
                        new WavePhase(6, EnemyType.Metal, enemyPathesList[1], 1f),
                        new WavePhase(4, EnemyType.Fire, enemyPathesList[0], 2f)
                    ],
                    2f
                ),
                new Wave(
                    [
                        new WavePhase(5, EnemyType.Fire, enemyPathesList[0], 2f),
                        new WavePhase(5, EnemyType.Metal, enemyPathesList[1], 2f),
                        new WavePhase(6, EnemyType.Water, enemyPathesList[2], 2f)
                    ],
                    3f
                ),
                new Wave(
                    [
                        new WavePhase(7, EnemyType.Metal, enemyPathesList[1], 1f),
                        new WavePhase(8, EnemyType.Water, enemyPathesList[2], 1f)
                    ],
                    2f
                )
                ];

            return new TileMapDefinition(
                tileSize,
                width,
                height,
                roadTiles,
                buildZoneTiles,
                enemyPathesList,
                wavesList
                );
            
        }
    }
}
