using Runtime.Managers;
using Runtime.Signals;
using Runtime.UI;
using System.Threading;
using UnityEngine;

namespace Runtime.Controller
{
    public class Knight : TroopController
    {
        public static bool revengeActivated = false;
        public static float healthMultiplier = 1, damageMultiplayer = 1, speedMultiplayer = 1, attackSpeedMultiplier = 1;
        
        public override void Attack()
        {
            animator.SetBool("CanAttack", true);
            animator.SetBool("Follow", false);
            base.Attack();
        }

        public override void LaunchAttack()
        {
            animator.SetTrigger("CoolDownFinish");
            AudioManager.instance.OnPlaySFXEffect(AudioLibrary.Instance.SwordStabSFX, source.knightSource);
            _attackCoolDown = (troopData.data.coolDown * attackSpeedMultiplier);
        }

        public override void MoveTowardsTarget()
        {
            animator.SetBool("CanAttack", false);
            animator.SetBool("Follow", true); 
            base.MoveTowardsTarget();
        }

        public override void UpdateStats()
        {
            maxHealth = troopData.data.health * healthMultiplier;
            health = (troopData.data.health * healthMultiplier) - (maxHealth - health);
            Mathf.Clamp(health, 0, maxHealth);
            _attackCoolDown = (troopData.data.coolDown * attackSpeedMultiplier);
            agent.speed = _speed = (troopData.data.speed * speedMultiplayer);
            _damage = (troopData.data.damage * damageMultiplayer);
            Debug.LogWarning($"{this.name}:\nHealth: {health} HealthMultiplayer: {healthMultiplier}\nCooldown {_attackCoolDown} Multiplier: {attackSpeedMultiplier}/n " +
                $"Speed: {agent.speed} Multiplier: {speedMultiplayer}\n damage: {_damage} multiplier: {damageMultiplayer}");
        }

        public override void Die()
        {
            if (revengeActivated) Revenge();
            base.Die();
        }

        public override void Damage()
        {
            base.Damage();
        }

        private void Revenge()
        {
            if (_target == null) return;
            _target.GetComponent<TroopController>().health -= 50;
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
