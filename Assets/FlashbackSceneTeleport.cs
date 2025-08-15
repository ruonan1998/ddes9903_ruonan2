using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FlashbackSceneTeleport : MonoBehaviour
{
    [Header("黑屏 Canvas")]
    public Canvas fadeCanvas;
    public float fadeDuration = 0.5f;

    public void TriggerTeleportToScene(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        if(fadeCanvas != null)
        {
            fadeCanvas.gameObject.SetActive(true);
            CanvasGroup canvasGroup = fadeCanvas.GetComponent<CanvasGroup>();
            if(canvasGroup == null)
            {
                canvasGroup = fadeCanvas.gameObject.AddComponent<CanvasGroup>();
            }

            float timer = 0f;
            while(timer < fadeDuration)
            {
                timer += Time.deltaTime;
                canvasGroup.alpha = timer / fadeDuration;
                yield return null;
            }
            canvasGroup.alpha = 1f;
        }

        yield return new WaitForSeconds(0.2f); // 黑屏稳定

        if(!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}