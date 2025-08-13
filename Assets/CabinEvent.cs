using UnityEngine;
using UnityEngine.SceneManagement;

public class CabinEvent : MonoBehaviour
{
    public int stayPoints = 1;
    public string nextIfStay = "CabinBedroomScene";
    public string nextIfLeave = "CabinExitScene";
    private bool done = false;

    public void StayInBedroom()
    {
        if (done) return; done = true;
        GameStateManager.Instance.AddForgivenessPoints(stayPoints);
        SceneManager.LoadScene(nextIfStay);
    }

    public void LeaveCabin()
    {
        if (done) return; done = true;
        SceneManager.LoadScene(nextIfLeave);
    }
}