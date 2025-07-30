using UnityEngine;

public class HeadJumpScare : MonoBehaviour
{
    public Transform barbieHead;       // 芭比头的Transform
    public Transform targetPoint;      // 玩家面前的位置
    public float moveDuration = 0.3f;  // 移动时间（冲过去）
    public float stayTime = 2f;        // 停留时间

    private Vector3 originalPosition;
    private bool triggered = false;

    void Start()
    {
        originalPosition = barbieHead.position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(MoveHead());
        }
    }

    System.Collections.IEnumerator MoveHead()
    {
        Vector3 startPos = barbieHead.position;
        Vector3 endPos = targetPoint.position;

        // 移动到玩家面前
        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            barbieHead.position = Vector3.Lerp(startPos, endPos, elapsed / moveDuration);
            yield return null;
        }

        barbieHead.position = endPos;

        yield return new WaitForSeconds(stayTime);

        // 回到原位置
        elapsed = 0f;
        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            barbieHead.position = Vector3.Lerp(endPos, originalPosition, elapsed / moveDuration);
            yield return null;
        }

        barbieHead.position = originalPosition;
    }
}