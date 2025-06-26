namespace Squence.Core.States
{
    internal class WaveState
    {
        public int CurrentWaveIndex { get; private set; } = 0;
        public int CurrentWavePhaseIndex { get; private set; } = 0;

        public float WaveDelayTimer { get; private set; } = 0f;
        public float WavePhaseDelayTimer { get; private set; } = 0f;
        public float EnemySpawnDelayTimer { get; private set; } = 0f;

        public bool IsWaitingForWave { get; private set; } = false;
        public bool IsWaitingForWavePhase { get; private set; } = false;

        public int SpawnedEnemies { get; private set; } = 0;

        public void SwitchToNextWave()
        {
            CurrentWaveIndex++;
            CurrentWavePhaseIndex = 0;
            WaveDelayTimer = 0f;
            WavePhaseDelayTimer = 0f;
            EnemySpawnDelayTimer = 0f;
            IsWaitingForWave = true;
            SpawnedEnemies = 0;
        }

        public void SwitchToNextWavePhase()
        {
            CurrentWavePhaseIndex++;
            WavePhaseDelayTimer = 0f;
            EnemySpawnDelayTimer = 0f;
            IsWaitingForWavePhase = true;
            SpawnedEnemies = 0;
        }

        public void StartWave()
        {
            IsWaitingForWave = false;
            IsWaitingForWavePhase = true;
        }

        public void StartPhase()
        {
            IsWaitingForWavePhase = false;
        }

        public void AddTimeToWaveDelayTimer(float deltaTime)
        {
            WaveDelayTimer += deltaTime;
        }

        public void AddTimeToWavePhaseDelayTimer(float deltaTime)
        {
            WavePhaseDelayTimer += deltaTime;
        }

        public void AddTimeToEnemySpawnDelayTimer(float deltaTime)
        {
            EnemySpawnDelayTimer += deltaTime;
        }

        public void AddEnemy()
        {
            EnemySpawnDelayTimer = 0f;
            SpawnedEnemies++;
        }
    }
}