using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class EndingManager_Simplified : MonoBehaviour
{
    [Header("鬼脸相关")]
    public GameObject ghostSmile;             // 鬼从床边探出的图
    public CanvasGroup screamImage;           // 贴脸杀图

    [Header("房间整体")]
    public GameObject houseGroup;             // 房间组

    [Header("黑屏 + 音乐")]
    public CanvasGroup blackScreen;           // 黑屏 UI
    public AudioSource secondMusic;           // 背景音乐

    [Header("结尾照片")]
    public CanvasGroup familyPhoto;           // 温馨照片

    void Start()
    {
        StartCoroutine(RunEndingSequence());
    }

    IEnumerator RunEndingSequence()
    {
        yield return new WaitForSeconds(2f);
        ghostSmile.SetActive(true);
        yield return new WaitForSeconds(4f);
        ghostSmile.SetActive(false);
        Debug.Log("鬼探头出现");

        yield return new WaitForSeconds(1.5f);
        screamImage.alpha = 1f;
        screamImage.gameObject.SetActive(true);
        ForceUIFullyOpaque(screamImage);
        Debug.Log("贴脸杀图像显示");

        yield return new WaitForSeconds(1f);
        houseGroup.SetActive(false);
        yield return new WaitForSeconds(10f); // <-- 这里给玩家20秒自由时间

        blackScreen.alpha = 0f;
        blackScreen.gameObject.SetActive(true);
        ForceUIFullyOpaque(blackScreen);
        yield return null;
        yield return FadeCanvas(blackScreen, 1f);

        secondMusic.Play();
        StartCoroutine(FadeOutMusic(secondMusic, 30f));
        Debug.Log("黑屏 + 音乐播放");

        // 直接进入温馨家庭照
        familyPhoto.alpha = 0f;
        familyPhoto.gameObject.SetActive(true);
        ForceUIFullyOpaque(familyPhoto);
        yield return null;
        yield return FadeCanvas(familyPhoto, 2f);
        Debug.Log("家庭照片显示");
    }

    IEnumerator FadeCanvas(CanvasGroup group, float duration)
    {
        float t = 0f;
        float startAlpha = group.alpha;
        while (t < duration)
        {
            t += Time.deltaTime;
            group.alpha = Mathf.Lerp(startAlpha, 1f, t / duration);
            yield return null;
        }
        group.alpha = 1f;
    }

    IEnumerator FadeOutMusic(AudioSource audio, float duration)
    {
        float startVolume = audio.volume;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            audio.volume = Mathf.Clamp01(Mathf.Lerp(startVolume, 0f, t / duration));
            yield return null;
        }
        audio.volume = 0f;
        audio.Stop();
    }

    void ForceUIFullyOpaque(CanvasGroup group)
    {
        // 如果是 Image
        Image img = group.GetComponent<Image>();
        if (img != null)
        {
            Color c = img.color;
            c.a = 1f;
            img.color = c;
        }

        // 如果是 RawImage
        RawImage raw = group.GetComponent<RawImage>();
        if (raw != null)
        {
            Color c = raw.color;
            c.a = 1f;
            raw.color = c;
        }
    }
}