using UnityEngine;
using System.Collections;

public class DelayActivateAndHide : MonoBehaviour
{
    public GameObject targetObject; // 挂动画的物体
    public float activateDelay = 6f; // 多少秒后激活
    public float hideDelayAfterAnim = 3f; // 播放完动画后再等几秒隐藏

    public void OnButtonClick()
    {
        StartCoroutine(ActivateAndHide());
    }

    IEnumerator ActivateAndHide()
    {
        // 等待激活时间
        yield return new WaitForSeconds(activateDelay);

        // 激活物体（动画会因为 Play On Awake 自动播放）
        targetObject.SetActive(true);

        // 等待动画播放完成
        Animator anim = targetObject.GetComponent<Animator>();
        if (anim != null)
        {
            // 获取当前动画片段的时长
            float animLength = anim.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animLength);
        }

        // 动画结束后再等额外的时间
        yield return new WaitForSeconds(hideDelayAfterAnim);

        // 隐藏物体
        targetObject.SetActive(false);
    }
}