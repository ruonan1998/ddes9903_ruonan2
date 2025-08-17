using UnityEngine;

public class Ending_v2Trigger : MonoBehaviour
{
    public Ending_v2Manager endingManager; // 拖拽 Manager
    public int endingIndex;                // 设置要触发的结局索引（0,1,2...）

    public void TriggerEnding()
    {
        if (endingManager != null)
        {
            endingManager.PlayEnding(endingIndex);
        }
        else
        {
            Debug.LogWarning("未绑定 Ending_v2Manager");
        }
    }
}