using Runtime.Controller;
using Runtime.Managers;
using Runtime.Signals;
using Runtime.Signals.Game;
using Runtime.Signals.UI;
using Runtime.UI;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime
{
    public class TroopManager : MonoBehaviour
    {
        private readonly List<GameObject> _knightList = new(), _magicianList = new(), _engineerList = new(), _selectedAgents = new();
        private readonly Stack<UISkilltree> _skillTree = new();
        private GameObject _summonedTroop;

        public static int KnightCount, WizardCount, EngineerCount;

        public AudioSource knightSource, wizardSource, engineerSource, buildSource, goblinSource, ogreSource;
 
        private void OnSummonTroop(TroopsSO newTroop, Vector3 position)
        {
            switch (newTroop.type)
            {
                case AgentType.Knight:
                    if (KnightCount <= 0)
                    {
                        if (!GamaManager.AffordCost(-newTroop.entity.entityCost * 2)) return;
                        GameSignals.Instance.onAddOrbs?.Invoke(-newTroop.entity.entityCost * 2);
                    }
                    if (CheckTroopList(_knightList))
                    {
                        _summonedTroop.transform.position = position;
                        _summonedTroop.SetActive(true);
                    }
                    else
                    {
                        _summonedTroop = Instantiate(newTroop.entity.entityPrefab, position, transform.rotation);
                        Debug.Log(KnightCount);
                        _knightList.Add(_summonedTroop);
                    }
                    AudioManager.instance.OnPlaySFXEffect(AudioLibrary.Instance.SpawnKnightSFX, knightSource);
                    if (KnightCount <= 0) GameSignals.Instance.onAddOrbs?.Invoke(-newTroop.entity.entityCost * 2);
                    break;

                case AgentType.Magician:
                    if (WizardCount <= 0)
                    {
                        if (!GamaManager.AffordCost(-newTroop.entity.entityCost * 2)) return;
                        GameSignals.Instance.onAddOrbs?.Invoke(-newTroop.entity.entityCost * 2);
                    }
                    if (CheckTroopList(_magicianList))
                    {
                        _summonedTroop.transform.position = position;
                        _summonedTroop.SetActive(true);
                    }
                    else
                    {
                        _summonedTroop = Instantiate(newTroop.entity.entityPrefab, position, transform.rotation); 
                        Debug.Log(WizardCount);
                        _magicianList.Add(_summonedTroop);
                    }
                    if (WizardCount <= 0) GameSignals.Instance.onAddOrbs?.Invoke(-newTroop.entity.entityCost * 2);
                    AudioManager.instance.OnPlaySFXEffect(AudioLibrary.Instance.SpawnWizardSFX, wizardSource);
                    break;

                case AgentType.Engineer:
                    if (EngineerCount <= 0) 
                    {
                        if (!GamaManager.AffordCost(-newTroop.entity.entityCost * 2)) return;
                        GameSignals.Instance.onAddOrbs?.Invoke(-newTroop.entity.entityCost * 2); 
                    }
                    if (CheckTroopList(_engineerList))
                    {
                        _summonedTroop.transform.position = position;
                        _summonedTroop.SetActive(true);
                    }
                    else
                    {
                        _summonedTroop = Instantiate(newTroop.entity.entityPrefab, position, transform.rotation);
                        Debug.Log(EngineerCount);
                        _engineerList.Add(_summonedTroop);
                    }
                    AudioManager.instance.OnPlaySFXEffect(AudioLibrary.Instance.SpawnEngineerSFX, engineerSource);
                    break;
                default:
                    break;
            }
            UISignals.Instance.onRemoveTroop?.Invoke(false);
        }

        private bool CheckTroopList(List<GameObject> troopList)
        {
            for (int i = 0; i < troopList.Count; i++)
            {
                if (!troopList[i].activeSelf)
                {
                    _summonedTroop = troopList[i];
                    return true;
                }
            }
            return false;
        }

        private void OnSelectTroop(TroopController troop)
        {
            if (_selectedAgents.Contains(troop.gameObject))
            {
                _selectedAgents.Remove(troop.gameObject);
                troop.highLight.SetActive(false);
            }
            else
            {
                _selectedAgents.Add(troop.gameObject);
                troop.highLight.SetActive(true);
            }
            if (_selectedAgents.Count == 0) return;
            if (!_selectedAgents[0].CompareTag(troop.gameObject.tag))
            {
                TroopSignals.Instance.onDeselectTroops?.Invoke();
            }
        }
        
        private void OnDeselectTroops()
        {
            for (int i = 0; i < _selectedAgents.Count;i++)
            {
                if (_selectedAgents[i] == null) continue;
                _selectedAgents[i].GetComponent<TroopController>().highLight.SetActive(false);
            }
            _selectedAgents.Clear();
        }

        private void OnSacrificeTroops()
        {
            //Play Death and PE
            foreach (var agent in _selectedAgents)
            {
                if (!agent.activeSelf) continue;
                agent.GetComponent<TroopController>().Die();
                GameSignals.Instance.onAddOrbs?.Invoke(agent.GetComponent<TroopController>().troopData.entity.entityCost / 2);
            }
            AudioManager.instance.OnPlaySFXEffect(AudioLibrary.Instance.KnightDiesSFX, knightSource);
        }
        private void OnSwitchTarget(GameObject target)
        {
            foreach (var agent in _selectedAgents)
            {
                //agent.GetComponent<TroopController>().Die();
                if (!agent.GetComponent<TroopController>()) return;
                agent.GetComponent<TroopController>().ChangeTarget(target);
            }
            //AudioManager.instance.OnPlaySFXEffect(AudioLibrary.Instance.SwitchTarget);
        }

        private void OnChangeGameState(GameState gameState)
        {
            if (gameState != GameState.Build) return;
            int liveEngineer =0, knightLiving= 0, wizardLiving = 0;
            //Debug.Log("Should remove" + _engineerList.Count.ToString() + " " + _knightList.Count.ToString() + " " + 
            //    _magicianList.Count.ToString());
            for (int i = 0; i < _engineerList.Count; i++)
            {
                if (!_engineerList[i].activeSelf) continue;
                _engineerList[i].SetActive(false);
                liveEngineer++;
            }
            for (int i = 0; i < _knightList.Count; i++)
            {
                if (!_knightList[i].activeSelf) continue;
                _knightList[i].SetActive(false);
                knightLiving++;
            }
            for (int i = 0; i < _magicianList.Count; i++)
            {
                if (!_magicianList[i].activeSelf) continue;
                _magicianList[i].SetActive(false);
                wizardLiving++;
            }
            UISignals.Instance.onUpdateCounters?.Invoke(knightLiving, wizardLiving, liveEngineer);
        }

        //Skills are assigned by numbers in the inspector
        private void OnUnlockSkill(TroopController troop, int rank, UISkilltree uiSkilltree)
        {
            _skillTree.Push(uiSkilltree);
            if (troop.troopData.type == AgentType.Knight)
            {
                switch (rank)
                {
                    case 0:
                        Knight.damageMultiplayer += 0.1f;
                        break;

                    case 1:
                        Knight.attackSpeedMultiplier -= 0.1f;
                        break;

                    case 2:
                        Knight.damageMultiplayer += 0.2f;
                        break;

                    case 3:
                        Knight.healthMultiplier += 0.25f;
                        break;

                    case 4:
                        Knight.speedMultiplayer += 0.3f;
                        break;

                    case 5:
                        Knight.revengeActivated = true;
                        break;

                    default: break;
                }
            }
            else if (troop.troopData.type == AgentType.Magician)
            {
                switch (rank)
                {
                    case 0:
                        Wizard.effectMultiplier += 0.1f;
                        break;

                    case 1:
                        Wizard.wizardDamageMultiplier += 0.1f;
                        break;

                    case 2:
                        Wizard.rangeMultiplier += 0.5f;
                        break;

                    case 3:
                        Wizard.wizardHealthMultiplier += 0.25f;
                        break;

                    case 4:
                        Wizard.wizardDamageMultiplier += 0.25f;
                        break;

                    case 5:
                        Wizard.effectMultiplier += 0.2f;
                        break;

                    default: break;
                }
            }
            else
            {
                switch (rank)
                {
                    case 0:
                        BuildTroops.buildSpeedMultiplier -= 0.15f;
                        break;

                    case 1:
                        BuildController.buildingHealthMultiplier += 0.25f;
                        TroopSignals.Instance.onUpgradeBuildings?.Invoke();
                        break;

                    case 2:
                        BuildController.buildingHealthMultiplier += 0.5f;
                        TroopSignals.Instance.onUpgradeBuildings?.Invoke();
                        break;

                    default: break;
                }
            }
            TroopSignals.Instance.onUpdateStats?.Invoke();
        }

        private void OnLockSkill()
        {
            if (_skillTree.Count == 0)
            {
                GameSignals.Instance.onLose?.Invoke(); 
                return;
            }
            UISkilltree skillTree = _skillTree.Pop();
            skillTree.Lock();
            Debug.Log(skillTree.name);
            TroopController troop = skillTree.troop;
            if (troop.troopData.type == AgentType.Knight)
            {
                switch (skillTree.rank)
                {
                    case 0:
                        Knight.damageMultiplayer -= 0.1f;
                        break;

                    case 1:
                        Knight.attackSpeedMultiplier += 0.1f;
                        break;

                    case 2:
                        Knight.damageMultiplayer -= 0.2f;
                        break;

                    case 3:
                        Knight.healthMultiplier -= 0.25f;
                        break;

                    case 4:
                        Knight.speedMultiplayer -= 0.3f;
                        break;

                    case 5:
                        Knight.revengeActivated = false;
                        break;

                    default: break;
                }
            }
            else if (troop.troopData.type == AgentType.Magician)
            {
                switch (skillTree.rank)
                {
                    case 0:
                        Wizard.effectMultiplier -= 0.1f;
                        break;

                    case 1:
                        Wizard.wizardDamageMultiplier -= 0.1f;
                        break;

                    case 2:
                        Wizard.rangeMultiplier -= 0.5f;
                        break;

                    case 3:
                        Wizard.wizardHealthMultiplier -= 0.25f;
                        break;

                    case 4:
                        Wizard.wizardDamageMultiplier -= 0.25f;
                        break;

                    case 5:
                        Wizard.effectMultiplier -= 0.2f;
                        break;

                    default: break;
                }
            }
            else
            {
                switch (skillTree.rank)
                {
                    case 0:
                        BuildTroops.buildSpeedMultiplier += 0.15f;
                        break;

                    case 1:
                        BuildController.buildingHealthMultiplier -= 0.25f;
                        TroopSignals.Instance.onUpgradeBuildings?.Invoke();
                        break;

                    case 2:
                        BuildController.buildingHealthMultiplier -= 0.5f;
                        TroopSignals.Instance.onUpgradeBuildings?.Invoke();
                        break;

                    default: break;
                }
            }
            TroopSignals.Instance.onUpdateStats?.Invoke();
            TroopSignals.Instance.onUpgradeBuildings?.Invoke();
        }

        //Subscribe to events
        private void OnEnable()
        {
            TroopSignals.Instance.onUnlockSkill += OnUnlockSkill;
            TroopSignals.Instance.onSummonTroop += OnSummonTroop;
            TroopSignals.Instance.onSelectTroop += OnSelectTroop;
            TroopSignals.Instance.onSacrificeTroops += OnSacrificeTroops;
            TroopSignals.Instance.onDeselectTroops += OnDeselectTroops;
            TroopSignals.Instance.onSwitchTarget += OnSwitchTarget;
            GameSignals.Instance.onChangeGameState += OnChangeGameState;
            TroopSignals.Instance.onLockSkill += OnLockSkill;
        }
        private void OnDisable()
        {
            if (GameSignals.Instance) GameSignals.Instance.onChangeGameState -= OnChangeGameState;
            if (TroopSignals.Instance == null) return;
            TroopSignals.Instance.onUnlockSkill -= OnUnlockSkill;
            TroopSignals.Instance.onSummonTroop -= OnSummonTroop;
            TroopSignals.Instance.onSelectTroop -= OnSelectTroop;
            TroopSignals.Instance.onSwitchTarget -= OnSwitchTarget;
            TroopSignals.Instance.onSacrificeTroops -= OnSacrificeTroops;
            TroopSignals.Instance.onDeselectTroops -= OnDeselectTroops;
            TroopSignals.Instance.onLockSkill -= OnLockSkill;
        }
    }
}