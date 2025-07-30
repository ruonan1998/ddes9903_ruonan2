using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WarningScreenController : MonoBehaviour
{
    public CanvasGroup warningCanvas;  // 挂着图像的 CanvasGroup
    public float displayTime = 5f;     // 保持全显时间
    public float fadeDuration = 1f;    // 淡出时间

    void Start()
    {
        if (warningCanvas != null)
        {
            warningCanvas.alpha = 1f;
            warningCanvas.gameObject.SetActive(true);
            StartCoroutine(FadeOutAfterDelay());
        }
    }

    IEnumerator FadeOutAfterDelay()
    {
        warningCanvas.alpha = 1f;
        yield return new WaitForSeconds(displayTime);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            warningCanvas.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        warningCanvas.alpha = 0f;
        warningCanvas.gameObject.SetActive(false);
    }
}