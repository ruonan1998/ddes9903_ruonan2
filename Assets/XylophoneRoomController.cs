using UnityEngine;

public class XylophoneRoomController : MonoBehaviour
{
    [Header("判定两次同音键")]
    public KeyCode sunshineKey = KeyCode.A;

    [Header("好鬼 / 坏鬼")]
    public GameObject goodGhost;               // 初始隐藏
    public GameObject badGhost;                // 初始隐藏

    [Header("分数 & 另一个房间")]
    public int successPoints = 1;
    public int failPoints = 0;
    public string otherRoomSceneName = "Phone Room";

    private int step = 0;
    private bool locked = false;

    void Update()
    {
        if (locked) return;

        if (Input.GetKeyDown(sunshineKey))
        {
            if (step == 0) { step = 1; return; }
            if (step == 1) { OnSuccess(); return; }
        }
        else if (Input.anyKeyDown)
        {
            OnFail();
        }
    }

    void OnSuccess()
    {
        locked = true;

        GameStateManager.Instance.RegisterFirstRoomResult(
            currentRoomScene: "Xylophone Room",
            success: true,
            otherRoomScene: otherRoomSceneName
        );
        GameStateManager.Instance.AddForgivenessPoints(successPoints);

        if (goodGhost != null) goodGhost.SetActive(true);
        else Debug.LogWarning("[XylophoneRoom] goodGhost 未绑定。");
    }

    void OnFail()
    {
        locked = true;

        GameStateManager.Instance.RegisterFirstRoomResult(
            currentRoomScene: "Xylophone Room",
            success: false,
            otherRoomScene: otherRoomSceneName
        );
        if (failPoints != 0) GameStateManager.Instance.AddForgivenessPoints(failPoints);

        if (badGhost != null) badGhost.SetActive(true);
        else Debug.LogWarning("[XylophoneRoom] badGhost 未绑定。");
    }
}