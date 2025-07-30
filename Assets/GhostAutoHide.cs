using UnityEngine;

public class GhostAutoHide : MonoBehaviour
{
    public GameObject target;

    public void HideObject()
    {
        target.SetActive(false);
    }
}