using Runtime.Signals.UI;
using UnityEngine;
using DG.Tweening;
using Runtime.Managers;

namespace Runtime.UI
{
    public class UIElement : MonoBehaviour
    {
        [SerializeField] private Transform buildPos, defendPos;
        [SerializeField] private Ease easeType;

        private bool _force;
        private void Start()
        {
            transform.position = buildPos.position;
        }

        private void SwitchToBuildState()
        {
            _force = false;
            ToggleButton(transform.gameObject);
            //transform.DOMove(buildPos.position, 0.3f).SetEase(easeType);
        }

        private void SwitchToDefendState()
        {
            _force = true;
            ToggleButton(transform.gameObject);
            //transform.DOMove(defendPos.position, 0.3f).SetEase(easeType);
        }

        public void ToggleButton(GameObject target)
        {
            UISignals.Instance.onPlaySound?.Invoke(AudioLibrary.Instance.UIPopSFX);
            if (target.transform.position == buildPos.position || _force)
            {
                _force = false;
                target.transform.DOMove(defendPos.position, 0.3f).SetEase(easeType);
            }
            else if (target.transform.position == defendPos.position || !_force)
            { 
                _force = true;
                target.transform.DOMove(buildPos.position, 0.3f).SetEase(easeType);
            }
            else
            {
                Debug.LogWarning($"{this.name} IS IN A WRONG SPOT");
            }
        }

        //Subscribing to events
        private void OnEnable()
        {
            UISignals.Instance.onSwitchingToBuild += SwitchToBuildState;
            UISignals.Instance.onSwitchingToDefend += SwitchToDefendState;
        }
        private void OnDisable()
        {
            if (!UISignals.Instance) return;
            UISignals.Instance.onSwitchingToDefend -= SwitchToDefendState;
            UISignals.Instance.onSwitchingToBuild -= SwitchToBuildState;
        }
    }
}
