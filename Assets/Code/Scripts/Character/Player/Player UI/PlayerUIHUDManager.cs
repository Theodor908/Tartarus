using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class PlayerUIHUDManager : MonoBehaviour
    {
        [SerializeField] UI_StatusBar healthBar;
        [SerializeField] UI_StatusBar staminaBar;

        public void RefreshHud()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);

            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
        }

        public void setNewHealthValue(float value)
        {
            healthBar.SetStatus(Mathf.RoundToInt(value));
        }

        public void setMaxHealthValue(float value)
        {
            healthBar.SetMaxStatus(value);
            healthBar.SetStatus(Mathf.RoundToInt(value));
        }

        public void setNewStaminaValue(float value)
        {
            staminaBar.SetStatus(Mathf.RoundToInt(value));
        }

        public void setMaxStaminaValue(float value)
        {
            staminaBar.SetMaxStatus(value);
            staminaBar.SetStatus(Mathf.RoundToInt(value));
        }

    }
}
