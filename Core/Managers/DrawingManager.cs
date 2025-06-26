using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using Squence.Core.Interfaces;
using Squence.Core.Services;
using Squence.Entities;
using System.Collections.Generic;

namespace Squence.Core.Managers
{
    internal class DrawingManager(SpriteBatch spriteBatch, TextureStore textureStore)
    {
        private readonly SpriteBatch _spriteBatch = spriteBatch;
        private readonly TextureStore _textureStore = textureStore;

        public void DrawRenderableEntities(IEnumerable<IRenderable> renderableEntities)
        {
            foreach (var renderableEntity in renderableEntities)
            {
                DrawRenderableEntity(renderableEntity);
            }
        }
        
        public void DrawRenderableEntity(IRenderable entity)
        {
            var texture = _textureStore.Get(entity.TextureName);
            var textureOrigin = entity is Bullet ? 
                new Vector2(texture.Width / 2f, texture.Height / 2f) :
                Vector2.Zero;
            var textureScale = entity.TextureWidth / (float)_textureStore.Get(entity.TextureName).Width;

            _spriteBatch.Draw(
                texture: texture,
                position: entity.TexturePosition,
                sourceRectangle: null,
                color: Color.White,
                rotation: entity.Rotation,
                origin: textureOrigin,
                scale: textureScale,
                effects: SpriteEffects.None,
                layerDepth: 0f
            );
        }
        
        public void DrawIconWithValue(Vector2 iconPosition, int iconSize, int paddingX, string iconName, string value, BitmapFontType bitmapFontType)
        {
            var font = GetBitmapFont(bitmapFontType);
            var textSize = font.MeasureString(value);
            var textHeight = textSize.Height;

            var verticalOffset = (iconSize - textHeight) / 2;
            Vector2 valuePosition = new(iconPosition.X + iconSize + paddingX, iconPosition.Y + verticalOffset);

            DrawIcon(iconPosition, iconSize, iconSize, iconName);
            DrawBitmapText(value, valuePosition, Color.White, bitmapFontType);
        }

        public void DrawIcon(Vector2 iconPosition, int iconWidth, int iconHeight, string iconName)
        {
            _spriteBatch.Draw(
                texture: _textureStore.Get(iconName),
                destinationRectangle: new Rectangle(
                    (int)iconPosition.X,
                    (int)iconPosition.Y,
                    iconWidth,
                    iconHeight
                    ),
                Color.White
                );
        }

        public void DrawBitmapText(string text, Vector2 position, Color color, BitmapFontType bitmapFontType)
        {
            var bitmapFont = GetBitmapFont(bitmapFontType);
            foreach (var glyph in bitmapFont.GetGlyphs(text, position))
            {
                if (glyph.Character == null)
                {
                    continue;
                }
                var region = glyph.Character.TextureRegion;
                _spriteBatch.Draw(region.Texture, glyph.Position, region.Bounds, color);
            }
        }

        private BitmapFont GetBitmapFont(BitmapFontType bitmapFontType)
        {
            return bitmapFontType switch
            {
                BitmapFontType.HUDPanel => _textureStore.HUDPanelBitmapFont,
                BitmapFontType.BuildingPanel => _textureStore.BuildingPanelBitmapFont,
                _ => _textureStore.HUDPanelBitmapFont
            };
        }
    }
}
