using UnityEngine;

public class XylophoneFailEvent : MonoBehaviour
{
    [Header("鬼影相关")]
    public GameObject ghost;             // 鬼影物体
    public SpriteRenderer ghostFace;     // 鬼影脸的SpriteRenderer
    public Sprite smileFace;             // 微笑脸
    public Sprite cryFace;               // 哭脸

    [Header("音效")]
    public AudioSource voiceSource;
    public AudioClip line1; // "Have you forgotten..."
    public AudioClip line2; // "...my sunshine?"
    public AudioClip backgroundHum; // 低频环境音

    [Header("灯光")]
    public Light mainLight;
    public float darkIntensity = 0.2f;
    public float normalIntensity = 1.0f;

    [Header("其他特效")]
    public Camera mainCamera;
    public Animator ghostAnimator; // 用于靠近或消失动画
    public float shakeAmount = 0.1f;
    public float shakeDuration = 0.2f;

    private bool eventTriggered = false;

    public void TriggerFailEvent()
    {
        if (eventTriggered) return;
        eventTriggered = true;

        // 灯光暗化
        if (mainLight != null)
            mainLight.intensity = darkIntensity;

        // 播放低频环境音
        if (backgroundHum != null)
            AudioSource.PlayClipAtPoint(backgroundHum, transform.position);

        // 显示鬼影
        ghost.SetActive(true);

        // 先显示微笑脸
        ghostFace.sprite = smileFace;
        voiceSource.clip = line1;
        voiceSource.Play();

        // 延迟切换哭脸
        Invoke(nameof(SwitchToCryFace), voiceSource.clip.length + 0.1f);
    }

    void SwitchToCryFace()
    {
        ghostFace.sprite = cryFace;
        StartCoroutine(CameraShake());

        voiceSource.clip = line2;
        voiceSource.Play();

        // 延迟回到微笑脸
        Invoke(nameof(SwitchToSmileFace), voiceSource.clip.length + 0.1f);
    }

    void SwitchToSmileFace()
    {
        ghostFace.sprite = smileFace;
        ghostAnimator.SetTrigger("Approach"); // 鬼影靠近动画

        // 这里可以加额外台词或结束逻辑
        Invoke(nameof(EndEvent), 2f);
    }

    void EndEvent()
    {
        ghostAnimator.SetTrigger("Disappear");
        if (mainLight != null)
            mainLight.intensity = normalIntensity;

        eventTriggered = false;
    }

    System.Collections.IEnumerator CameraShake()
    {
        Vector3 originalPos = mainCamera.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeAmount;
            float y = Random.Range(-1f, 1f) * shakeAmount;

            mainCamera.transform.localPosition = originalPos + new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalPos;
    }
}