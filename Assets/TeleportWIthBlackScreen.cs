using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RestartWithGhost : MonoBehaviour
{
    [Header("玩家和目标")]
    public GameObject player;
    public Transform targetPoint;

    [Header("房间里的鬼和黑屏滤镜")]
    public GameObject ghost;
    public GameObject filter; // ✅ 新增滤镜对象
    public CanvasGroup blackScreen;

    [Header("初始延迟")]
    public float delayBeforeTeleport = 4f;

    [Header("黑屏淡入/淡出时间")]
    public float fadeDuration = 1f;

    [Header("传送控制")]
    public bool overrideGameStateTeleport = true; // ✅ 是否覆盖 GSM 传送

    // 缓存初始状态
    private List<Transform> allObjects = new List<Transform>();
    private Dictionary<Transform, Vector3> initialPositions = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Quaternion> initialRotations = new Dictionary<Transform, Quaternion>();
    private Dictionary<GameObject, bool> initialActiveStates = new Dictionary<GameObject, bool>();

    void Start()
    {
        CacheInitialStates();
    }

    void CacheInitialStates()
    {
        foreach (GameObject go in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            CacheObjectRecursively(go.transform);
        }
    }

    void CacheObjectRecursively(Transform t)
    {
        allObjects.Add(t);
        initialPositions[t] = t.position;
        initialRotations[t] = t.rotation;
        initialActiveStates[t.gameObject] = t.gameObject.activeSelf;

        foreach (Transform child in t)
        {
            CacheObjectRecursively(child);
        }
    }

    public void OnButtonClicked()
    {
        StartCoroutine(RestartSequence());
    }

    IEnumerator RestartSequence()
    {
        yield return new WaitForSeconds(delayBeforeTeleport);

        // 激活黑屏
        blackScreen.alpha = 0f;
        blackScreen.gameObject.SetActive(true);

        // 黑屏渐入
        float timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            blackScreen.alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeDuration);
            yield return null;
        }
        blackScreen.alpha = 1f;

        // **重置场景**
        foreach (Transform obj in allObjects)
        {
            obj.position = initialPositions[obj];
            obj.rotation = initialRotations[obj];
            obj.gameObject.SetActive(initialActiveStates[obj.gameObject]);
        }

        // 传送玩家（避免和 GameStateManager 冲突）
        if (overrideGameStateTeleport && player != null && targetPoint != null)
        {
            player.transform.position = targetPoint.position;
            player.transform.rotation = targetPoint.rotation;
        }

        // ✅ 激活鬼和滤镜
        if (ghost != null) ghost.SetActive(true);
        if (filter != null) filter.SetActive(true);

        // 黑屏淡出
        timeElapsed = 0f;
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            blackScreen.alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeDuration);
            yield return null;
        }
        blackScreen.alpha = 0f;
        blackScreen.gameObject.SetActive(false);
    }
}