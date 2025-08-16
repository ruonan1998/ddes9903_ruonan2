using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    [Header("Scene Names (必须与 Build Settings 完全一致)")]
    public string xylophoneScene = "Xylophone Room";
    public string phoneScene     = "Phone Room";
    public string bigScene       = "Main Scene"; // 你的大场景名，Inspector 里改

    [Header("大场景里的固定落点物体名")]
    public string bigSceneSpawnName = "SpawnPoint"; // 大场景里放一个同名空物体

    [Header("分值（只统计房间交互成功）")]
    [Tooltip("成功一次 +1，最大 2 分（两个房间）")]
    public int successScore = 0;    // 0~2

    // 房间完成记录：null=未完成, true=成功, false=失败
    private Dictionary<string, bool?> roomResult = new Dictionary<string, bool?>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            roomResult[xylophoneScene] = null;
            roomResult[phoneScene] = null;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 由房间控制器在交互结束时调用。
    /// </summary>
    /// <param name="roomName">当前房间场景名</param>
    /// <param name="success">是否成功（成功才+1分）</param>
    /// <param name="delaySeconds">延迟多少秒再跳转</param>
    public void NotifyRoomFinished(string roomName, bool success, float delaySeconds)
    {
        // 只记录一次
        if (roomResult.ContainsKey(roomName) && roomResult[roomName].HasValue == false)
        {
            // 已有 false，说明之前记录过失败；但是为了保险不重复记录，直接返回
            // 不过一般不会走到这
        }

        roomResult[roomName] = success;

        if (success)
            successScore = Mathf.Clamp(successScore + 1, 0, 2); // 只来自房间，最大 2

        Debug.Log($"[GSM] Room '{roomName}' finished. success={success}, score={successScore}");

        // 判断是否两间都体验过
        bool xDone = roomResult[xylophoneScene].HasValue;
        bool pDone = roomResult[phoneScene].HasValue;

        if (!xDone || !pDone)
        {
            // 还有另外一个房间没做 → 跳去另一个房间
            string next = (roomName == xylophoneScene) ? phoneScene : xylophoneScene;
            StartCoroutine(LoadSceneAfterDelay(next, delaySeconds));
        }
        else
        {
            // 两个房间都完成 → 回大场景并落在固定点
            StartCoroutine(LoadBigSceneAfterDelay(delaySeconds));
        }
    }

    private IEnumerator LoadSceneAfterDelay(string sceneName, float delay)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator LoadBigSceneAfterDelay(float delay)
    {
        if (delay > 0f) yield return new WaitForSeconds(delay);

        // 订阅回调以便在大场景里把玩家放到固定点
        SceneManager.sceneLoaded += OnBigSceneLoaded;
        SceneManager.LoadScene(bigScene);
    }

    private void OnBigSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnBigSceneLoaded;

        // 定位玩家与落点
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject spawn  = GameObject.Find(bigSceneSpawnName);

        if (player != null && spawn != null)
        {
            player.transform.SetPositionAndRotation(spawn.transform.position, spawn.transform.rotation);
            Debug.Log($"[GSM] Teleported player to '{bigSceneSpawnName}' in big scene.");
        }
        else
        {
            if (player == null) Debug.LogWarning("[GSM] Player with tag 'Player' not found in big scene.");
            if (spawn  == null) Debug.LogWarning($"[GSM] Spawn object '{bigSceneSpawnName}' not found in big scene.");
        }
    }

    // —— 供以后结局判定脚本读取 —— //
    public int GetScore() => successScore;
    public bool? GetRoomResult(string roomSceneName)
    {
        return roomResult.ContainsKey(roomSceneName) ? roomResult[roomSceneName] : null;
    }

    // (可选) 测试时重置
    public void ResetProgress()
    {
        roomResult[xylophoneScene] = null;
        roomResult[phoneScene] = null;
        successScore = 0;
    }
}