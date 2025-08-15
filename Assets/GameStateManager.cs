using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    private Dictionary<string, bool> eventResults = new Dictionary<string, bool>();
    private int forgivenessPoints = 0;

    [Header("延迟设置")]
    public float roomSuccessDelay = 2f;
    public float roomFailDelay = 4f;

    [Header("大场景玩家位置")]
    public Transform mainSceneSpawnPoint;

    private bool pickedDoll = false;
    private bool enteredFinalRoom = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void CompleteEvent(string eventName, bool success)
    {
        Debug.Log($"[GameStateManager] Event: {eventName} | Success: {success}");
        eventResults[eventName] = success;

        if (success)
            forgivenessPoints++;

        StartCoroutine(DelayedRoomTeleport(eventName, success));
    }

    private IEnumerator DelayedRoomTeleport(string completedRoom, bool success)
    {
        float delay = success ? roomSuccessDelay : roomFailDelay;
        yield return new WaitForSeconds(delay);

        string nextRoom = GetNextRoom(completedRoom);

        if (!string.IsNullOrEmpty(nextRoom))
        {
            // 使用新的推荐方法
            FlashbackSceneTeleport tele = Object.FindFirstObjectByType<FlashbackSceneTeleport>();
            if (tele != null)
            {
                tele.TriggerTeleportToScene(nextRoom);
            }
            else
            {
                Debug.LogWarning("[GameStateManager] No FlashbackSceneTeleport found!");
            }
        }
        else
        {
            SceneManager.LoadScene("MainScene");
        }
    }

    private string GetNextRoom(string completedRoom)
    {
        bool xylDone = eventResults.ContainsKey("XylophoneRoom");
        bool phoneDone = eventResults.ContainsKey("PhoneRoom");

        if (!xylDone)
            return "XylophoneRoom";
        if (!phoneDone)
            return "PhoneRoom";

        return null;
    }

    public void PickDoll() => pickedDoll = true;
    public void EnterFinalRoom() => enteredFinalRoom = true;

    public void CheckForEnding()
    {
        if (!enteredFinalRoom)
        {
            SceneManager.LoadScene("LoopEnding");
            return;
        }
        if (!pickedDoll)
        {
            SceneManager.LoadScene("TraumaEnding");
            return;
        }
        if (forgivenessPoints >= 2)
        {
            SceneManager.LoadScene("ForgivenessEnding");
            return;
        }
        SceneManager.LoadScene("DefaultEnding");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene" && mainSceneSpawnPoint != null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = mainSceneSpawnPoint.position;
                player.transform.rotation = mainSceneSpawnPoint.rotation;
            }
        }
    }
}