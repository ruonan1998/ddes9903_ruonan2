using UnityEngine;

public class FootstepController : MonoBehaviour
{
    public AudioSource footstepAudio;
    public float moveThreshold = 0.1f;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 检测是否在移动
        if (controller.velocity.magnitude > moveThreshold)
        {
            // 如果没在播放脚步声，则播放
            if (!footstepAudio.isPlaying)
            {
                footstepAudio.Play();
            }
        }
        else
        {
            // 不在移动，停止播放
            if (footstepAudio.isPlaying)
            {
                footstepAudio.Stop();
            }
        }
    }
}