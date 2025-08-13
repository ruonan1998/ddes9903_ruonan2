using UnityEngine;

public class DollEvent : MonoBehaviour
{
    public int pickPoints = 1;
    private bool done = false;

    public void PickUpDoll()
    {
        if (done) return; done = true;
        GameStateManager.Instance.AddForgivenessPoints(pickPoints);
        Debug.Log("[Doll] Picked. +" + pickPoints);
    }

    public void IgnoreDoll()
    {
        if (done) return; done = true;
        Debug.Log("[Doll] Ignored.");
    }
}