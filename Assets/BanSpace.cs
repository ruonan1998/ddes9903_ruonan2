using UnityEngine;

public class KillJumpInput : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 禁止其他脚本响应跳跃
            // 可选：你可以在这里设置一个全局变量标志跳跃已被禁用
        }

        // 阻止默认跳跃按键（旧输入系统）
        Input.ResetInputAxes();
    }
}