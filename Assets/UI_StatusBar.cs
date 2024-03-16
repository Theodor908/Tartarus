using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tartarus
{
    public class UI_StatusBar : MonoBehaviour
    {

        private Slider slider; // The slider that represents the player's stamina

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
        }
        public virtual void SetStatus(float value)
        {
            slider.value = value;
        }

        public virtual void SetMaxStatus(float value)
        {
            slider.maxValue = value;
            slider.value = value;
        }

    }
}
