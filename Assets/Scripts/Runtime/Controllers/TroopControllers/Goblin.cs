using Runtime.Controller;
using Runtime.Managers;
using Runtime.Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class Goblin : TroopController
    {
        [SerializeField] private Material matWhite, matBlue;
        [SerializeField] private SkinnedMeshRenderer mesh;
        //the lower the stronger
        [SerializeField] private float resistence;
        private float _timer, _wizardEffect = 1f;
        private int _wizardMultiplier;

        public override void Attack()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _wizardMultiplier = 0;
                _wizardEffect = 1f;
                agent.speed = _speed = troopData.data.speed;
                mesh.material = matWhite;
            }
            animator.SetBool("CanAttack", true);
            base.Attack();
        }

        public override void LaunchAttack()
        {
            animator.SetTrigger("CoolDownFinish");
            AudioManager.instance.OnPlaySFXEffect(AudioLibrary.Instance.GoblinStabSFX, source.goblinSource);
            _attackCoolDown = troopData.data.coolDown * _wizardEffect;
        }

        public override void MoveTowardsTarget()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0) 
            {
                _wizardMultiplier = 0;
                _wizardEffect = 1f;
                agent.speed = _speed = troopData.data.speed;
                mesh.material = matWhite;
            }
            animator.SetBool("CanAttack", false);
            base.MoveTowardsTarget();
        }

        public override void AttackedByWizard(float effect)
        {
            _wizardMultiplier++;
            _timer = 5f;
            mesh.material = matBlue;
            _wizardEffect = (effect * resistence) * _wizardMultiplier;
            Mathf.Clamp(_wizardEffect, 1, 5);
            agent.speed = _speed = troopData.data.speed / _wizardEffect;
        }
    }
}
