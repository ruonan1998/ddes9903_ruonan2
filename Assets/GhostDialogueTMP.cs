using UnityEngine;
using TMPro;
using System.Collections;

namespace MyGame.Dialogue
{
    [DisallowMultipleComponent]
    public class GhostDialogueTMP : MonoBehaviour
    {
        [Header("鬼影对象")]
        public GameObject ghost; // 鬼影物体

        [Header("台词与音频")]
        public AudioSource audioSource;
        public AudioClip[] dialogueClips; // 最多三个音频
        public string[] dialogueTexts;    // 对应的字幕
        public TMP_Text dialogueTextUI;
        public float textFadeDuration = 1f;
        public float delayBetweenLines = 1f;

        private void OnEnable()
        {
            if (ghost != null)
                ghost.SetActive(true);

            if (dialogueClips.Length > 0)
                StartCoroutine(PlayAll());
        }

        private IEnumerator PlayAll()
        {
            dialogueTextUI.text = "";
            for (int i = 0; i < dialogueClips.Length; i++)
            {
                if (i < dialogueTexts.Length)
                    yield return StartCoroutine(FadeInText(dialogueTexts[i]));

                if (dialogueClips[i] != null)
                {
                    audioSource.clip = dialogueClips[i];
                    audioSource.Play();
                    yield return new WaitForSeconds(dialogueClips[i].length);
                }

                yield return new WaitForSeconds(delayBetweenLines);

                if (i < dialogueTexts.Length)
                    yield return StartCoroutine(FadeOutText());
            }
        }

        private IEnumerator FadeInText(string text)
        {
            dialogueTextUI.text = text;
            Color c = dialogueTextUI.color;
            c.a = 0;
            dialogueTextUI.color = c;

            float t = 0;
            while (t < textFadeDuration)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(0, 1, t / textFadeDuration);
                dialogueTextUI.color = c;
                yield return null;
            }
        }

        private IEnumerator FadeOutText()
        {
            Color c = dialogueTextUI.color;
            float t = 0;
            while (t < textFadeDuration)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(1, 0, t / textFadeDuration);
                dialogueTextUI.color = c;
                yield return null;
            }
            dialogueTextUI.text = "";
        }
    }
}