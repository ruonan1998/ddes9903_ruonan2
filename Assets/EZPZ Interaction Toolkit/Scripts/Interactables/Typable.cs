using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.InputSystem; // 必须引入 Input System

public class Typable : InteractableGeneral
{
    [Header("Typing Interaction Settings")]
    public UnityEvent onTextMatch;
    public UnityEvent onReleaseTyping;
    public UnityEvent onEnterKeyNotForWebGL;
    public TextMatchRelay textMatchRelay;
    public string matchText;
    public string cursorText = "_";
    public bool releaseOnEnterKey = true;

    [Header("System Stuff - Usually Don't Touch")]
    public string typeTextBuffer = "";
    public bool typeCapture = false;
    public TextMeshProUGUI textDisplay;

    public RaycastInteractor raycastInteractor;

    private void Start()
    {
        if (Keyboard.current != null)
            Keyboard.current.onTextInput += OnTextInput;
    }

    private void OnDestroy()
    {
        if (Keyboard.current != null)
            Keyboard.current.onTextInput -= OnTextInput;
    }

    public void OnMouseDown()
    {
        if (raycastInteractor != null)
            raycastInteractor.ReleaseFromTyping();
    }

    private void OnTextInput(char ch)
    {
        if (!typeCapture) return;

        if (ch == '\b' || ch == 127) // backspace
        {
            if (typeTextBuffer.Length >= 1)
                typeTextBuffer = typeTextBuffer.Substring(0, typeTextBuffer.Length - 1);
        }
        else if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            HandleEnterKey();
        }
        else if (ch == '\x1b') // Escape
        {
            if (raycastInteractor != null)
                raycastInteractor.ReleaseFromTyping();
        }
        else if (ch == '`')
        {
            onReleaseTyping.Invoke();
            if (raycastInteractor != null)
                raycastInteractor.ReleaseFromTyping();
        }
        else
        {
            typeTextBuffer += ch;
        }

        SyncText();
    }

    public void HandleEnterKey()
    {
        if (releaseOnEnterKey && raycastInteractor != null)
            raycastInteractor.ReleaseFromTyping();
        else
            typeTextBuffer += '\n';

        onEnterKeyNotForWebGL.Invoke();
    }

    public void ClearTypeBuffer()
    {
        typeTextBuffer = "";
        SyncText();
    }

    // ✅ 必须是 public，否则其他脚本找不到
    public void SyncText()
    {
        if (textDisplay == null) return;

        textDisplay.text = typeTextBuffer + cursorText;

        if (typeTextBuffer.Length > 0 && typeTextBuffer.Equals(matchText))
        {
            onTextMatch.Invoke();

            if (raycastInteractor != null)
                raycastInteractor.ReleaseFromTyping();
        }

        if (textMatchRelay != null)
            textMatchRelay.CheckMatch();
    }
}