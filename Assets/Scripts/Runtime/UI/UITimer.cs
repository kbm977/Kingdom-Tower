using Runtime.Managers;
using Runtime.Signals.Game;
using TMPro;
using UnityEngine;

namespace Runtime.UI
{
    public class UITimer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timer;

        private float _timer = 90f;

        private void Update()
        {
            if (_timer <= 0) return;
            _timer -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(_timer / 60);
            int seconds = Mathf.FloorToInt(_timer % 60);
            timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        public void SkipTimer()
        {
            GameSignals.Instance.onChangeGameState?.Invoke(GameState.Defend);
        }
        private void OnEnable()
        {
            _timer = 90;
        }
    }
}
