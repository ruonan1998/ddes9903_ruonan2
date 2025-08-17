using UnityEngine;

public class DollChoice : MonoBehaviour
{
    public static bool pickedDoll = false;   // 是否捡了娃娃
    public static bool madeChoice = false;   // 是否已经做过选择（避免重复触发）

    // 在 “Pick Up” 按钮的 OnClick 调用
    public void PickUpDoll()
    {
        if (madeChoice) return;

        pickedDoll = true;
        madeChoice = true;

        // 拿娃娃 → 额外送 1 分（不封顶）
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.successScore += 1;
            Debug.Log("[DollChoice] Picked up doll. +1 score. Total=" + GameStateManager.Instance.successScore);
        }
        else
        {
            Debug.LogWarning("[DollChoice] GameStateManager not found.");
        }
    }

    // 在 “Leave It” 按钮的 OnClick 调用
    public void LeaveDoll()
    {
        if (madeChoice) return;

        pickedDoll = false;
        madeChoice = true;

        Debug.Log("[DollChoice] Left the doll. Will trigger trauma ending.");
        // 不在这里立刻判结局；由 EndingTrigger（卧室/出口）进入判定
    }
}