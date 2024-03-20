using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class CharacterStatsManager : MonoBehaviour
    {

        CharacterManager characterManager;

        [Header("Stats regeneration")]
        private float staminaRegenTimer = 0f;
        private float staminaTickTimer = 0f;
        [SerializeField] float staminaRegenDelay = 3f;
        [SerializeField] float staminaRegenRate = 1f;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {

            float stamina = 100;
            stamina += endurance * 10;

            return Mathf.RoundToInt(stamina);

        }

        public virtual void RegenerateStamina()
        {
            if(characterManager.isSprinting || characterManager.isInteracting || characterManager.isJumping)
            {
                ResetStaminaRegenTimer();
                return;
            }

            staminaRegenTimer += Time.deltaTime;

            if (staminaRegenTimer > staminaRegenDelay)
            {
                if (characterManager.currentStamina < PlayerStatsManager.instance.CalculateStaminaBasedOnEnduranceLevel(characterManager.endurance))
                {
                    staminaTickTimer += Time.deltaTime;
                    if (staminaTickTimer > 0.1f)
                    {
                        staminaTickTimer = 0f;
                        characterManager.currentStamina += staminaRegenRate;
                        PlayerUIManager.instance.playerUIHudManager.setNewStaminaValue(characterManager.currentStamina);
                    }
                }

            }

        }

        public virtual void ResetStaminaRegenTimer()
        {
            staminaRegenTimer = 0f;
        }

    }

}
