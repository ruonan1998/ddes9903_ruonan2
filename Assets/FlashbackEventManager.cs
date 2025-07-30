using System.Collections;
using UnityEngine;

public class FlashbackEventManager : MonoBehaviour
{
    [Header("toggleGroup")]
    public GameObject toggleGroup;         // 自动播放音效的物品组

    [Header("flashbackMusic")]
    public AudioSource flashbackMusic;
    public float returnTime = 15f;         // 多久后返回路边

    [Header("Transform player")]
    public Transform player;               // 拖进Player
    public Transform flashbackPoint;       // 回忆区域坐标（房间）
    public Transform returnPoint;          // 回到路边的坐标

    [Header("flashCanvas")]
    public CanvasGroup flashCanvas;

    public void StartFlashback()
    {
        StartCoroutine(FlashAndPlay());
    }

    private IEnumerator FlashAndPlay()
    {
        // 可选：白屏闪一下
        if (flashCanvas != null) flashCanvas.alpha = 1f;

        // 传送玩家到房间
        yield return new WaitForSeconds(0.1f);
        player.position = flashbackPoint.position;

        // 激活回忆物品 + 播放音乐
        toggleGroup.SetActive(true);
        flashbackMusic.Play();

        if (flashCanvas != null) flashCanvas.alpha = 0f;

        // 等音乐播一段时间
        yield return new WaitForSeconds(returnTime);

        // 回到原来位置
        flashbackMusic.Stop();
        toggleGroup.SetActive(false);
        player.position = returnPoint.position;
    }
}