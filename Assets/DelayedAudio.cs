using UnityEngine;

public class DelayedAudio : MonoBehaviour
{
    public AudioSource audioSource; // 拖入你的 AudioSource 组件
    public float delayTime = 3f;    // 延迟时间，默认3秒

    void Start()
    {
        StartCoroutine(PlayAudioWithDelay());
    }

    System.Collections.IEnumerator PlayAudioWithDelay()
    {
        yield return new WaitForSeconds(delayTime);
        audioSource.Play();
    }
}