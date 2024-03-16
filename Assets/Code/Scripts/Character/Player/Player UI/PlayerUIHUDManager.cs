using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class PlayerUIHUDManager : MonoBehaviour
    {
        [SerializeField] UI_StatusBar staminaBar;

        public void setNewStaminaValue(float value)
        {
            staminaBar.SetStatus(value);
        }

        public void setMaxStaminaValue(float value)
        {
            staminaBar.SetMaxStatus(value);
        }

    }
}
