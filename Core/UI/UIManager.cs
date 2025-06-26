using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Squence.Core.Interfaces;
using Squence.Core.Managers;
using Squence.Core.States;
using Squence.Entities;

namespace Squence.Core.UI
{
    internal class UIManager(GameState gameState, GraphicsDevice graphicsDevice): IDrawable
    {
        private readonly HUDPanel _hudPanel = new(gameState);
        private readonly BuildingPanel _buildingPanel = new(gameState, graphicsDevice);

        public void Draw(DrawingManager drawingManager)
        {
            _hudPanel.Draw(drawingManager);
            _buildingPanel.Draw(drawingManager);
        }
        
        public void ShowBuildingPanelForTile(TileBuildZone tile)
        {
            _buildingPanel.ShowForTile(tile);
        }

        public void HideBuildingPanel()
        {
            _buildingPanel.Hide();
        }

        public bool TryHandleClick(MouseState mouseState)
        {
            return _buildingPanel.TryHandleClick(mouseState);
        }
    }
}
