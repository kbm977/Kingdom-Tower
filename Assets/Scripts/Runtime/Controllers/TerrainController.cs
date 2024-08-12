using DG.Tweening;
using Runtime.Signals.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Controller
{
    public class TerrainController : MonoBehaviour
    {
        [SerializeField] private Vector2[] levelPos;

        int _levelCount;
        Vector3 _terrainPos;

        private void OnNextLevel()
        {
            if (_levelCount >= 2) return;
            _terrainPos = new Vector3(levelPos[_levelCount].x, 0, levelPos[_levelCount].y);
            gameObject.transform.DOMove(_terrainPos, 3f).SetEase(Ease.OutSine);
            _levelCount++;
        }

        private void OnEnable()
        {
            GameSignals.Instance.onNextLevel += OnNextLevel;
        }

        private void OnDisable()
        {
            if (GameSignals.Instance) GameSignals.Instance.onNextLevel -= OnNextLevel;
        }
    }
}
