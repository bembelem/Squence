using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Squence.Core.Managers;
using Squence.Core.Services;
using Squence.Core.States;

namespace Squence.Core.UI
{
    internal class HUDPanel : Interfaces.IDrawable
    {
        private readonly GameState _gameState;
        private readonly GraphicsDevice _graphicsDevice;

        private readonly Vector2 _center;

        private readonly Vector2 _restartIconPosition;
        private readonly int _restartIconSize = 102;
        private readonly int _centerPaddingY = 20;

        public HUDPanel(GameState gameState, GraphicsDevice graphicsDevice)
        {
            _gameState = gameState;
            _graphicsDevice = graphicsDevice;

            var windowWidth = graphicsDevice.PresentationParameters.BackBufferWidth;
            var windowHeight = graphicsDevice.PresentationParameters.BackBufferHeight;
            _center = new Vector2(windowWidth / 2, windowHeight / 2);

            _restartIconPosition = new Vector2(_center.X - _restartIconSize / 2, _center.Y - _restartIconSize / 2 + _centerPaddingY * 2);
        }

        public void Draw(DrawingManager drawingManager)
        {
            DrawGameParams(drawingManager);

            if (_gameState.GameStateType == GameStateType.Stop)
            {
                DrawGameOver(drawingManager);
            }
        }

        private void DrawGameParams(DrawingManager drawingManager)
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
                new Vector2(200, 20),
                80,
                10,
                "Content/Icons/chest_icon.png",
                $"{_gameState.MoneyCount}",
                BitmapFontType.HUDPanel
                );
            // отображаем количество заработанных очков
            drawingManager.DrawIconWithValue(
                new Vector2(420, 20),
                80,
                10,
                "Content/Icons/score_icon.png",
                $"{_gameState.ScorePoints}",
                BitmapFontType.HUDPanel
                );
        }

        private void DrawGameOver(DrawingManager drawingManager)
        {
            var textMessage = "Score:";
            var textScore = $"{_gameState.ScorePoints}";
            var textMessageSize = drawingManager.GetBitmapFont(BitmapFontType.HUDPanel).MeasureString(textMessage);
            var textScoreSize = drawingManager.GetBitmapFont(BitmapFontType.HUDPanel).MeasureString(textScore);

            Vector2 textMessagePosition = new(_center.X - textMessageSize.Width / 2, _center.Y - (int)(_restartIconSize * 1.75) + _centerPaddingY);
            Vector2 textScorePosition = new(_center.X - textScoreSize.Width / 2, _center.Y - _restartIconSize + _centerPaddingY);

            drawingManager.DrawDarkOverlay(_graphicsDevice);

            drawingManager.DrawBitmapText(
                textMessage,
                textMessagePosition,
                new Color(240, 200, 90),
                BitmapFontType.HUDPanel
                );
            drawingManager.DrawBitmapText(
               $"{_gameState.ScorePoints}",
               textScorePosition,
               Color.White,
               BitmapFontType.HUDPanel
               );
            drawingManager.DrawIcon(
                _restartIconPosition,
                _restartIconSize,
                _restartIconSize,
                "Content/Icons/restart_icon.png"
                );
        }

        public void TryHandleRestartClick(MouseState mouseState)
        {
            if (IsClicked(mouseState, _restartIconPosition, _restartIconSize, _restartIconSize))
            {
                _gameState.RestartGame();
            }
        }

        private static bool IsClicked(MouseState mouseState, Vector2 componentPosition, int componentWidth, int componentHeight)
        {
            Vector2 mousePosition = new(mouseState.X, mouseState.Y);
            Rectangle componentBorders = new(
                        (int)componentPosition.X,
                        (int)componentPosition.Y,
                        componentWidth,
                        componentHeight
                        );

            return componentBorders.Contains(mousePosition);
        }
    }
}
