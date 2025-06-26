using Microsoft.Xna.Framework;
using Squence.Data;
using Squence.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Squence.Core.Managers
{
    // обработка данных уровня
    internal class TileMapManager: Interfaces.IDrawable
    {
        public TileMapDefinition TileMapDefinition { get; private set; }
        private readonly Tile[,] _tiles;

        public TileMapManager(TileMapDefinition tileMapDefinition)
        {
            TileMapDefinition = tileMapDefinition;
            _tiles = new Tile[tileMapDefinition.Width, tileMapDefinition.Height];

            InitTileMap(tileMapDefinition);
        }
        
        public void Draw(DrawingManager drawingManger)
        {
            for (var i = 0; i < _tiles.GetLength(0); i++)
            {
                for (var j = 0; j < _tiles.GetLength(1); j++)
                {
                    drawingManger.DrawRenderableEntity(_tiles[i, j]);
                }
            }
        }

        public void InitTileMap(TileMapDefinition tileMapDefinition)
        {
            FillSpecialTiles(tileMapDefinition.RoadTiles, TileType.Road, tileMapDefinition); // заполняем дороги
            FillSpecialTiles(tileMapDefinition.BuildZoneTiles, TileType.BuildZone, tileMapDefinition); // заполняем зоны строительства

            // заполняем остальные пустые клетки травой
            for (var x = 0; x < tileMapDefinition.Width; x++)
            {
                for (var y = 0; y < tileMapDefinition.Height; y++)
                {
                    if (_tiles[x, y] == null)
                    {
                        var tilePosition = new Vector2(x * tileMapDefinition.TileSize, y * tileMapDefinition.TileSize);
                        _tiles[x, y] = new Tile(TileType.Grass, tilePosition, tileMapDefinition.TileSize);
                    }
                }
            }
        }

        private void FillSpecialTiles(List<Point> tilesList, TileType tileType, TileMapDefinition tileMapDefinition)
        {
            foreach (var tile in tilesList)
            {
                var tilePosition = new Vector2(tile.X * tileMapDefinition.TileSize, tile.Y * tileMapDefinition.TileSize);
                if (tileType == TileType.BuildZone)
                {
                    _tiles[tile.X, tile.Y] = new TileBuildZone(tilePosition, tileMapDefinition.TileSize);
                }
                else
                {
                    _tiles[tile.X, tile.Y] = new Tile(tileType, tilePosition, tileMapDefinition.TileSize);
                }    
            }
        }

        public Tile GetTile(int x, int y)
        {
            return _tiles[x, y];
        }

        public List<TileBuildZone> GetBuildZones()
        {
            List <TileBuildZone> buildZonesList = [];
            foreach (var buildZone in TileMapDefinition.BuildZoneTiles)
            {
                TileBuildZone tile = _tiles[buildZone.X, buildZone.Y] as TileBuildZone;
                buildZonesList.Add(tile);
            }

            return buildZonesList;
        }
    }
}
