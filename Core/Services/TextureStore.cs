using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Content.BitmapFonts;
using MonoGame.Extended.Graphics;
using Squence.Entities;
using System.Collections.Generic;
using System.IO;

namespace Squence.Core.Services
{
    enum BitmapFontType
    {
        HUDPanel,
        BuildingPanel
    }

    internal class TextureStore
    {
        public BitmapFont HUDPanelBitmapFont { get; private set; }
        public BitmapFont BuildingPanelBitmapFont { get; private set; }
        private readonly GraphicsDevice _graphicsDevice;
        private readonly Dictionary<string, Texture2D> _textures = [];

        private static readonly Dictionary<BulletType, Dictionary<int, string>> _buildZoneTextures = new()
        {
            [BulletType.Fire] = new Dictionary<int, string>
            {
                [1] = "Content/Tiles/Building/fire_lvl_1.png",
                [2] = "Content/Tiles/Building/fire_lvl_2.png",
                [3] = "Content/Tiles/Building/fire_lvl_3.png"
            },
            [BulletType.Ice] = new Dictionary<int, string>
            {
                [1] = "Content/Tiles/Building/ice_lvl_1.png",
                [2] = "Content/Tiles/Building/ice_lvl_2.png",
                [3] = "Content/Tiles/Building/ice_lvl_3.png"
            },
            [BulletType.Lightning] = new Dictionary<int, string>
            {
                [1] = "Content/Tiles/Building/lightning_lvl_1.png",
                [2] = "Content/Tiles/Building/lightning_lvl_2.png",
                [3] = "Content/Tiles/Building/lightning_lvl_3.png"
            }
        };

        private static readonly Dictionary<EnemyType, Dictionary<DirectionType, string>> _enemiesTextures = new()
        {
            [EnemyType.Fire] = new Dictionary<DirectionType, string>
            {
                [DirectionType.Up] = "Content/Enemies/fire up.png",
                [DirectionType.Left] = "Content/Enemies/fire down left.png",
                [DirectionType.Right] = "Content/Enemies/fire down right.png",
            },
            [EnemyType.Water] = new Dictionary<DirectionType, string>
            {
                [DirectionType.Up] = "Content/Enemies/water up.png",
                [DirectionType.Left] = "Content/Enemies/water down left.png",
                [DirectionType.Right] = "Content/Enemies/water down right.png",
            },
            [EnemyType.Metal] = new Dictionary<DirectionType, string>
            {
                [DirectionType.Up] = "Content/Enemies/metal up.png",
                [DirectionType.Left] = "Content/Enemies/metal down left.png",
                [DirectionType.Right] = "Content/Enemies/metal down right.png",
            }
        };

        public TextureStore(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            HUDPanelBitmapFont = GetBitmap(graphicsDevice, "Content/Font/Nudge Orb_48.fnt", "Content/Font/Nudge Orb_48_0.png");
            BuildingPanelBitmapFont = GetBitmap(graphicsDevice, "Content/Font/Nudge Orb_24.fnt", "Content/Font/Nudge Orb_24_0.png");
        }

        private static BitmapFont GetBitmap(GraphicsDevice graphicsDevice, string fontFileName, string fontTextureName)
        {
            var fontFile = BitmapFontFileReader.Read(fontFileName);

            // Загрузка текстуры, которая используется в этом .fnt
            var fontTexture = Texture2D.FromFile(graphicsDevice, fontTextureName);

            // Преобразование CharacterBlock → BitmapFontCharacter
            var characters = new List<BitmapFontCharacter>();
            foreach (var c in fontFile.Characters)
            {
                var character = new BitmapFontCharacter(
                    (int)c.ID,                                       
                    new Texture2DRegion(fontTexture, c.X, c.Y, c.Width, c.Height),
                    c.XOffset,
                    c.YOffset,
                    c.XAdvance                                        
                );

                characters.Add(character);
            }

            // Создание BitmapFont вручную (используем Info.FontSize и Common.LineHeight)
            return new BitmapFont(
                fontFile.FontName ?? "Unknown",
                fontFile.Info.FontSize,
                fontFile.Common.LineHeight,
                characters
            );
        }

        // ленивая загрузка текстур
        public Texture2D Get(string textureName)
        {
            if (!_textures.TryGetValue(textureName, out Texture2D value))
            {
                // чтение изображения из файла
                using var stream = File.OpenRead(textureName);
                value = Texture2D.FromStream(_graphicsDevice, stream);
                _textures[textureName] = value;
            }

            return value;
        }

        public static string GetTileTextureName(TileType tileType)
        {
            return tileType switch
            {
                TileType.Grass => "Content/Tiles/tile_grass.png",
                TileType.Road => "Content/Tiles/tile_road.png",
                TileType.BuildZone => "Content/Tiles/tile_build_zone.png",
                _ => "Content/Tiles/tile_grass.png",
            };
        }

        public static string GetBuildZoneTextureName(BulletType bulletType, int levelBuilding)
        {
            if (bulletType == BulletType.None)
            {
                return "Content/Tiles/Building/tile_build_zone.png";
            }

            return _buildZoneTextures[bulletType][levelBuilding];
        }

        public static string GetHeroTextureName(DirectionType directionType)
        {
            return directionType switch
            {
                DirectionType.Up => "Content/Hero/MC up.png",
                DirectionType.Down => "Content/Hero/MC down.png",
                DirectionType.Left => "Content/Hero/MC left.png",
                DirectionType.Right => "Content/Hero/MC right.png",
                _ => "Content/Hero/MC down.png"
            };
        }

        public static string GetEnemyTextureName(EnemyType enemyType, DirectionType directionType)
        {
            return _enemiesTextures[enemyType][directionType];
        }

        public static string GetBulletTextureName(BulletType bulletType)
        {
            return bulletType switch
            {
                BulletType.None => "Content/Bullets/bullet_none.png",
                BulletType.Fire => "Content/Bullets/bullet_fire.png",
                BulletType.Ice => "Content/Bullets/bullet_ice.png",
                BulletType.Lightning => "Content/Bullets/bullet_lightning.png",
                _ => "Content/Bullets/bullet_fire.png"
            };
        }

        public static string GetIconChosenTextureName(BulletType iconBulletType)
        {
            return iconBulletType switch
            {
                BulletType.Fire => "Content/Icons/fire_icon.png",
                BulletType.Lightning => "Content/Icons/lightning_icon.png",
                BulletType.Ice => "Content/Icons/ice_icon.png",
                _ => "Content/Icons/fire_icon.png"
            };
        }

        public static string GetIconChoiceableTextureName(BulletType iconBulletType)
        {
            return iconBulletType switch
            {
                BulletType.Fire => "Content/Icons/fire_icon_choice.png",
                BulletType.Lightning => "Content/Icons/lightning_icon_choice.png",
                BulletType.Ice => "Content/Icons/ice_icon_choice.png",
                _ => "Content/Icons/fire_icon_choice.png"
            };
        }

        public static string GetIconLockedTextureName(BulletType iconBulletType)
        {
            return iconBulletType switch
            {
                BulletType.Fire => "Content/Icons/fire_icon_lock.png",
                BulletType.Lightning => "Content/Icons/lightning_icon_lock.png",
                BulletType.Ice => "Content/Icons/ice_icon_lock.png",
                _ => "Content/Icons/fire_icon_lock.png"
            };
        }
    }
}
