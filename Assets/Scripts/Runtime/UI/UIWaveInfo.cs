using Runtime.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Runtime
{
    public class UIWaveInfo : MonoBehaviour
    {
        [SerializeField] private EnemyWavesManager waveManager;
        [SerializeField] private TextMeshProUGUI goblinText, ogreText;
        private void OnEnable()
        {
            if (waveManager == null) return;
            goblinText.text = waveManager.GetWaveInformation()[0].ToString();
            ogreText.text = waveManager.GetWaveInformation()[1].ToString();
        }
    }
}
