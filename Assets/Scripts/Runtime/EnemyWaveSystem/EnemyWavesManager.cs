using Runtime.Signals;
using Runtime.Signals.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Managers
{
    public class EnemyWavesManager : MonoBehaviour
    {
        [SerializeField] private WaveSO[] levelData;
        [SerializeField] public Transform[] spawnTransform;
        [SerializeField] private DialogueTrigger[] dialogueTrigger;
        [SerializeField] private AudioSource goblinSource, ogreSource;

        private int _waveCount, _goblinCount, _ogreCount, _remainingGoblins, _levelCount, _spawnCount;

        public int[] GetWaveInformation()
        {
            int[] waveInfo = new int[2];
            waveInfo[0] = levelData[_levelCount].waveGoblins[_waveCount].goblinCount;
            waveInfo[1] = levelData[_levelCount].waveGoblins[_waveCount].ogreCount;
            return waveInfo; 
        }

        private void OnChangeGameState(GameState state)
        {
            if (state == GameState.Defend) StartWave();
        }
        
        private void StartWave()
        {
            _goblinCount = levelData[_levelCount].waveGoblins[_waveCount].goblinCount;
            _ogreCount = levelData[_levelCount].waveGoblins[_waveCount].ogreCount;
            _remainingGoblins = _goblinCount + _ogreCount;
            Debug.Log(_remainingGoblins);
            _waveCount++;

            StartCoroutine(SpawnGoblins());
        }

        private IEnumerator SpawnGoblins()
        {
            for(int i = 0; i < _goblinCount; i++)
            {
                int seed = System.DateTime.Now.Millisecond + i;
                Random.InitState(seed);
                int n = Random.Range(_spawnCount, levelData[_levelCount].spawnPosCount + _spawnCount);
                Debug.Log($"min: {_spawnCount} max: {levelData[_levelCount].spawnPosCount} result: {n}");
                Instantiate(levelData[_levelCount].goblin, spawnTransform[n].position, spawnTransform[n].rotation);
                yield return new WaitForSeconds(0.5f);
            }
            AudioManager.instance.OnPlaySFXEffect(AudioLibrary.Instance.SpawnGoblinSFX, goblinSource);
            for (int i = 0; i < _ogreCount; i++)
            {
                int n = Random.Range(_spawnCount, levelData[_levelCount].spawnPosCount + _spawnCount);
                Debug.Log($"min: {_spawnCount} max: {levelData[_levelCount].spawnPosCount} result: {n}");
                Instantiate(levelData[_levelCount].ogre, spawnTransform[n].position, spawnTransform[n].rotation);
                yield return new WaitForSeconds(1f);
            }
            if (_ogreCount > 0) AudioManager.instance.OnPlaySFXEffect(AudioLibrary.Instance.SpawnOgreSFX, ogreSource);
        }

        private void OnGoblinDied()
        {
            _remainingGoblins--;

            if (_remainingGoblins > 0) return;
            AudioManager.instance.OnPlayMusic(AudioLibrary.Instance.MainMusic);
            TroopSignals.Instance.onTowerReward?.Invoke();
            if (_waveCount <= 2)
            {
                GameSignals.Instance.onChangeGameState?.Invoke(GameState.Build);
                return;
            }
            dialogueTrigger[_levelCount].TriggerDialogue();
            if (_levelCount >= 2) return;
            _spawnCount = levelData[_levelCount].spawnPosCount;
            _levelCount++; _waveCount = 0;
            GameSignals.Instance.onNextLevel?.Invoke();
        }

        private void OnLose() => dialogueTrigger[dialogueTrigger.Length - 1].TriggerDialogue();

        private void OnEnable()
        {
            GameSignals.Instance.onLose += OnLose;
            GameSignals.Instance.onChangeGameState += OnChangeGameState;
            EnemyWavesSignals.Instance.onGoblinDied += OnGoblinDied;
        }

        private void OnDisable()
        {
            if (GameSignals.Instance)
            {
                GameSignals.Instance.onChangeGameState -= OnChangeGameState;
                GameSignals.Instance.onLose -= OnLose;
            }

            if (EnemyWavesSignals.Instance) EnemyWavesSignals.Instance.onGoblinDied -= OnGoblinDied;
        }
    }
}
