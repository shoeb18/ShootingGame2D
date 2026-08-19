using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/* LevelManager: Manages scene fades and level loading.
 - Awake(): set singleton instance.
 - Start(): get CanvasGroup and fade to transparent on start.
 - RestartLevel(): start fade to black then reload current level by index.
 - LoadLevelString(string): start fade to black then load level by name.
 - FadeToTransparent(): coroutine fading UI to transparent.
 - FadeToBlackInt(int): coroutine fading UI to black then load by index.
 - FadeToBlackString(string): coroutine fading UI to black then load by name.
*/
public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private float fadeDuration;
    private CanvasGroup canvasGroup;

    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        StartCoroutine(FadeToTransparent());
    }

    public void RestartLevel()
    {
        StartCoroutine(FadeToBlackInt(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadLevelString(string levelName)
    {
        StartCoroutine(FadeToBlackString(levelName));
    }
    

    public IEnumerator FadeToTransparent()
    {
        float time = 0f;
        float startAlpha = canvasGroup.alpha;
        float endAlpha = 0f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;
    }

    public IEnumerator FadeToBlackInt(int levelIndex)
    {
        float time = 0f;
        float startAlpha = canvasGroup.alpha;
        float endAlpha = 1f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;
        SceneManager.LoadScene(levelIndex);
    }

    public IEnumerator FadeToBlackString(string levelName)
    {
        float time = 0f;
        float startAlpha = canvasGroup.alpha;
        float endAlpha = 1f;
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;
        SceneManager.LoadScene(levelName);
    }

}
