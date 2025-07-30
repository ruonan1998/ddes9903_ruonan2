using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    public AudioClip lockedSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        if (lockedSound != null)
        {
            audioSource.PlayOneShot(lockedSound);
        }
    }
}