using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public Slider progressBar;

    // 🔥 NEW: control loading speed from Inspector
    public float loadingSpeed = 1f;

    void Start()
    {
        StartCoroutine(LoadAsync());
    }

    IEnumerator LoadAsync()
    {
        string sceneToLoad = PlayerPrefs.GetString("SceneToLoad");

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        float progress = 0;

        while (!operation.isDone)
        {
            float target = Mathf.Clamp01(operation.progress / 0.9f);

            // 🔥 APPLY SPEED HERE
            progress = Mathf.MoveTowards(progress, target, Time.deltaTime * loadingSpeed);

            progressBar.value = progress;

            if (progress >= 1f)
            {
                yield return new WaitForSeconds(1f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}