using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Squence.Core.Managers;
using Squence.Core.Services;
using Squence.Core.States;
using Squence.Core.UI;
using Squence.Data;

namespace Squence
{
    public class Game1 : Game
    {
        private readonly GameState _gameState = new();
        private readonly TileMapDefinition _tileMapDefinition = LevelMap.GetTileMapDefinition();

        private readonly GraphicsDeviceManager _graphics;

        private SpriteBatch _spriteBatch;
        private UIManager _uiManager;
        private EntityManager _entityManager;
        private TileMapManager _tileMapManager;

        private WaveManager _waveManager;
        private CollisionManager _collisionManager;
        private DrawingManager _drawingManager;
        private InputManager _inputManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = _tileMapDefinition.Width * _tileMapDefinition.TileSize;
            _graphics.PreferredBackBufferHeight = _tileMapDefinition.Height * _tileMapDefinition.TileSize;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _uiManager = new UIManager(_gameState, GraphicsDevice);
            _entityManager = new EntityManager(_gameState, GraphicsDevice);
            _tileMapManager = new TileMapManager(_tileMapDefinition);

            _waveManager = new WaveManager(_entityManager, _tileMapDefinition.WavesList, _tileMapDefinition.TileSize);
            _collisionManager = new CollisionManager(_entityManager, _tileMapManager, _gameState);
            _drawingManager = new DrawingManager(_spriteBatch, new TextureStore(GraphicsDevice));
            _inputManager = new InputManager(_entityManager, new BuildingManager(_tileMapManager, _uiManager));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape)
                || _gameState.HealthPoints <= 0)
                Exit();

            // TODO: Add your update logic here
            _inputManager.Update(gameTime);
            _entityManager.Update(gameTime);
            _waveManager.Update(gameTime);
            _collisionManager.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _tileMapManager.Draw(_drawingManager);
            _entityManager.Draw(_drawingManager);
            _uiManager.Draw(_drawingManager);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
