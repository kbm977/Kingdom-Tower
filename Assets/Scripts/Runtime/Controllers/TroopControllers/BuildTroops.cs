using Runtime.Managers;
using Runtime.Signals;
using UnityEngine;
namespace Runtime.Controller
{
    public class BuildTroops : TroopController
    {
        public static float buildSpeedMultiplier = 1f;
        public override void Attack()
        {
            animator.SetBool("CanAttack", true);
            animator.SetBool("Follow", false);
            base.Attack();
        }
        public override void LaunchAttack()
        {
            animator.SetTrigger("CoolDownFinish");
            AudioManager.instance.OnPlaySFXEffect(AudioLibrary.Instance.EngineerBuildSFX, source.engineerSource);
            _attackCoolDown = (troopData.data.coolDown * buildSpeedMultiplier);
        }

        public override void Damage()
        {
            if (_target == null) return;
            if (_target.GetComponent<BuildController>().Build())
            {
                animator.SetBool("CanAttack", false);
                _target = null;
            }
        }
        public override void MoveTowardsTarget()
        {
            animator.SetBool("CanAttack", false);
            animator.SetBool("Follow", true);
            base.MoveTowardsTarget();
        }
        public override void UpdateStats()
        {
            _attackCoolDown = (troopData.data.coolDown * buildSpeedMultiplier);
            Debug.LogWarning($"{this.name}:\ncooldown: {_attackCoolDown} multiplier: {buildSpeedMultiplier}");
        }
        private void OnEnable()
        {
            health = maxHealth;
            healthBar.UpdateHealthBar(health, maxHealth);
            TroopSignals.Instance.onUpdateStats += UpdateStats;
        }

        private void OnDisable()
        {
            if (!TroopSignals.Instance) return;
            TroopSignals.Instance.onUpdateStats -= UpdateStats;
        }
    }
}
