using UnityEngine;

public class PlayAudioOnceOnTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    private bool hasPlayed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.CompareTag("Player")) // 注意你的角色要有 "Player" 标签
        {
            audioSource.Play();
            hasPlayed = true;
        }
    }
}