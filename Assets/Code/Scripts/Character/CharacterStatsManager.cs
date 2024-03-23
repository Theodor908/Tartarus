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

        protected virtual void Start()
        {
            characterManager.maxHealth = CalculateHealthBasedOnVitalityLevel(characterManager.vitality);
            characterManager.maxStamina = CalculateStaminaBasedOnEnduranceLevel(characterManager.endurance);
        }

        protected virtual void Update()
        {
            RegenerateStamina();
        }

        protected virtual int CalculateHealthBasedOnVitalityLevel(int vitality)
        {

            float health = 100;
            health += (vitality - 1) * 10;

            return Mathf.RoundToInt(health);

        }

        protected virtual int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {

            float stamina = 100;
            stamina += (endurance - 1) * 10;

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
                if (characterManager.currentStamina < characterManager.maxStamina)
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
