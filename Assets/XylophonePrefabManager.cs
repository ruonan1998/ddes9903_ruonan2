using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Serializable 类不继承 MonoBehaviour
[System.Serializable]
public class KeyNote
{
    public GameObject keyObject;  // 对应琴键物体
    public string noteName;       // 音符字母，例如 "C"
    public AudioClip noteClip;    // 对应音效
}

public class XylophonePrefabManager : MonoBehaviour
{
    [Header("琴键设置")]
    public List<KeyNote> keys = new List<KeyNote>(); // 在 Inspector 里配置
    public List<string> targetNotes;                 // 玩家目标音符（长度=2）
    public AudioSource completionAudio;
    public Color highlightColor = Color.yellow;
    public Color errorColor = Color.red;
    public float highlightDuration = 0.2f;
    public float allowedInterval = 4f; // 两个音之间的最大间隔（秒）

    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();

    private List<string> currentSequence = new List<string>(); // 当前按下的顺序
    private bool isWaitingSecondNote = false; // 是否已经按下第一个音符
    private float timer = 0f;
    private bool puzzleCompleted = false; // 只触发一次剧情

    void Start()
    {
        // 存储原始颜色
        foreach (KeyNote kn in keys)
        {
            if (kn.keyObject == null) continue;
            Renderer rend = kn.keyObject.GetComponent<Renderer>();
            if (rend != null && !originalColors.ContainsKey(kn.keyObject))
                originalColors.Add(kn.keyObject, rend.material.color);
        }
    }

    void Update()
    {
        if (isWaitingSecondNote)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                StartCoroutine(ShowErrorAndReset());
            }
        }

        if (Input.GetMouseButtonDown(0) && !puzzleCompleted)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                foreach (KeyNote kn in keys)
                {
                    if (kn.keyObject != null && hit.collider.gameObject == kn.keyObject)
                    {
                        StartCoroutine(HandleKeyClick(kn));
                        break;
                    }
                }
            }
        }
    }

    IEnumerator HandleKeyClick(KeyNote kn)
    {
        Renderer rend = kn.keyObject.GetComponent<Renderer>();

        // 播放音效
        if (kn.noteClip != null)
            AudioSource.PlayClipAtPoint(kn.noteClip, Camera.main.transform.position);

        // 高亮显示
        if (rend != null)
            rend.material.color = highlightColor;

        yield return new WaitForSeconds(highlightDuration);

        if (rend != null)
            rend.material.color = originalColors[kn.keyObject];

        // 处理按键逻辑
        if (!isWaitingSecondNote)
        {
            // 第一次按
            if (kn.noteName == targetNotes[0])
            {
                currentSequence.Clear();
                currentSequence.Add(kn.noteName);
                isWaitingSecondNote = true;
                timer = allowedInterval; // 开始计时
            }
            else
            {
                // 按错就忽略
            }
        }
        else
        {
            // 等待第二个按键
            if (kn.noteName == targetNotes[1] && currentSequence.Count == 1)
            {
                currentSequence.Add(kn.noteName);
                if (CheckPuzzleComplete())
                    OnPuzzleComplete();
            }
            else
            {
                // 第二次按错，直接重置
                StartCoroutine(ShowErrorAndReset());
            }
        }
    }

    bool CheckPuzzleComplete()
    {
        if (currentSequence.Count == targetNotes.Count)
            return true;
        return false;
    }

    IEnumerator ShowErrorAndReset()
    {
        isWaitingSecondNote = false;
        currentSequence.Clear();
        timer = 0f;

        // 闪两次红光
        if (targetNotes.Count > 0)
        {
            KeyNote firstNote = keys.Find(k => k.noteName == targetNotes[0]);
            if (firstNote != null && firstNote.keyObject != null)
            {
                Renderer rend = firstNote.keyObject.GetComponent<Renderer>();
                if (rend != null)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        rend.material.color = errorColor;
                        yield return new WaitForSeconds(0.3f);
                        rend.material.color = originalColors[firstNote.keyObject];
                        yield return new WaitForSeconds(0.3f);
                    }
                }
            }
        }
    }

    void OnPuzzleComplete()
    {
        isWaitingSecondNote = false;
        timer = 0f;
        puzzleCompleted = true; // 标记完成，只触发一次

        if (completionAudio != null)
            completionAudio.Play();

        Debug.Log("🎉 木琴谜题完成！（只触发一次）");
    }
}