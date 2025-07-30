using UnityEngine;

public class TriggerToFlashback : MonoBehaviour
{
    public FlashbackEventManager manager;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggered)
        {
            triggered = true;
            manager.StartFlashback();
        }
    }
}