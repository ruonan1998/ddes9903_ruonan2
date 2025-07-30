using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowPhotoOnMusicTime : MonoBehaviour
{
    public AudioSource audioSource;
    public CanvasGroup photoCanvasGroup;
    public float triggerTime = 12f;

    private bool triggered = false;

    void Start()
    {
        photoCanvasGroup.alpha = 0;
        photoCanvasGroup.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!triggered && audioSource.isPlaying && audioSource.time >= triggerTime)
        {
            triggered = true;
            photoCanvasGroup.gameObject.SetActive(true);
            StartCoroutine(FadeInPhoto());
        }
    }

    IEnumerator FadeInPhoto()
    {
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            photoCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / duration);
            yield return null;
        }

        photoCanvasGroup.alpha = 1;

        // ✅ 停留 17 秒
        yield return new WaitForSeconds(15f);
        StartCoroutine(FadeOutPhoto());
    }

    IEnumerator FadeOutPhoto()
    {
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            photoCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / duration);
            yield return null;
        }

        photoCanvasGroup.alpha = 0;
        photoCanvasGroup.gameObject.SetActive(false);
    }
}