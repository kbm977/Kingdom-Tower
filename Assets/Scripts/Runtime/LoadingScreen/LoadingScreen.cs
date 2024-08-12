using Runtime.Managers;
using Runtime.Signals.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Runtime
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private Image loadingBarFill;

        public void LoadScene(int sceneID)
        {
            AudioManager.instance.OnPlaySFXEffect(AudioLibrary.Instance.UIClickSFX);
            StartCoroutine(LoadSceneAsync(sceneID));
        }

        IEnumerator LoadSceneAsync(int sceneID)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID);

            loadingScreen.SetActive(true);

            while (!operation.isDone)
            {
                float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

                loadingBarFill.fillAmount = progressValue;

                yield return null;
            }
        }
    }
}
