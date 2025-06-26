namespace Squence.Core.States
{
    public enum GameStateType
    {
        Play,
        Stop,
        Restart
    }

    internal class GameState
    {
        public int HealthPoints { get; private set; } = 3;
        public int MoneyCount { get; private set; } = 100;
        public int ScorePoints { get; private set; } = 0;
        public GameStateType GameStateType { get; private set; } = GameStateType.Play;

        public void HandleEnemyBreakthrough()
        {
            HealthPoints--;
        }

        public void HandleCoinCollection()
        {
            MoneyCount++;
            ScorePoints++;
        }

        public void HandleBuildZone(int moneyCost)
        {
            MoneyCount -= moneyCost;
        }

        public void HandleKillEnemy()
        {
            ScorePoints += 10;
        }

        public void StopPlay()
        {
            GameStateType = GameStateType.Stop;
        }

        public void RestartGame()
        {
            GameStateType = GameStateType.Restart;
        }
    }
}
