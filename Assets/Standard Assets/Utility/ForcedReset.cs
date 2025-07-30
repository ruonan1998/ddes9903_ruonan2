using System;
using UnityEngine;
using UnityEngine.SceneManagement; // 新的加载场景方法
using UnityStandardAssets.CrossPlatformInput;

public class ForcedReset : MonoBehaviour
{
    private void Update()
    {
        // 如果按下了“ResetObject”按钮
        if (CrossPlatformInputManager.GetButtonDown("ResetObject"))
        {
            // 重新加载当前场景
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        }
    }
}