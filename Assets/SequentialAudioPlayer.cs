using UnityEngine;
using System.Collections;

public class SequentialAudioPlayer : MonoBehaviour
{
    [Header("音频源（最多 3 个）")]
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource audioSource3;

    [Header("播放间隔（秒）")]
    public float intervalBetweenClips = 1f;

    public void PlayAll()
    {
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        // 播放第 1 句
        if (audioSource1 != null && audioSource1.clip != null)
        {
            audioSource1.Play();
            yield return new WaitForSeconds(audioSource1.clip.length + intervalBetweenClips);
        }

        // 播放第 2 句
        if (audioSource2 != null && audioSource2.clip != null)
        {
            audioSource2.Play();
            yield return new WaitForSeconds(audioSource2.clip.length + intervalBetweenClips);
        }

        // 播放第 3 句
        if (audioSource3 != null && audioSource3.clip != null)
        {
            audioSource3.Play();
        }
    }
}