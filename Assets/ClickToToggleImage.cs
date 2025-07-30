using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickToTogglePopup : MonoBehaviour
{
    [SerializeField] private GameObject popupGroup;    // 黑色背景+图的整体
    [SerializeField] private Image popupImage;         // 要显示的大图

    private Coroutine autoCloseCoroutine;

    private void OnMouseDown()
    {
        if (popupGroup != null)
        {
            popupGroup.SetActive(true);

            // 如果之前启动过计时器，先停掉
            if (autoCloseCoroutine != null)
            {
                StopCoroutine(autoCloseCoroutine);
            }

            // 开始自动关闭倒计时
            autoCloseCoroutine = StartCoroutine(AutoCloseAfterSeconds(3f));
        }
    }

    private IEnumerator AutoCloseAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        popupGroup.SetActive(false);
        autoCloseCoroutine = null;
    }
}