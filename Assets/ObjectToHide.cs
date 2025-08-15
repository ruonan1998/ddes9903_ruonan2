using UnityEngine;

public class HideOnAudioStart : MonoBehaviour
{
    public AudioSource audioSource;       // 音乐所在的 AudioSource
    public GameObject[] objectsToHide;    // 要隐藏的物体列表
    public GameObject[] objectsToToggle;  // 要切换激活状态的物体列表
    private bool hasStarted = false;      // 确保只执行一次

    void Update()
    {
        if (!hasStarted && audioSource != null && audioSource.isPlaying)
        {
            // 隐藏列表里的物体
            foreach (GameObject obj in objectsToHide)
            {
                if (obj != null)
                    obj.SetActive(false);
            }

            // 切换 toggle 列表里的每个物体激活状态
            foreach (GameObject obj in objectsToToggle)
            {
                if (obj != null)
                    obj.SetActive(!obj.activeSelf);
            }

            hasStarted = true; // 防止重复触发
        }
    }
}