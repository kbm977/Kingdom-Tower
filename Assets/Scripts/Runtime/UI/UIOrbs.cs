using Runtime.Signals.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Runtime.UI
{
    public class UIOrbs : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _orbTextField;

        private void OnUpdateOrb(int orbCount)
        {
            _orbTextField.text = orbCount.ToString();
        }

        private void OnEnable()
        {
            UISignals.Instance.onUpdateOrb += OnUpdateOrb;
        }

        private void OnDisable()
        {
            if (!UISignals.Instance) return;
            UISignals.Instance.onUpdateOrb -= OnUpdateOrb;
        }
    }
}
