using DG.Tweening;
using Runtime.Signals;
using Runtime.Signals.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Controller
{
    public class CastleTransitionController : MonoBehaviour
    {
        [SerializeField] private int hidePos;
        [SerializeField] private Vector3[] newPos;
        [SerializeField] private Vector3[] newRotation;
        [SerializeField] private GameObject mesh;

        private int _levelCount;
        private Camera _cam;

        private void Awake()
        {
            _cam = Camera.main;
        }

        private void OnNextLevel()
        {
            gameObject.transform.DOMoveY(hidePos, 1f).SetEase(Ease.OutBounce);
            StartCoroutine(MoveDown());
        }

        IEnumerator MoveDown()
        {
            yield return new WaitForSeconds(1f);
            mesh.SetActive(false);
            gameObject.transform.DOMove(new Vector3(newPos[_levelCount].x, hidePos, newPos[_levelCount].z), 1f);

            _cam.transform.DOMove(new Vector3(newPos[_levelCount].x - 5, _cam.transform.position.y, newPos[_levelCount].z - 5), 1f);
            gameObject.transform.DORotate(newRotation[_levelCount], 3f).SetEase(Ease.InOutBack);
            yield return new WaitForSeconds(1f);
            mesh.SetActive(true);
            gameObject.transform.DOMove(newPos[_levelCount], 1.5f).SetEase(Ease.InOutSine);
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
