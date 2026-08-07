using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

    public void LoadNextLevel(string levelName)
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
