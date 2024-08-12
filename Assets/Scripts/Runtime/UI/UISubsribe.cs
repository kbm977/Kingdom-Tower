using Runtime.Managers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime
{
    public class UISubsribe : MonoBehaviour
    {
        public Slider slider;
        public bool isMusic;
        private AudioManager audioManager;
        private void Start()
        {
            audioManager = FindObjectOfType<AudioManager>();

            if (isMusic) slider.value = audioManager.musicVolume;
            else slider.value = audioManager.sfxVolume;
        }

        public void SyncSlider()
        {
            if (isMusic) audioManager.ChangeMusicVolume(slider);
            else audioManager.ChangeSFXVolume(slider);
        }
    }
}
