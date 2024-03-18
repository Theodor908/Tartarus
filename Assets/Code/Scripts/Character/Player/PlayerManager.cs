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

        public string characterName = "";

        protected override void Awake()
        {
            base.Awake();
            WorldSaveManager.instance.playerManager = this;

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

            currentStamina = PlayerStatsManager.instance.CalculateStaminaBasedOnEnduranceLevel(endurance);
            PlayerUIManager.instance.playerUIHudManager.setMaxStaminaValue(currentStamina);
        }

        protected override void Update()
        {
            base.Update();
            //Handle movement
            playerLocomotionManager.HandleAllMovement();
            // Regen stamina
            playerStatsManager.RegenerateStamina();
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
        }
        public void LoadGameFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            characterName = currentCharacterData.characterName;
            transform.position = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
        }

    }
}
