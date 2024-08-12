using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Managers
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource musicSource, sfxSource;

        public float musicVolume = 0.5f, sfxVolume = 1f;
        public static AudioManager instance;

        public void ChangeMusicVolume(Slider slider)
        {
            musicVolume = slider.value;
            musicSource.volume = musicVolume;
        }
        public void ChangeSFXVolume(Slider slider)
        {
            sfxVolume = slider.value;
            sfxSource.volume = sfxVolume;
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void OnPlayMusic(AudioClip music)
        {
            StartCoroutine(SwitchMusic(music));
        }

        IEnumerator SwitchMusic(AudioClip music)
        {
            float original = musicSource.volume;
            float duration = 1;
            for (float t = 0f; t < duration + 2; t += Time.deltaTime)
            {
                musicSource.volume = Mathf.Lerp(musicSource.volume, 0f, t / (duration + 2));
                yield return null;
            }
            musicSource.volume = 0.1f;
            musicSource.clip = music;
            musicSource.Play();
            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                musicSource.volume = Mathf.Lerp(musicSource.volume, original, t / duration);
                yield return null;
            }
            musicSource.volume = original;
            Debug.Log("WOW");
        }

        public void OnPlaySFXEffect(AudioClip sfx, AudioSource source)
        {
            if (sfx == source.clip && source.isPlaying) return;
            source.volume = sfxSource.volume;
            source.clip = sfx;
            source.Play();
        }
        public void OnPlaySFXEffect(AudioClip sfx)
        {
            if (sfx == sfxSource.clip && sfxSource.isPlaying) return;
            sfxSource.clip = sfx;
            sfxSource.Play();
        }
    }
}
