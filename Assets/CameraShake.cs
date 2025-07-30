using UnityEngine;
using System.Collections;

public class CameraShakeTrigger : MonoBehaviour
{
    public float duration = 0.5f;       // 抖动持续时间
    public float magnitude = 0.1f;      // 抖动幅度

    private bool hasShaken = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasShaken && other.CompareTag("Player"))  // 角色必须带有“Player”标签
        {
            hasShaken = true;
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        Vector3 originalPos = Camera.main.transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            Camera.main.transform.localPosition = originalPos + new Vector3(x, y, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.localPosition = originalPos;
    }
}