using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;

    [SerializeField] private Slider slider;

    public void Load (string sceneName)
    {
        StartCoroutine(LoadAsynch(sceneName));
    }

    IEnumerator LoadAsynch (string sceneName)
    {
        AsyncOperation asop = SceneManager.LoadSceneAsync(sceneName);

        loadingScreen.SetActive(true);

        while (!asop.isDone)
        {
            float progress = Mathf.Clamp01(asop.progress / .9f);
            slider.value = progress;
            yield return null;
        }
    }
}