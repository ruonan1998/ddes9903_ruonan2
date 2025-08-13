using UnityEngine;
using TMPro; // 用于 TextMeshPro

public class XylophoneSuccessEvent : MonoBehaviour
{
    [Header("UI 相关")]
    public TextMeshProUGUI dialogueText; // 直接把场景里的 TextMeshPro 对象拖进来
    public float textDisplayTime = 3f;   // 显示多久后消失

    [Header("木琴音符")]
    public KeyCode firstKey = KeyCode.A; // 第一个正确音
    public KeyCode secondKey = KeyCode.S; // 第二个正确音

    private int correctCount = 0;
    private bool puzzleCompleted = false;

    void Update()
    {
        if (puzzleCompleted) return; // 已完成就不检测

        if (Input.GetKeyDown(firstKey))
        {
            correctCount = 1; // 按对第一个音
        }
        else if (Input.GetKeyDown(secondKey) && correctCount == 1)
        {
            correctCount = 2; // 按对第二个音
            PuzzleSuccess();
        }
        else if (Input.anyKeyDown) // 按错
        {
            correctCount = 0;
        }
    }

    void PuzzleSuccess()
    {
        puzzleCompleted = true;
        ShowDialogue("My only sunshine..."); // 你想要的台词
    }

    void ShowDialogue(string message)
    {
        if (dialogueText != null)
        {
            dialogueText.text = message;
            dialogueText.gameObject.SetActive(true);
            Invoke("HideDialogue", textDisplayTime);
        }
        else
        {
            Debug.LogWarning("没有绑定 dialogueText UI 对象");
        }
    }

    void HideDialogue()
    {
        if (dialogueText != null)
        {
            dialogueText.gameObject.SetActive(false);
        }
    }
}
