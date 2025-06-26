using Squence.Core.UI;
using Squence.Entities;

namespace Squence.Core.States
{
    internal class BuildingPanelState
    {
        public bool IsVisible { get; private set; } = false;
        public TileBuildZone TileBuildZone { get; private set; }
        public BulletType BulletType { get; private set; } = BulletType.None;
        public int LevelBuilding { get; private set; } = 0;
        public int LevelUpCost { get => GetLevelUpCost(LevelBuilding + 1); }

        public void ShowForTile(TileBuildZone tile)
        {
            IsVisible = true;
            TileBuildZone = tile;
            BulletType = tile.BulletType;
            LevelBuilding = tile.LevelBuilding;
        }

        public void Hide()
        {
            IsVisible = false;
        }

        public void HandleIconClick(BulletType bulletType)
        {
            if (BulletType != BulletType.None && LevelBuilding != 0) return;

            if (LevelBuilding == 0)
            {
                BulletType = bulletType;
            }
        }

        public void HandleButtonClick(PanelButtonType buttonType, GameState gameState)
        {
            if (buttonType == PanelButtonType.Build)
            {
                if (gameState.MoneyCount >= LevelUpCost && LevelBuilding <= 2)
                {
                    LevelBuilding++;
                    gameState.BuildZone(LevelUpCost);
                    TileBuildZone.BuildZone(BulletType, LevelBuilding);
                    

                }
            }
            else
            {
                BulletType = BulletType.None;
                LevelBuilding = 0;
                TileBuildZone.DestroyZone();
            }
        }
        
        private static int GetLevelUpCost(int levelBuilding)
        {
            return levelBuilding switch
            {
                1 => 25,
                2 => 50,
                3 => 75,
                _ => -1
            };
        }
    }
}
