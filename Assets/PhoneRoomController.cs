using UnityEngine;

public class PhoneRoomController : MonoBehaviour
{
    [Header("好鬼 / 坏鬼")]
    public GameObject goodGhost;               // 初始隐藏
    public GameObject badGhost;                // 初始隐藏

    [Header("分数 & 另一个房间")]
    public int successPoints = 1;
    public int failPoints = 0;
    public string otherRoomSceneName = "Xylophone Room";

    private bool locked = false;

    // 你的按钮/交互事件调用这个方法
    public void ResolvePhone(bool correct)
    {
        if (locked) return;
        locked = true;

        if (correct)
        {
            GameStateManager.Instance.RegisterFirstRoomResult(
                currentRoomScene: "Phone Room",
                success: true,
                otherRoomScene: otherRoomSceneName
            );
            GameStateManager.Instance.AddForgivenessPoints(successPoints);
            if (goodGhost != null) goodGhost.SetActive(true);
        }
        else
        {
            GameStateManager.Instance.RegisterFirstRoomResult(
                currentRoomScene: "Phone Room",
                success: false,
                otherRoomScene: otherRoomSceneName
            );
            if (failPoints != 0) GameStateManager.Instance.AddForgivenessPoints(failPoints);
            if (badGhost != null) badGhost.SetActive(true);
        }
    }
}