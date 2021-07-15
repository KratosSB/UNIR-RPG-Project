using System;

namespace RPG.Attributes
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health healthComponent = null;
        [SerializeField] private RectTransform foreground = null;
        [SerializeField] private Canvas rootCanvas = null;
        [SerializeField] private float secondsToFadeHealthBar = 3f;

        private Vector3 foregroundScale;

        private void Start()
        {
            foregroundScale = foreground.localScale;
            rootCanvas.enabled = false;
        }

        public void HandleHealthChanged()
        {
            //Todo change this and move to UnityEvent Text Damage
            if (Mathf.Approximately(healthComponent.GetFraction(),0f)
            ||  Mathf.Approximately(healthComponent.GetFraction(),1f))
            {
                rootCanvas.enabled = false;
                return;
            }

            rootCanvas.enabled = true;
            StartCoroutine(FadeOutHealthBar(secondsToFadeHealthBar));
            foreground.localScale = new Vector3(healthComponent.GetFraction(), foregroundScale.y, foregroundScale.z);
        }

        private IEnumerator FadeOutHealthBar(float secondsToFade)
        {
            yield return new WaitForSeconds(secondsToFade);
            rootCanvas.enabled = false;
        }
    }
}