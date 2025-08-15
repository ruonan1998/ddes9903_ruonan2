using UnityEngine;
using System.Collections;

public class PhoneRoomController : MonoBehaviour
{
    [Header("延迟设置")]
    public float typableMatchDelay = 4f;   // Typable 匹配成功后延迟
    public float answerButtonDelay = 3f;   // 按下按钮后的延迟

    private bool roomCompleted = false;

    /// <summary>
    /// Typable Match 成功时调用
    /// </summary>
    public void OnTypableMatchSuccess()
    {
        if (roomCompleted) return;
        roomCompleted = true;
        StartCoroutine(DelayedComplete(typableMatchDelay, true));
    }

    /// <summary>
    /// Answer 按钮按下时调用
    /// </summary>
    public void OnAnswerButtonPressed(bool success)
    {
        if (roomCompleted) return;
        roomCompleted = true;
        StartCoroutine(DelayedComplete(answerButtonDelay, success));
    }

    /// <summary>
    /// 延迟后通知 GameStateManager
    /// </summary>
    private IEnumerator DelayedComplete(float delay, bool success)
    {
        yield return new WaitForSeconds(delay);

        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.CompleteEvent("Phone Room", success);
            Debug.Log($"[PhoneRoom] Completed. Success: {success}");
        }
        else
        {
            Debug.LogError("[PhoneRoom] GameStateManager not found!");
        }
    }
}