using Runtime.Signals;
using Runtime.Signals.Game;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Controller
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField] private TroopController troop;

        private void Damage()
        {
            troop.Damage();
        }

        private void Die()
        {
            troop.gameObject.SetActive(false);
        }

        private void DiePermenant()
        {
            GameSignals.Instance.onAddOrbs?.Invoke(troop.troopData.entity.entityCost);
            EnemyWavesSignals.Instance.onGoblinDied?.Invoke();
            Destroy(troop.gameObject);
        }
    }
}
