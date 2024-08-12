using Runtime.Controller;
using Runtime.Managers;
using Runtime.Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class Wizard : TroopController
    {
        public static float wizardHealthMultiplier = 1, wizardDamageMultiplier = 1, rangeMultiplier = 1, effectMultiplier = 1;

        private float _effect = 0.75f;

        public override void Attack()
        {
            _range = (troopData.data.attackRange * rangeMultiplier) + 3;
            animator.SetBool("EnemyInRange", true);
            base.Attack();
        }

        public override void LaunchAttack()
        {
            base.LaunchAttack();
            AudioManager.instance.OnPlaySFXEffect(AudioLibrary.Instance.WizardAttackSFX, source.wizardSource);
            if (_target.GetComponent<TroopController>().troopData.type == AgentType.Knight)
            {
                _target.GetComponent<TroopController>().AttackedByWizard(_effect);
            }
            Damage();
        }

        public override void MoveTowardsTarget()
        {
            _range = (troopData.data.attackRange * rangeMultiplier);
            animator.SetBool("EnemyClose", false);
            animator.SetBool("EnemyInRange", false);
            base.MoveTowardsTarget();
        }
        public override void UpdateStats()
        {
            maxHealth = troopData.data.health * wizardHealthMultiplier;
            health = (troopData.data.health * wizardHealthMultiplier);
            Mathf.Clamp(health, 0, maxHealth);
            _effect = (0.75f * effectMultiplier);
            _range = (troopData.data.attackRange * rangeMultiplier);
            _damage = (troopData.data.damage * wizardDamageMultiplier);
            Debug.LogWarning($"{this.name}:\nHealth: {health} HealthMultiplayer: {wizardHealthMultiplier}\neffect {_effect} Multiplier: {effectMultiplier}/n " +
                $"Speed: {_range} Multiplier: {rangeMultiplier}\n damage: {_damage} multiplier: {wizardDamageMultiplier}");
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
