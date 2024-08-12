using Runtime.Controller;
using Runtime.Managers;
using Runtime.Signals;
using Runtime.Signals.Game;
using Runtime.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Runtime
{
    public class BuildController : MonoBehaviour
    {
        public BuildSO buildData;

        [SerializeField] private HealthBar healthBar;
        [SerializeField] private Material mat;
        [SerializeField] private GameObject[] meshes;
        [SerializeField] private NavMeshObstacle obstacle;
        [SerializeField] private GameObject highlight;
        [SerializeField] private TroopManager tm;

        public float currentHealth = 0f, maxHealth;

        public static float buildingHealthMultiplier;
        public static bool revengeActive;

        private List<GameObject> _selectedBuilds = new();
        public bool alive = true;

        private int _wealthGenerated;

        private void Awake()
        {
            tm = FindObjectOfType<TroopManager>();
        }
        private void Start()
        {
            maxHealth = buildData.health + (buildData.health * buildingHealthMultiplier);

            if (buildData.buildType == BuildType.Castle) 
            { 
                currentHealth = maxHealth;
                healthBar.UpdateHealthBar(currentHealth, maxHealth);
            }
        }

        public bool Build()
        {
            if (currentHealth < buildData.health)
            { 
                currentHealth += buildData.health / buildData.buildTime;
                healthBar.UpdateHealthBar(currentHealth, buildData.health);
            }
            
            if (currentHealth >= buildData.health)
            {
                if (!obstacle.enabled) FinishBuild();

                return true;
            }
            return false;
        }

        private void FinishBuild()
        { 
            AudioManager.instance.OnPlaySFXEffect(AudioLibrary.Instance.PlaceBuildingSFX, tm.buildSource);
            obstacle.enabled = true;
            foreach(GameObject mesh in meshes)
            {
                mesh.GetComponent<MeshRenderer>().material = mat;
            }
            gameObject.layer = LayerMask.NameToLayer("Player");
            gameObject.tag = "Player";
            currentHealth = maxHealth;
        }

        public void Damage(float damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Max(0, currentHealth);
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
            if (currentHealth == 0 && alive) Die();
        }

        private void Die()
        {
            AudioManager.instance.OnPlaySFXEffect(AudioLibrary.Instance.DestroyBuilding, tm.buildSource);
            switch (buildData.buildType)
            {
                case BuildType.Castle:
                    GameSignals.Instance.onLose?.Invoke();
                    break;
                case BuildType.Tower:
                    GameSignals.Instance.onAddOrbs?.Invoke(-_wealthGenerated);
                    if (GamaManager.OrbCount <= 0) TroopSignals.Instance.onLockSkill?.Invoke();
                    break;
                default: break;
            }
            alive = false;
            Destroy(gameObject);
        }
        private void UpgradeBuildings()
        {
            Debug.LogWarning(maxHealth);
            maxHealth = buildData.health + (buildData.health * buildingHealthMultiplier);
            currentHealth += (buildData.health * buildingHealthMultiplier);
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }

        private void SelectBuild(BuildController build)
        {
            if (_selectedBuilds.Contains(build.gameObject))
            {
                _selectedBuilds.Remove(build.gameObject);
                build.highlight.SetActive(false);
            }
            else
            {
                _selectedBuilds.Add(build.gameObject);
                build.highlight.SetActive(true);
            }
        }

        private void OnDeselectBuilds()
        {
            for (int i = 0; i < _selectedBuilds.Count; i++)
            {
                if (_selectedBuilds[i] == null) continue;
                _selectedBuilds[i].GetComponent<BuildController>().highlight.SetActive(false);
            }
            _selectedBuilds.Clear();
        }

        private void AddReward()
        {
            if (!obstacle.enabled) return;
            _wealthGenerated += buildData.wealthReturn;
            GameSignals.Instance.onAddOrbs?.Invoke(buildData.wealthReturn);
        }

        private void OnEnable()
        {
            TroopSignals.Instance.onTowerReward += AddReward;
            TroopSignals.Instance.onUpgradeBuildings += UpgradeBuildings;
            TroopSignals.Instance.onSelectBuild += SelectBuild;
            TroopSignals.Instance.onDeselectBuilds += OnDeselectBuilds;
        }
        private void OnDisable()
        {
            if (!TroopSignals.Instance) return;
            TroopSignals.Instance.onTowerReward -= AddReward;
            TroopSignals.Instance.onUpgradeBuildings -= UpgradeBuildings;
            TroopSignals.Instance.onSelectBuild -= SelectBuild;
            TroopSignals.Instance.onDeselectBuilds -= OnDeselectBuilds;
        }
    }
}