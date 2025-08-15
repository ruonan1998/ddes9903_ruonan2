using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class SceneTriggerTeleport : MonoBehaviour
{
    [Header("目标场景")]
    public string targetScene; // 要切换到的场景名字

    [Header("黑屏设置")]
    public Canvas fadeCanvas;     // 用于黑屏的 Canvas
    public Image fadeImage;       // 黑屏 Image
    public float fadeDuration = 0.5f; // 黑屏淡入淡出时间
    public float delayBeforeLoad = 0.5f; // 黑屏后延迟加载场景

    private bool triggered = false;

    private void Start()
    {
        if(fadeCanvas != null)
        {
            fadeCanvas.gameObject.SetActive(false); // 初始隐藏
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return; // 防止重复触发
        if (!other.CompareTag("Player")) return;

        triggered = true;
        StartCoroutine(FadeAndLoadScene());
    }

    private IEnumerator FadeAndLoadScene()
    {
        if(fadeCanvas != null && fadeImage != null)
        {
            fadeCanvas.gameObject.SetActive(true);
            // 黑屏淡入
            float timer = 0f;
            while(timer < fadeDuration)
            {
                timer += Time.deltaTime;
                fadeImage.color = new Color(0, 0, 0, timer / fadeDuration);
                yield return null;
            }
            fadeImage.color = Color.black;
        }

        // 等待一小段时间后加载场景
        yield return new WaitForSeconds(delayBeforeLoad);

        if(!string.IsNullOrEmpty(targetScene))
        {
            SceneManager.LoadScene(targetScene);
        }
    }
}