using Microsoft.Xna.Framework;
using Squence.Core.Interfaces;
using Squence.Core.States;
using Squence.Data;
using Squence.Entities;
using System.Collections.Generic;

namespace Squence.Core.Managers
{
    internal class WaveManager(EntityManager entityManager, List<Wave> wavesList, int tileSize): IUpdatable
    {
        private readonly EntityManager _entityManager = entityManager;
        private readonly List<Wave> _wavesList = wavesList;
        private readonly int _tileSize = tileSize;

        private readonly WaveState _waveState = new();

        public void Update(GameTime gameTime)
        {
            // ничего не происходит, если все волны закончились
            if (_waveState.CurrentWaveIndex >= _wavesList.Count)
            {
                return;
            }

            // если все фазы волны завершены, то переключаемся к следующей волне
            var currentWave = _wavesList[_waveState.CurrentWaveIndex];
            if (_waveState.CurrentWavePhaseIndex >= currentWave.WavePhasesList.Count)
            {
                _waveState.SwitchToNextWave();
                return;
            }

            // если предыдущая фаза завершена, то переключаемся к следующей
            var currentWavePhase = currentWave.WavePhasesList[_waveState.CurrentWavePhaseIndex];
            if (_waveState.SpawnedEnemies >= currentWavePhase.EnemyCount)
            {
                _waveState.SwitchToNextWavePhase();
                return;
            }

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // начинаем новую волну, как только завершится период ожидания новой волны
            if (_waveState.IsWaitingForWave)
            {
                _waveState.AddTimeToWaveDelayTimer(deltaTime);
                if (_waveState.WaveDelayTimer >= 3f)
                {
                    _waveState.StartWave();
                } else
                {
                    return;
                }
            }

            // начинаем новую фазу волны, как только завершится период ожидания новой фазы волны
            if (_waveState.IsWaitingForWavePhase)
            {
                _waveState.AddTimeToWavePhaseDelayTimer(deltaTime);
                if (_waveState.WavePhaseDelayTimer >= currentWave.WavePhasesDelay)
                {
                    _waveState.StartPhase();
                } else
                {
                    return;
                }
            }

            // размещаем нового врага, как только завершится период ожидания размещения нового врага
            _waveState.AddTimeToEnemySpawnDelayTimer(deltaTime);
            if (_waveState.EnemySpawnDelayTimer >= currentWavePhase.EnemySpawnDelay)
            {
                SpawnEnemy(currentWavePhase);
            }
        }

        private void SpawnEnemy(WavePhase currentWavePhase)
        {
            _entityManager.AddEnemy(new Enemy(currentWavePhase.EnemyPath, _tileSize, currentWavePhase.EnemyType)); // передавать tileSize
            _waveState.AddEnemy();
        }
    }
}
