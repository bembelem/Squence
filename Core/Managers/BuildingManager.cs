using Microsoft.Xna.Framework.Input;
using Squence.Core.UI;
using Squence.Entities;

namespace Squence.Core.Managers
{
    internal class BuildingManager(TileMapManager tileMapManager, UIManager uiManager)
    {
        private readonly TileMapManager _tileMapManager = tileMapManager;
        private readonly UIManager _uiManager = uiManager;

        public void TryHandleTileClick(MouseState mouseState)
        {
            var tileX = mouseState.X / _tileMapManager.TileMapDefinition.TileSize;
            var tileY = mouseState.Y / _tileMapManager.TileMapDefinition.TileSize;

            var tile = _tileMapManager.GetTile(tileX, tileY);
            if (tile is TileBuildZone)
            {
                _uiManager.ShowBuildingPanelForTile(tile as TileBuildZone);
            } else
            {
                _uiManager.HideBuildingPanel();
            }
        }

        public bool TryHandleUIClick(MouseState mouseState)
        {
            return _uiManager.TryHandleClick(mouseState);
        } 
    }
}
