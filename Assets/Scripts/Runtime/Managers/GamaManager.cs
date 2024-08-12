using UnityEngine;
using Runtime.Signals.Game;
using Runtime.Signals.UI;
using Runtime.Signals;

namespace Runtime.Managers
{
    public class GamaManager : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        private float _stateTimer = 90f;
        private GameState _gameState;

        private int _orbCounter = 0;

        public static int OrbCount { get; private set; }

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            OnAddOrbs(1000);
        }

        private void Update()
        {
            _stateTimer -= Time.deltaTime;
            if (_stateTimer <= 0 && _gameState == GameState.Build)
            {
                GameSignals.Instance.onChangeGameState?.Invoke(GameState.Defend);
            }
        }

        private void OnChangeGameState(GameState newState)
        {
            _gameState = newState;
            UISignals.Instance.onCancel?.Invoke();
            if (_gameState == GameState.Build)
            {
                _stateTimer = 90f;
            }
            else
            {
                AudioManager.instance.OnPlayMusic(AudioLibrary.Instance.BattleMusic);
            }
        }

        public static bool AffordCost(int cost)
        {
            if (Mathf.Abs(cost) > OrbCount && cost < 0)
            {
                return false;
            }
            return true;
        }

        private void OnAddOrbs(int amount)
        {
            if (!AffordCost(amount)) return;
            if (amount > 0) AudioManager.instance.OnPlaySFXEffect(AudioLibrary.Instance.IncreaseOrbs, source);
            else AudioManager.instance.OnPlaySFXEffect(AudioLibrary.Instance.DecreaseOrbs, source);


            _orbCounter += amount;
            OrbCount = _orbCounter;
            UISignals.Instance.onUpdateOrb?.Invoke(_orbCounter);
        }

        private void OnCursorChange(bool state, Texture2D cursor)
        {
            //Vector2 size = new(cursor.width/2, cursor.height/2);
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        }

        //Subscribing to events
        private void OnEnable()
        {
            GameSignals.Instance.onChangeGameState += OnChangeGameState;
            GameSignals.Instance.onAddOrbs += OnAddOrbs;
            GameSignals.Instance.onCursorChange += OnCursorChange;
        }
        private void OnDisable()
        {
            if (!GameSignals.Instance) return;
            GameSignals.Instance.onChangeGameState -= OnChangeGameState;
            GameSignals.Instance.onAddOrbs -= OnAddOrbs;
            GameSignals.Instance.onCursorChange -= OnCursorChange;
        }
    }

    public enum GameState
    {
        Build,
        Defend,
        Attack
    }
}