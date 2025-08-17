using System.Collections;
using UnityEngine;

public class XylophoneRoomController : MonoBehaviour
{
    [Header("成功判定（二选一，填其一即可）")]
    public XylophonePrefabManager prefabManager; // 若用木琴谜题完成判定，请拖上来
    public GameObject goodGhostToggle;           // 若用“好鬼出现”判定，请拖上来

    [Header("倒计时/延迟（秒）")]
    public float roomTimeLimit   = 40f; // 超时=失败
    public float successDelay    = 2f;  // 成功后延迟跳转
    public float failDelay       = 4f;  // 失败后延迟跳转

    private bool finished = false;
    private float timer;

    void Start()
    {
        timer = roomTimeLimit;
        StartCoroutine(RoomTimer());
    }

    void Update()
    {
        if (finished) return;

        // 方式一：木琴谜题完成
        if (prefabManager != null && prefabManager.PuzzleCompleted)
        {
            OnPuzzleSuccess();
            return;
        }

        // 方式二：好鬼出现即视为成功（从未激活->激活）
        // 仅当你用这个方式时才检查
        if (prefabManager == null && goodGhostToggle != null && goodGhostToggle.activeSelf)
        {
            OnPuzzleSuccess();
            return;
        }
    }

    private void OnPuzzleSuccess()
    {
        if (finished) return;
        finished = true;

        GameStateManager.Instance?.NotifyRoomFinished(
            GameStateManager.Instance.xylophoneScene,
            success: true,               // 成功才 +1 分
            delaySeconds: successDelay
        );
        Debug.Log("[Xylophone] Success.");
    }

    private IEnumerator RoomTimer()
    {
        while (!finished && timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        if (!finished)
        {
            // 超时=失败（不加分），照样推进流程
            finished = true;

            GameStateManager.Instance?.NotifyRoomFinished(
                GameStateManager.Instance.xylophoneScene,
                success: false,
                delaySeconds: failDelay
            );
            Debug.Log("[Xylophone] Timeout -> Fail.");
        }
    }
}