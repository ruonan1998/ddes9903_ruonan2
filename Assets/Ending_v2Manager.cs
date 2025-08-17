using UnityEngine;
using System.Collections;

public class Ending_v2Manager : MonoBehaviour
{
    [System.Serializable]
    public class EndingGroup
    {
        public string endingName;          // 结局名字（方便在 Inspector 里区分）
        public GameObject[] objectsToShow; // 要激活的物体
        public Animator[] animators;       // 要播放动画的 Animator
        public string[] animationTriggers; // 动画的 Trigger 名称
        public float delayBeforePlay = 0f; // 延时秒数
    }

    public EndingGroup[] endings; // 在 Inspector 里配置所有结局组

    // 播放结局的方法（给 Trigger 调用）
    public void PlayEnding(int endingIndex)
    {
        if (endingIndex < 0 || endingIndex >= endings.Length)
        {
            Debug.LogWarning("结局索引无效: " + endingIndex);
            return;
        }

        StartCoroutine(PlayEndingRoutine(endings[endingIndex]));
    }

    private IEnumerator PlayEndingRoutine(EndingGroup ending)
    {
        yield return new WaitForSeconds(ending.delayBeforePlay);

        // 激活物体
        foreach (var obj in ending.objectsToShow)
        {
            if (obj != null) obj.SetActive(true);
        }

        // 播放动画
        for (int i = 0; i < ending.animators.Length; i++)
        {
            if (ending.animators[i] != null && i < ending.animationTriggers.Length)
            {
                ending.animators[i].SetTrigger(ending.animationTriggers[i]);
            }
        }
    }
}