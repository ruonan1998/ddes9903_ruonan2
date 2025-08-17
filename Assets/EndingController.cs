using UnityEngine;

public class EndingController : MonoBehaviour
{
    public static EndingController Instance;

    [Header("是否在判定时打印分支到Console")]
    public bool debugLog = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 如果你希望跨场景存在
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 统一判定结局。
    /// forceLoop=true 来自大门触发（逃离）→ 循环结局。
    /// 否则：没捡娃娃=创伤；分值>=2=原谅；否则循环。
    /// </summary>
    public void DecideEnding(bool forceLoop = false)
    {
        if (forceLoop)
        {
            if (debugLog) Debug.Log("[Ending] 循环结局（从大门离开）");
            // TODO: 这里放你“进入循环结局”的后续（比如开剧情、标记、或Load结局场景）
            return;
        }

        int score = GameStateManager.Instance != null ? GameStateManager.Instance.GetScore() : 0;
        bool made = DollChoice.madeChoice;
        bool picked = DollChoice.pickedDoll;

        // 1) 没捡娃娃 → 创伤
        if (made && !picked)
        {
            if (debugLog) Debug.Log("[Ending] 创伤结局（没有拿娃娃）");
            // TODO: 进入创伤结局
            return;
        }

        // 2) 分值>=2 → 原谅
        if (score >= 2)
        {
            if (debugLog) Debug.Log("[Ending] 原谅结局（分值达成）");
            // TODO: 进入原谅结局
            return;
        }

        // 3) 其他 → 循环
        if (debugLog) Debug.Log("[Ending] 循环结局（分值不足）");
        // TODO: 进入循环结局
    }
}