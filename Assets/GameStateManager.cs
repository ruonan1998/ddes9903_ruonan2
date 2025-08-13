using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    [Header("结局分数")]
    public int forgivenessPoints = 0;

    [Header("“木琴/电话”首个互动的解锁逻辑")]
    public bool firstSpecialInteractionDone = false;
    public bool firstSpecialInteractionSuccess = false;
    public bool secondRoomUnlocked = false;   // 首次成功后才解锁
    public string secondRoomSceneName = "";   // 首次成功后要出现的“另一个房间”名

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddForgivenessPoints(int amount)
    {
        forgivenessPoints += amount;
        Debug.Log("[GSM] ForgivenessPoints = " + forgivenessPoints);
    }

    // 只在“电话/木琴”这两个房间的第一次互动时调用
    public void RegisterFirstRoomResult(string currentRoomScene, bool success, string otherRoomScene)
    {
        if (firstSpecialInteractionDone) return;

        firstSpecialInteractionDone = true;
        firstSpecialInteractionSuccess = success;

        if (success)
        {
            secondRoomUnlocked = true;
            secondRoomSceneName = otherRoomScene;
            Debug.Log($"[GSM] 首次互动成功，解锁房间：{secondRoomSceneName}");
        }
        else
        {
            secondRoomUnlocked = false;
            secondRoomSceneName = "";
            Debug.Log("[GSM] 首次互动失败，另一个房间不会出现。");
        }
    }

    public bool CanEnterSecondRoom(string targetScene)
    {
        return secondRoomUnlocked && !string.IsNullOrEmpty(secondRoomSceneName)
               && secondRoomSceneName == targetScene;
    }
}