using Runtime.Signals.Game;
using Runtime.Signals.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Managers.UI
{
    public class UIManager : MonoBehaviour
    {
        //These game objects hold unique UI elements for each state that does not exist in the other.
        //This allows for a dynamic adjustion of UI elements in the future
        [SerializeField] private GameObject buildUI, defendUI;
        [SerializeField] private AudioSource source;
        //Checks whcih state its entering and calling the appropriate function
        private void OnChangGameState(GameState gameState)
        {
            Debug.Log($"Changing to {gameState} scene");
            switch (gameState)
            {
                case GameState.Build:
                    SetBuildScene();
                    break;

                case GameState.Defend:
                    SetDefenseScene();
                    break;

                default: 
                    break;
            }
        }

        //Invoke actions required by UI elements to adjust themselves when switching to build state
        private void SetBuildScene()
        {
            buildUI.SetActive(true);
            defendUI.SetActive(false);
            UISignals.Instance.onSwitchingToBuild?.Invoke();
        }

        //Invoke actions required by UI elements to adjust themselves when switching to defend state
        private void SetDefenseScene()
        {
            buildUI.SetActive(false);
            defendUI.SetActive(true);
            UISignals.Instance.onSwitchingToDefend?.Invoke();
        }

        private void OnPlaySound(AudioClip clip)
        {
            AudioManager.instance.OnPlaySFXEffect(clip, source);
        }

        //Subscribing to events
        private void OnEnable()
        {
            UISignals.Instance.onPlaySound += OnPlaySound;
            GameSignals.Instance.onChangeGameState += OnChangGameState;
        }
        private void OnDisable()
        {
            if (UISignals.Instance)
                UISignals.Instance.onPlaySound -= OnPlaySound;
            if (!GameSignals.Instance) return;
            GameSignals.Instance.onChangeGameState -= OnChangGameState;
        }
    }
}
