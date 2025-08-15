using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeScreen : MonoBehaviour
{
    public static FadeScreen Instance;

    public Image blackImage; // 拖入 Canvas 上全屏黑色 Image
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (blackImage != null)
        {
            blackImage.gameObject.SetActive(true);
            blackImage.color = new Color(0, 0, 0, 0);
        }
    }

    public IEnumerator FadeOutIn(string sceneName, Vector3? spawnPosition = null)
    {
        // Fade out
        if (blackImage != null)
            yield return StartCoroutine(Fade(0f, 1f));

        // 切换场景
        SceneManager.LoadScene(sceneName);

        // 等待一帧
        yield return null;

        // 移动玩家到 spawn
        if (spawnPosition.HasValue)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                player.transform.position = spawnPosition.Value;
        }

        // Fade in
        if (blackImage != null)
            yield return StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
            if (blackImage != null)
                blackImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        if (blackImage != null)
            blackImage.color = new Color(0, 0, 0, endAlpha);
    }
}