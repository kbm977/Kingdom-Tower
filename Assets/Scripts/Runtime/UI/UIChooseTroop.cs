using Runtime.Managers;
using Runtime.Signals;
using Runtime.Signals.Game;
using Runtime.Signals.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class UIChooseTroop : MonoBehaviour
    {
        [SerializeField] private TroopsSO troopData;
        [SerializeField] private TextMeshProUGUI troopCount, troopCost;
        [SerializeField] private GameObject troopControlPanel;

        private int _knightCount, _magicianCount, _engineerCount;

        //Add to troop counters and check for orbs availablity. If the player cant afford it, the button is disabled
        private void OnAddTroop()
        {
            if (!GamaManager.AffordCost(-troopData.entity.entityCost)) return;
            GameSignals.Instance.onAddOrbs?.Invoke(-troopData.entity.entityCost);
            Debug.Log($" {this.name} + {troopData.type} has been added");
            if (troopData.type == AgentType.Knight)
            {
                _knightCount++;
                TroopManager.KnightCount = _knightCount;
                UpdateCounter(_knightCount);
            }
            else if (troopData.type == AgentType.Magician)
            {
                _magicianCount++;
                TroopManager.WizardCount = _magicianCount;
                UpdateCounter(_magicianCount);
            }
            else
            {
                _engineerCount++;
                TroopManager.EngineerCount = _engineerCount;
                UpdateCounter(_engineerCount);
            }
        }

        //Troop count are in the UI, so this is responsible for removing troops and refunding orbs
        private void OnRemoveTroop(bool refund)
        {
            if (troopData.type == AgentType.Knight)
            {
                if (_knightCount == 0) return;
                _knightCount--;
                TroopManager.KnightCount = _knightCount;
                UpdateCounter(_knightCount);
            }
            else if (troopData.type == AgentType.Magician)
            {
                if (_magicianCount == 0) return;
                _magicianCount--;
                TroopManager.WizardCount = _magicianCount;
                UpdateCounter(_magicianCount);
            }
            else
            {
                if (_engineerCount == 0) return;
                _engineerCount--;
                TroopManager.EngineerCount = _engineerCount;
                UpdateCounter(_engineerCount);
            }
            if (!refund) return;
            GameSignals.Instance.onAddOrbs?.Invoke(troopData.entity.entityCost);
            //Debug.Log($" {troopData.type} has been removed");
        }

        //Update UI orb display
        private void UpdateCounter(int number)
        {
            this.troopCount.text = number.ToString();
        }

        //Placing the troop control panel in the right spot depending on the button's position
        public void SetTroopControlPanel()
        {
            UISignals.Instance.onAddTroop = OnAddTroop;
            UISignals.Instance.onRemoveTroop = OnRemoveTroop;

            OnTroopControlToggle();
            troopCost.text = troopData.entity.entityCost.ToString();
            troopControlPanel.transform.position = new Vector3(transform.position.x, (transform.position.y) * 2, transform.position.z);
        }

        public void ChooseTroopEntity()
        {
            InputSignals.Instance.onSelectEntity?.Invoke(troopData);
        }

        //Toggles the state of the troop control panel
        private void OnTroopControlToggle()
        {
            UISignals.Instance.onPlaySound?.Invoke(AudioLibrary.Instance.UIClickSFX);
            troopControlPanel.SetActive(true);
        }

        private void OnChangeGameState(GameState state)
        {
            switch (state)
            {
                case GameState.Build:
                    GetComponent<Button>().onClick.RemoveAllListeners();
                    GetComponent<Button>().onClick.AddListener(SetTroopControlPanel);
                    break;

                case GameState.Defend:
                    GetComponent<Button>().onClick.RemoveAllListeners();
                    GetComponent<Button>().onClick.AddListener(ChooseTroopEntity);
                    break;

                default:
                    break;
            }
        }

        private void OnUpdateCounters(int knightCount, int wizardCount, int engineerCount)
        {
            if (troopData.type == AgentType.Knight)
            {
                _knightCount += knightCount;
                TroopManager.KnightCount = _knightCount;
                UpdateCounter(_knightCount);
            }
            else if (troopData.type == AgentType.Magician)
            {
                _magicianCount += wizardCount;
                TroopManager.WizardCount = _magicianCount;
                UpdateCounter(_magicianCount);
            }
            else
            {
                _engineerCount += engineerCount;
                TroopManager.EngineerCount = _engineerCount;
                UpdateCounter(_engineerCount);
            }
        }

        //Subscribe to events
        private void OnEnable()
        {
            UISignals.Instance.onUpdateCounters += OnUpdateCounters;
            GameSignals.Instance.onChangeGameState += OnChangeGameState;
        }
        private void OnDisable()
        {
            if (UISignals.Instance)
            {
                UISignals.Instance.onUpdateCounters -= OnUpdateCounters;
            }
            
            if (GameSignals.Instance)
                GameSignals.Instance.onChangeGameState -= OnChangeGameState;
        }
    }
}