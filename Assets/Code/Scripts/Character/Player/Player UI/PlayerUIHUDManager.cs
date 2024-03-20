using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class PlayerUIHUDManager : MonoBehaviour
    {
        [SerializeField] UI_StatusBar healthBar;
        [SerializeField] UI_StatusBar staminaBar;

        public void setNewHealthValue(float value)
        {
            healthBar.SetStatus(Mathf.RoundToInt(value));
        }

        public void setMaxHealtValue(float value)
        {
            healthBar.SetMaxStatus(value);
        }

        public void setNewStaminaValue(float value)
        {
            staminaBar.SetStatus(Mathf.RoundToInt(value));
        }

        public void setMaxStaminaValue(float value)
        {
            staminaBar.SetMaxStatus(value);
        }

    }
}
