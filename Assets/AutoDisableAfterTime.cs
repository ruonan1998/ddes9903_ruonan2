using UnityEngine;

public class AutoDisableAfterTime : MonoBehaviour
{
    public float delay = 4f;

    void OnEnable()
    {
        Invoke(nameof(DisableObject), delay);
    }

    void DisableObject()
    {
        gameObject.SetActive(false);
    }
}