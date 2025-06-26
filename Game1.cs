using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Squence.Core.Managers;
using Squence.Data;

namespace Squence
{
    public class Game1 : Game
    {

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameManager _gameManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            var tileMapDefinition = LevelMap.GetTileMapDefinition();
            _graphics.PreferredBackBufferWidth = tileMapDefinition.Width * tileMapDefinition.TileSize;
            _graphics.PreferredBackBufferHeight = tileMapDefinition.Height * tileMapDefinition.TileSize;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _gameManager = new GameManager(GraphicsDevice, _spriteBatch);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _gameManager.Update(gameTime, GraphicsDevice, _spriteBatch);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _gameManager.Draw();

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
