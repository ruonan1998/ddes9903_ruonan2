using UnityEngine;

public class SunshinePuzzle : MonoBehaviour
{
    public KeyCode firstNoteKey = KeyCode.A;  // 第一个音符
    public KeyCode secondNoteKey = KeyCode.S; // 第二个音符
    private int noteIndex = 0;
    private bool puzzleCompleted = false;

    public void Update()
    {
        if (puzzleCompleted) return;

        if (Input.GetKeyDown(firstNoteKey) && noteIndex == 0)
        {
            noteIndex = 1; // 第一音符正确
        }
        else if (Input.GetKeyDown(secondNoteKey) && noteIndex == 1)
        {
            PuzzleSolved();
        }
        else if (Input.anyKeyDown)
        {
            noteIndex = 0; // 错误重置
        }
    }

    private void PuzzleSolved()
    {
        puzzleCompleted = true;
        Debug.Log("Sunshine Puzzle Solved!");
        // 这里可以调用鬼影或对话
    }
}