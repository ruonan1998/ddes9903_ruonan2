using System;
using UnityEngine;
using UnityEngine.UI; // ✅ 使用 Unity 的 UI 系统

namespace UnityStandardAssets.Utility
{
    public class SimpleActivatorMenu : MonoBehaviour
    {
        // 一个非常简单的菜单脚本，可以切换激活的对象
        public Text camSwitchButton; // ✅ 改为 UI.Text 类型
        public GameObject[] objects;

        private int m_CurrentActiveObject;

        private void OnEnable()
        {
            // 初始激活第一个对象
            m_CurrentActiveObject = 0;

            if (camSwitchButton != null && objects.Length > 0)
            {
                camSwitchButton.text = objects[m_CurrentActiveObject].name;
            }
        }

        public void NextCamera()
        {
            int nextactiveobject = m_CurrentActiveObject + 1 >= objects.Length ? 0 : m_CurrentActiveObject + 1;

            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(i == nextactiveobject);
            }

            m_CurrentActiveObject = nextactiveobject;

            if (camSwitchButton != null)
            {
                camSwitchButton.text = objects[m_CurrentActiveObject].name;
            }
        }
    }
}