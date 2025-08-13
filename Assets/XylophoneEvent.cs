using UnityEngine;
using UnityEngine.UI;

public class XylophoneEvent : MonoBehaviour
{
    [Header("鬼影相关（失败时用）")]
    public GameObject ghost; 
    public Image ghostFaceUI; 
    public SpriteRenderer ghostFaceSpriteRenderer;
    public Sprite smileFace; 
    public Sprite cryFace;

    [Header("音效")]
    public AudioSource voiceSource;
    public AudioClip line1; // "Have you forgotten..."
    public AudioClip line2; // "...my sunshine?"
    public AudioClip backgroundHum; // 低频环境音
    public AudioClip successClip; // 成功音效

    [Header("灯光（失败时会暗化）")]
    public Light mainLight;
    public float darkIntensity = 0.2f;
    public float normalIntensity = 1.0f;

    [Header("其他特效")]
    public Camera mainCamera;
    public Animator ghostAnimator; 
    public float shakeAmount = 0.1f;
    public float shakeDuration = 0.2f;

    private bool eventTriggered = false;

    // ✅ 触发成功事件
    public void TriggerSuccessEvent()
    {
        if (eventTriggered) return;
        eventTriggered = true;

        Debug.Log("木琴演奏正确 - 触发成功事件");

        // 停止背景低频音
        if (voiceSource.isPlaying) voiceSource.Stop();

        // 恢复灯光
        if (mainLight != null)
            mainLight.intensity = normalIntensity;

        // 播放成功音效
        if (successClip != null)
            AudioSource.PlayClipAtPoint(successClip, transform.position);

        // 隐藏鬼影
        ghost.SetActive(false);

        // TODO: 这里可以触发后续剧情，比如 NPC 对话
        Debug.Log("可以在这里加对话系统调用，例如 DialogueManager.StartDialogue()");
    }

    // ❌ 触发失败事件
    public void TriggerFailEvent()
    {
        if (eventTriggered) return;
        eventTriggered = true;

        Debug.Log("木琴演奏错误 - 触发失败事件");

        // 灯光暗化
        if (mainLight != null)
            mainLight.intensity = darkIntensity;

        // 播放低频背景音
        if (backgroundHum != null)
            AudioSource.PlayClipAtPoint(backgroundHum, transform.position);

        // 显示鬼影并微笑
        ghost.SetActive(true);
        SetGhostFace(smileFace);

        voiceSource.clip = line1;
        voiceSource.Play();

        // 延迟哭脸
        Invoke(nameof(SwitchToCryFace), voiceSource.clip.length + 0.1f);
    }

    void SwitchToCryFace()
    {
        SetGhostFace(cryFace);
        StartCoroutine(CameraShake());

        voiceSource.clip = line2;
        voiceSource.Play();

        // 延迟回到微笑脸
        Invoke(nameof(SwitchToSmileFace), voiceSource.clip.length + 0.1f);
    }

    void SwitchToSmileFace()
    {
        SetGhostFace(smileFace);
        ghostAnimator.SetTrigger("Approach");

        Invoke(nameof(EndFailEvent), 2f);
    }

    void EndFailEvent()
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

    void SetGhostFace(Sprite newFace)
    {
        if (ghostFaceUI != null)
            ghostFaceUI.sprite = newFace;
        if (ghostFaceSpriteRenderer != null)
            ghostFaceSpriteRenderer.sprite = newFace;
    }
}
