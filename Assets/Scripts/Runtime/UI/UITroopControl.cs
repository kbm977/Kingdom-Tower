using Runtime.Controller;
using Runtime.Managers;
using Runtime.Signals;
using Runtime.Signals.Game;
using Runtime.Signals.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class UITroopControl : MonoBehaviour
    {
        [SerializeField] private GameObject entityPreview;
        [SerializeField] private Image entityImage;
        [SerializeField] private TextMeshProUGUI entityName, currentHealth, entityCost;
        [SerializeField] private Slider healthBar;
        [SerializeField] private GameObject actionButtons;

        [SerializeField] private Texture2D normalMouseCursor, swordMouseCursor;

        private TroopController _selectedTroop;
        private BuildController _selectedBuild;
        public void OnCancel()
        {
            UISignals.Instance.onCancel?.Invoke();
        }

        public void AddTroop()
        {
            UISignals.Instance.onAddTroop?.Invoke();
        }

        public void RemoveTroop()
        {
            UISignals.Instance.onRemoveTroop?.Invoke(true);
        }

        private void Update()
        {
            if (_selectedTroop)
            {
                healthBar.value = _selectedTroop.health / _selectedTroop.maxHealth;
                currentHealth.text = $"{(int)_selectedTroop.health} / {(int)_selectedTroop.maxHealth}";
            }
            else if (_selectedBuild)
            {
                healthBar.value = _selectedBuild.currentHealth / _selectedBuild.maxHealth;
                currentHealth.text = $"{(int)_selectedBuild.currentHealth} / {(int)_selectedBuild.maxHealth}";
            }
        }
        private void OnSelectTroop(TroopController troop)
        {
            entityPreview.SetActive(true);

            if (troop.troopData.entity)
            {
                _selectedTroop = troop;
                entityImage.sprite = _selectedTroop.troopData.entity.entityImage;
                entityName.text = _selectedTroop.troopData.entity.entityName;
                entityCost.text = _selectedTroop.troopData.entity.entityCost.ToString();
            }

            if (troop.gameObject.CompareTag("Enemy"))
            {
                actionButtons.SetActive(false);
            }
            else
            {
                actionButtons.SetActive(true);
            }
        }

        private void OnDeselect()
        {
            _selectedBuild = null;
            _selectedTroop = null;
            GameSignals.Instance.onCursorChange?.Invoke(false, normalMouseCursor);
            entityPreview.SetActive(false);
        }

        public void OnCursorChangeTarget()
        {
            GameSignals.Instance.onCursorChange?.Invoke(true, swordMouseCursor);
        }

        public void OnSacrificeTroops()
        {
            TroopSignals.Instance.onSacrificeTroops?.Invoke();
            OnDeselect();
        }

        private void OnSelectBuild(BuildController build)
        { 
            entityPreview.SetActive(true);
            actionButtons.SetActive(false);

            if (build.buildData.entity)
            {
                _selectedBuild = build;
                entityImage.sprite = _selectedBuild.buildData.entity.entityImage;
                entityName.text = _selectedBuild.buildData.entity.entityName;
            }
        }

        private void OnEnable()
        {
            TroopSignals.Instance.onSelectTroop += OnSelectTroop;
            TroopSignals.Instance.onSelectBuild += OnSelectBuild;
            TroopSignals.Instance.onDeselectTroops += OnDeselect;
            TroopSignals.Instance.onDeselectBuilds += OnDeselect;
        }

        private void OnDisable()
        {
            if (TroopSignals.Instance == null) return;
            TroopSignals.Instance.onSelectBuild -= OnSelectBuild;
            TroopSignals.Instance.onSelectTroop -= OnSelectTroop;
            TroopSignals.Instance.onDeselectBuilds -= OnDeselect;
            TroopSignals.Instance.onDeselectTroops -= OnDeselect;
        }
    }
}
