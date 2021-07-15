using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private Coroutine currentActiveFade = null;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private IEnumerator FadeOutIn()
        {
            yield return FadeOut(3f);
            Debug.Log("Faded out!");
            yield return FadeIn(3f);
            Debug.Log("Faded in!");
        }

        public void FadeOutInmediate()
        {
            _canvasGroup.alpha = 1;
        }
        
        public Coroutine FadeOut(float time)
        {
            return Fade(1, time);
        }

        public Coroutine FadeIn(float time)
        {
            return Fade(0, time);
        }

        //Controls the activation of the fade in / fade out effect.
        public Coroutine Fade(float target, float time)
        {
            if (currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
            }
            currentActiveFade = StartCoroutine(FadeCoroutine(target, time));
            return currentActiveFade;
        }
        
        //Fading effect using the transparency alpha channel of a plane in front of the player's camera
        private IEnumerator FadeCoroutine(float target, float time)
        {
            while (!Mathf.Approximately(_canvasGroup.alpha, target))
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }
        }
    }
}