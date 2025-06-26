namespace Squence.Core.States
{

    internal class GameState
    {
        public int HealthPoints { get; private set; } = 3;
        public int MoneyCount { get; private set; } = 500;

        public void HandleEnemyBreakthrough()
        {
            HealthPoints--;
        }

        public void HandleCoinCollection()
        {
            MoneyCount++;
        }

        public void BuildZone(int moneyCost)
        {
            MoneyCount -= moneyCost;
        }
    }
}
