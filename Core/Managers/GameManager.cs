using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Squence.Core.Interfaces;
using Squence.Core.Services;
using Squence.Core.States;
using Squence.Core.UI;
using Squence.Data;

namespace Squence.Core.Managers
{
    internal class GameManager
    {
        private GameState _gameState;

        private UIManager _uiManager;
        private EntityManager _entityManager;
        private TileMapManager _tileMapManager;

        private WaveManager _waveManager;
        private CollisionManager _collisionManager;
        private DrawingManager _drawingManager;
        private InputManager _inputManager;

        public GameManager(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            InitGameManager(graphicsDevice, spriteBatch);
        }

        private void InitGameManager(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            _gameState = new GameState();

            var _tileMapDefinition = LevelMap.GetTileMapDefinition();

            _uiManager = new UIManager(_gameState, graphicsDevice);
            _entityManager = new EntityManager(_gameState, graphicsDevice);
            _tileMapManager = new TileMapManager(_tileMapDefinition);

            _waveManager = new WaveManager(_entityManager, _tileMapDefinition.WavesList, _tileMapDefinition.TileSize);
            _collisionManager = new CollisionManager(_entityManager, _tileMapManager, _gameState);
            _drawingManager = new DrawingManager(spriteBatch, new TextureStore(graphicsDevice));
            _inputManager = new InputManager(_entityManager, new BuildingManager(_tileMapManager, _uiManager));
        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            if (_gameState.GameStateType == GameStateType.Stop)
            {
                _inputManager.UpdateGameOver(_uiManager);
                return;
            }

            if (_gameState.GameStateType == GameStateType.Restart)
            {
                InitGameManager(graphicsDevice, spriteBatch);
                return;
            }

            _inputManager.Update(gameTime);
            _entityManager.Update(gameTime);
            _waveManager.Update(gameTime);
            _collisionManager.Update();

            if (_gameState.HealthPoints <= 0)
            {
                _gameState.StopPlay();
            }
        }

        public void Draw()
        {
            _tileMapManager.Draw(_drawingManager);
            _entityManager.Draw(_drawingManager);
            _uiManager.Draw(_drawingManager);
        }
    }
}
