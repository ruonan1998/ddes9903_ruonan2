using UnityEngine;
using System.Collections;

public class TriggerToStartEnding : MonoBehaviour
{
    public GameObject endingController;
    public float delay = 1f;

    private bool triggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(ActivateAfterDelay());
        }
    }

    IEnumerator ActivateAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        endingController.SetActive(true);
    }
}