using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Squence.Core.Managers;
using Squence.Core.Services;
using Squence.Core.States;
using Squence.Entities;
using System.Collections.Generic;

namespace Squence.Core.UI
{
    enum PanelButtonType
    {
        Build,
        Cancel
    }

    internal class BuildingPanel : Interfaces.IDrawable
    {
        private readonly record struct BuildingIcon(
            BulletType BulletType,
            Vector2 IconPosition
            );
        private readonly record struct PanelButton(
            PanelButtonType ButtonType, 
            Vector2 ButtonPosition, 
            int ButtonWidth, 
            int ButtonHeight
            );

        private readonly GameState _gameState;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly BuildingPanelState _buildingPanelState = new();

        private readonly List<BuildingIcon> _buildingIconsList;
        private readonly List<PanelButton> _panelButtonsList;
        private readonly int _iconSize = 80;

        public BuildingPanel(GameState gameState, GraphicsDevice graphicsDevice)
        {
            _gameState = gameState;
            _graphicsDevice = graphicsDevice;

            var windowWidth = _graphicsDevice.PresentationParameters.BackBufferWidth;
            var windowHeight = _graphicsDevice.PresentationParameters.BackBufferHeight;
            _buildingIconsList =
            [
                new(BulletType.Fire, new Vector2(windowWidth - 300, windowHeight - 150)),
                new(BulletType.Lightning, new Vector2(windowWidth - 200, windowHeight - 150)),
                new(BulletType.Ice, new Vector2(windowWidth - 100, windowHeight - 150))
            ];
            _panelButtonsList =
            [
                new(PanelButtonType.Build, new Vector2(windowWidth - 300, windowHeight - 80), 140, 90),
                new(PanelButtonType.Cancel, new Vector2(windowWidth - 150, windowHeight - 80), 140, 90)
            ];
        }

        public void Draw(DrawingManager drawingManager)
        {
            if (!_buildingPanelState.IsVisible) return;

            DrawIcons(drawingManager);
            DrawButtons(drawingManager);
        }

        private void DrawIcons(DrawingManager drawingManager)
        {
            foreach (var icon in _buildingIconsList)
            {
                drawingManager.DrawIcon(
                    icon.IconPosition, 
                    _iconSize, 
                    _iconSize, 
                    GetIconTextureName(icon.BulletType, _buildingPanelState)
                    );
            }
        }

        private void DrawButtons(DrawingManager drawingManager)
        {
            foreach (var button in _panelButtonsList)
            {
                drawingManager.DrawIcon(
                    button.ButtonPosition,
                    button.ButtonWidth,
                    button.ButtonHeight,
                    "Content/button.png"
                    );

                if (button.ButtonType == PanelButtonType.Build)
                {
                    drawingManager.DrawIconWithValue(
                        new Vector2(button.ButtonPosition.X + 22, button.ButtonPosition.Y + 22),
                        48,
                        0,
                        "Content/coin.png",
                        $"{_buildingPanelState.LevelUpCost}",
                        BitmapFontType.BuildingPanel
                        );
                } 
                else
                {
                    drawingManager.DrawIcon(
                        new Vector2(button.ButtonPosition.X + 46, button.ButtonPosition.Y + 22),
                        48, 
                        48, 
                        "Content/Icons/cancel_icon.png"
                        );
                }
            }
        }

        public void ShowForTile(TileBuildZone tile)
        {
            _buildingPanelState.ShowForTile(tile);
        }

        public void Hide()
        {
            _buildingPanelState.Hide();
        }

        public bool TryHandleClick(MouseState mouseState)
        {
            if (!_buildingPanelState.IsVisible)
            { 
                return false;
            }

            return TryHandleIconsClick(mouseState) || TryHandleButtonsClick(mouseState);
        }

        private bool TryHandleIconsClick(MouseState mouseState)
        {
            foreach (var icon in _buildingIconsList)
            {
                if (IsClicked(mouseState, icon.IconPosition, _iconSize, _iconSize))
                {
                    _buildingPanelState.HandleIconClick(icon.BulletType);
                    return true;
                }
            }

            return false;
        }

        private bool TryHandleButtonsClick(MouseState mouseState)
        {
            foreach (var button in _panelButtonsList)
            {
                if (IsClicked(mouseState, button.ButtonPosition, button.ButtonWidth, button.ButtonHeight))
                {
                    _buildingPanelState.HandleButtonClick(button.ButtonType, _gameState);
                    return true;
                }
            }

            return false;
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

        private static string GetIconTextureName(BulletType iconBulletType, BuildingPanelState buildingPanelState)
        {
            // если зона ещё не построена
            if (buildingPanelState.LevelBuilding == 0)
            {
                if (iconBulletType == buildingPanelState.BulletType)
                {
                    return TextureStore.GetIconChosenTextureName(iconBulletType);
                }
                else
                {
                    return TextureStore.GetIconChoiceableTextureName(iconBulletType);
                }
            }
            // блокируем выбор других
            else
            {
                if (iconBulletType == buildingPanelState.BulletType)
                {
                    return TextureStore.GetIconChosenTextureName(iconBulletType);
                }
                else
                {
                    return TextureStore.GetIconLockedTextureName(iconBulletType);
                }
            }
        }
    }
}
