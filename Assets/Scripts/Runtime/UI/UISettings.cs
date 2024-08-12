using Runtime.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UI
{
    public class UISettings : MonoBehaviour
    {
        [SerializeField] private GameObject settings, audioSettings;
        private bool _inSettings = false;

        private void Start()
        {
            Screen.SetResolution(Screen.width, Screen.height, true);
        }
        public void SwitchSetting()
        {
            AudioManager.instance.OnPlaySFXEffect(AudioLibrary.Instance.UIClickSFX);
            if (_inSettings)
            {
                settings.SetActive(true);
                audioSettings.SetActive(false);
                _inSettings = false;
                return;
            }
            settings.SetActive(false);
            audioSettings.SetActive(true);
            _inSettings = true;
        }


    }
}
