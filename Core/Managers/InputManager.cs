using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Squence.Core.Interfaces;
using Squence.Entities;

namespace Squence.Core.Managers
{
    // создаём новые сущности и обновляем параметры существующих
    internal class InputManager(EntityManager entityManager, BuildingManager buildingManager): IUpdatable
    {
        private readonly EntityManager _entityManager = entityManager;
        private readonly BuildingManager _buildingManager = buildingManager;

        private bool _isMouseLeftBulletPressed = false;
        private bool _isMouseLeftBuildingPressed = false;

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            UpdateHero(gameTime, keyboardState);
            UpdateBuilding(mouseState);
            UpdateBullets(gameTime, mouseState);
        }

        private void UpdateHero(GameTime gameTime, KeyboardState keyboardState)
        {
            MoveHero(gameTime, keyboardState);
        }

        private void MoveHero(GameTime gameTime, KeyboardState keyboardState)
        {
            var direction = Vector2.Zero;
            var directionType = DirectionType.Down;

            if (keyboardState.IsKeyDown(Keys.W)) 
            {
                direction.Y -= 1;
                directionType = DirectionType.Up;
            } 
            if (keyboardState.IsKeyDown(Keys.S))
            {
                direction.Y += 1;
                directionType = DirectionType.Down;
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                direction.X -= 1;
                directionType = DirectionType.Left;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                direction.X += 1;
                directionType = DirectionType.Right;
            }

            _entityManager.SetHeroDirectionType(directionType);
            _entityManager.MoveHero(direction, gameTime);
        }

        private void UpdateBullets(GameTime gameTime, MouseState mouseState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed && !_isMouseLeftBulletPressed)
            {
                var mousePosition = new Vector2(mouseState.X, mouseState.Y);
                var heroPosition = _entityManager.Hero.TexturePosition;
                var bulletType = _entityManager.Hero.BulletType;
                var levelBuilding = _entityManager.Hero.LevelBuilding;

                var bulletStartPosition = Bullet.GetStartPosition(_entityManager.Hero);
                var direction = mousePosition - heroPosition;
                direction.Normalize();
                _entityManager.AddBullet(new Bullet(bulletStartPosition, direction, bulletType, levelBuilding));

                _isMouseLeftBulletPressed = true;
            }

            if (mouseState.LeftButton == ButtonState.Released)
            {
                _isMouseLeftBulletPressed = false;
            }
        }

        private void UpdateBuilding(MouseState mouseState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed && !_isMouseLeftBuildingPressed)
            {   
                if (!_buildingManager.TryHandleUIClick(mouseState))
                {
                    _buildingManager.TryHandleTileClick(mouseState);
                }
                _isMouseLeftBuildingPressed = true;
            }

            if (mouseState.LeftButton == ButtonState.Released)
            {
                _isMouseLeftBuildingPressed = false;
            }
        }
    }
}
