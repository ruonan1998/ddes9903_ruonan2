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
    public string bigScene       = "Main Scene"; // 你的大场景名

    [Header("大场景里的固定落点物体名")]
    public string bigSceneSpawnName = "SpawnPoint"; // 大场景里放一个同名空物体

    [Header("分值（只统计房间交互成功；娃娃额外+1 可超出2）")]
    [Tooltip("房间成功一次 +1（最大两次=2），娃娃可再+1，总分阈值由你在结局里判定")]
    public int successScore = 0;    // 初始 0；房间成功各+1（封顶2），娃娃按钮可再+1

    // 房间完成记录：null=未完成, true=成功, false=失败
    private Dictionary<string, bool?> roomResult = new Dictionary<string, bool?>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            roomResult[xylophoneScene] = null;
            roomResult[phoneScene]     = null;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 由房间控制器在交互结束时调用（成功才+1分）。
    /// </summary>
    public void NotifyRoomFinished(string roomName, bool success, float delaySeconds)
    {
        roomResult[roomName] = success;

        // 房间成功才 +1；此处仅把房间得分封顶到2（娃娃不受此限制）
        if (success)
        {
            int roomSuccesses = 0;
            if (roomResult.ContainsKey(xylophoneScene) && roomResult[xylophoneScene] == true) roomSuccesses++;
            if (roomResult.ContainsKey(phoneScene)     && roomResult[phoneScene]     == true) roomSuccesses++;

            // 先把 successScore 去掉房间部分的封顶束缚，随后再校正。
            // 简化：如果已有房间成功次数 > 当前已计入的房间分，就同步到房间成功次数
            int dollBonus = Mathf.Max(0, successScore - 2); // 假定2是房间成功的上限
            successScore = Mathf.Min(roomSuccesses, 2) + dollBonus;
        }

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

    // —— 供结局读取 —— //
    public int GetScore() => successScore;
    public bool? GetRoomResult(string roomSceneName)
        => roomResult.ContainsKey(roomSceneName) ? roomResult[roomSceneName] : null;

    // 测试重置
    public void ResetProgress()
    {
        roomResult[xylophoneScene] = null;
        roomResult[phoneScene]     = null;
        successScore = 0;
    }
}