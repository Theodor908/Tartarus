using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tartarus
{
    public class UI_StatusBar : MonoBehaviour
    {

        private Slider slider;
        private RectTransform rt;

        [Header("Bar options")]
        [SerializeField] private bool scaleBarLengthWithStatus = true;
        [SerializeField] protected float widthScaleMultiplier = 1;
        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
            rt = GetComponent<RectTransform>();
        }
        public virtual void SetStatus(float value)
        {
            slider.value = value;
        }

        public virtual void SetMaxStatus(float value)
        {
            slider.maxValue = value;
            slider.value = value;

            // Scale based on value
            if (scaleBarLengthWithStatus)
            {
                rt.sizeDelta = new Vector2(value * widthScaleMultiplier, rt.sizeDelta.y);
            }

            PlayerUIManager.instance.playerUIHudManager.RefreshHud();

        }

    }
}
