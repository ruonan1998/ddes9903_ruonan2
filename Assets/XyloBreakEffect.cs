using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoXyloBreak : MonoBehaviour
{
    public GameObject normalXylo;
    public GameObject brokenXylo;
    public CanvasGroup glitchCanvas;
    public float delay = 3f; // 延迟几秒触发

    void Start()
    {
        StartCoroutine(AutoTrigger());
    }

    IEnumerator AutoTrigger()
    {
        glitchCanvas.alpha = 0;
        glitchCanvas.gameObject.SetActive(true);

        yield return new WaitForSeconds(delay);

        // 替换木琴
        normalXylo.SetActive(false);
        brokenXylo.SetActive(true);

        // 黑白闪烁效果
        for (int i = 0; i < 6; i++)
        {
            glitchCanvas.alpha = 1;
            yield return new WaitForSeconds(0.05f);
            glitchCanvas.alpha = 0;
            yield return new WaitForSeconds(0.05f);
        }

        glitchCanvas.gameObject.SetActive(false);
    }
}