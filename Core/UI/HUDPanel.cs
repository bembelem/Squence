using Microsoft.Xna.Framework;
using Squence.Core.Managers;
using Squence.Core.Services;
using Squence.Core.States;

namespace Squence.Core.UI
{
    internal class HUDPanel(GameState gameState) : Interfaces.IDrawable
    {
        private readonly GameState _gameState = gameState;
        public void Draw(DrawingManager drawingManager)
        {
            // отображаем количество жизней до проигрыша
            drawingManager.DrawIconWithValue(
                new Vector2(20, 20),
                80,
                10,
                "Content/Icons/heart_icon.png",
                $"{_gameState.HealthPoints}",
                BitmapFontType.HUDPanel
                );
            // отображаем количество собранных монет
            drawingManager.DrawIconWithValue(
                new Vector2(200, 22),
                80,
                10,
                "Content/Icons/chest_icon.png",
                $"{_gameState.MoneyCount}",
                BitmapFontType.HUDPanel
                );
        }
    }
}
