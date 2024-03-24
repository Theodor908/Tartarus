using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tartarus
{
    public class PlayerManager : CharacterManager
    {

        [Header ("Debug Menu")]
        [SerializeField] bool levelUp = false;
        [SerializeField] int levelUpPoints = 0;
        [SerializeField] bool respawnPlayer = false;
        [SerializeField] bool switchRightHandWeapon = false;

        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerAnimationManager playerAnimationManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;

        [Header("Weapon slots")]
        public int previousRightHandWeaponID = -1;
        public int previousLeftHandWeaponID = -1;
        public int currentRightHandWeaponID = 0;
        public int currentLeftHandWeaponID = 0;

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
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();

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

            //Handle movement
            playerLocomotionManager.HandleAllMovement();
            //Handle weapon change
            HandleWeaponChange(previousRightHandWeaponID, currentRightHandWeaponID, previousLeftHandWeaponID, currentLeftHandWeaponID);
            // Update resources
            PlayerUIManager.instance.playerUIHudManager.setNewHealthValue(currentHealth);
            PlayerUIManager.instance.playerUIHudManager.setNewStaminaValue(currentStamina);
            
            CheckHealthPoints();   

            //Debug
            DebugMenu();

        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            PlayerCamera.instance.HandleAllCameraActions();
        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            PlayerUIManager.instance.playerUIPopUpManager.ShowYouDiedPopUp();
            return base.ProcessDeathEvent(manuallySelectDeathAnimation);
        }

        public override void ReviveCharacter()
        {
            base.ReviveCharacter();
            
            currentHealth = maxHealth;
            currentStamina = maxStamina;

            //Rebirth effects

            playerAnimationManager.PlayTargetAnimation("Empty", false);

        }

        public void HandleWeaponChange(int previousRightID, int currentRightID, int previousLeftID, int currentLeftID)
        {
            if (previousRightID != currentRightID)
            {
                Debug.Log("Right weapon change");
                OnCurrentRightHandWeaponIDChange(currentRightID);
                previousRightHandWeaponID = currentRightHandWeaponID;
            }

            if(previousLeftID != currentLeftID)
            {
                OnCurrentLeftHandWeaponIDChange(currentLeftID);
                previousLeftHandWeaponID = currentLeftHandWeaponID;
            }
        }

        public void OnCurrentRightHandWeaponIDChange(int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponItem(newID));
            playerInventoryManager.currentRightHandWeaponItem = newWeapon;
            playerEquipmentManager.LoadWeaponOnRightHand();
            Debug.Log("Loaded player manager new weapon " + newWeapon.itemName + " with ID " + newWeapon.itemID);
        }

        public void OnCurrentLeftHandWeaponIDChange(int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponItem(newID));
            playerInventoryManager.currentLeftHandWeaponItem = newWeapon;
            playerEquipmentManager.LoadWeaponOnLeftHand();
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

        //Debug

        private void DebugMenu()
        {
            if (levelUp)
            {
                playerStatsManager.LevelUpVitality(levelUpPoints);
                playerStatsManager.LevelUpEndurance(levelUpPoints);
                levelUp = false;
                levelUpPoints = 0;
            }

            if (respawnPlayer)
            {
                respawnPlayer = false;
                ReviveCharacter();
            }

            if(switchRightHandWeapon)
            {
                switchRightHandWeapon = false;
                playerEquipmentManager.SwitchRightWeapon();
            }

        }

    }
}
