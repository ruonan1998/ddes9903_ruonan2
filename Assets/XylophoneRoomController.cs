using UnityEngine;
using System.Collections;

public class XylophoneRoomController : MonoBehaviour
{
    [Header("引用")]
    public XylophonePrefabManager prefabManager;

    [Header("倒计时设置")]
    public float roomTimeLimit = 60f;  
    public float postSuccessDelay = 2f;
    public float postFailDelay = 3f;

    [Header("失败鬼影")]
    public GameObject failGhostVisual;

    private bool roomCompleted = false;
    private float timer;

    private void Start()
    {
        if (prefabManager == null)
            Debug.LogError("[XylophoneRoom] PrefabManager not assigned!");

        timer = roomTimeLimit;
        StartCoroutine(RoomTimer());
    }

    private void Update()
    {
        if (roomCompleted) return;

        // 使用公共只读属性
        if (prefabManager.PuzzleCompleted)
        {
            roomCompleted = true;
            StartCoroutine(DelayedComplete(postSuccessDelay, true));
        }
    }

    private IEnumerator RoomTimer()
    {
        while (!roomCompleted && timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        if (!roomCompleted)
        {
            roomCompleted = true;
            if (failGhostVisual != null)
                failGhostVisual.SetActive(true);

            StartCoroutine(DelayedComplete(postFailDelay, false));
        }
    }

    private IEnumerator DelayedComplete(float delay, bool success)
    {
        yield return new WaitForSeconds(delay);

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.CompleteEvent("XylophoneRoom", success);
            Debug.Log($"[XylophoneRoom] Completed. Success: {success}");
        }
        else
        {
            Debug.LogError("[XylophoneRoom] GameStateManager not found!");
        }
    }
}