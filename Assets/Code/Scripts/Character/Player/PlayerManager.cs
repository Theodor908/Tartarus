using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tartarus
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerAnimationManager playerAnimationManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        [Header ("Debug for level up")]
        public bool levelUp = false;
        public int levelUpPoints = 0;

        protected override void Awake()
        {
            base.Awake();
            WorldSaveManager.instance.playerManager = this;
            // Check if WorldSaveManager is set to PlayerManager
            if(WorldSaveManager.instance.playerManager == this)
            {
                Debug.Log("PlayerManager is set to WorldSaveManager");
            }
            else
            {
                Debug.Log("PlayerManager is not set to WorldSaveManager");
            }

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimationManager = GetComponent<PlayerAnimationManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();

        }

        protected override void Start()
        {
            base.Start();
            PlayerUIManager.instance.playerUIHudManager.setMaxHealthValue(maxHealth);
            PlayerUIManager.instance.playerUIHudManager.setMaxStaminaValue(maxStamina);

            currentHealth = maxHealth;
            currentStamina = maxStamina;

            PlayerStatsManager.instance.ResetStaminaRegenTimer();

        }

        protected override void Update()
        {
            base.Update();

            //Debug for level up
            if(levelUp)
            {
                playerStatsManager.LevelUpVitality(levelUpPoints);
                playerStatsManager.LevelUpEndurance(levelUpPoints);
                levelUp = false;
                levelUpPoints = 0;
            }

            //Handle movement
            playerLocomotionManager.HandleAllMovement();
            // Update resources
            PlayerUIManager.instance.playerUIHudManager.setNewHealthValue(currentHealth);
            PlayerUIManager.instance.playerUIHudManager.setNewStaminaValue(currentStamina);
            
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            PlayerCamera.instance.HandleAllCameraActions();
        }

        public void SaveGameToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            currentCharacterData.characterName = characterName;
            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.zPosition = transform.position.z;

            currentCharacterData.vitality = vitality;
            currentCharacterData.endurance = endurance;

            currentCharacterData.currentHealth = currentHealth;
            currentCharacterData.currentStamina = currentStamina;


        }
        public void LoadGameFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            characterName = currentCharacterData.characterName;
            transform.position = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);

            vitality = currentCharacterData.vitality;
            endurance = currentCharacterData.endurance;

            currentHealth = currentCharacterData.currentHealth;
            currentStamina = currentCharacterData.currentStamina;
        }

    }
}
