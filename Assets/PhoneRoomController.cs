using System.Collections;
using UnityEngine;

public class PhoneRoomController : MonoBehaviour
{
    [Header("延迟跳转设置（单位：秒）")]
    public float successDelay = 4f; // TypableMatch 成功或 Answer=成功
    public float failDelay    = 3f; // Answer=失败（如果你会传 false）

    private bool finished = false;

    // TypableMatch 成功时调用
    public void OnTypableMatchSuccess()
    {
        if (finished) return;
        finished = true;

        GameStateManager.Instance?.NotifyRoomFinished(
            GameStateManager.Instance.phoneScene,
            success: true,
            delaySeconds: successDelay
        );
    }

    // Answer 按钮按下时调用；由你传入 success=true/false
    public void OnAnswerButtonPressed(bool success)
    {
        if (finished) return;
        finished = true;

        float delay = success ? successDelay : failDelay;

        GameStateManager.Instance?.NotifyRoomFinished(
            GameStateManager.Instance.phoneScene,
            success: success,
            delaySeconds: delay
        );
    }
}