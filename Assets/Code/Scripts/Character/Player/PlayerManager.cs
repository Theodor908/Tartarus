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
        [HideInInspector] public PlayerCombatManager playerCombatManager;

        [Header("Equipment")]
        public int previousRightHandWeaponID = -1;
        public int previousLeftHandWeaponID = -1;
        public int currentRightHandWeaponID = 0;
        public int currentLeftHandWeaponID = 0;
        public bool isUsingRightHand = true;
        public bool isUsingLeftHand = false;

        [Header("Player Weapons")]
        public int currentWeaponBeingUsed = 0;

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
            playerCombatManager = GetComponent<PlayerCombatManager>();

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
        #region Death Event and Revive
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

            isDead = false;

            //Rebirth effects
            Debug.Log("Reviving player");
            animator.SetBool("isDead", false);

        }
        #endregion
        #region Weapon Actions
        public void HandleWeaponChange(int previousRightID, int currentRightID, int previousLeftID, int currentLeftID)
        {
            if (previousRightID != currentRightID)
            {
                Debug.Log("Right weapon change");
                OnCurrentRightHandWeaponIDChange(currentRightID);
                previousRightHandWeaponID = currentRightHandWeaponID;
                OnCurrentWeaponBeingUsedIDChange(currentRightID);
            }

            if(previousLeftID != currentLeftID)
            {
                OnCurrentLeftHandWeaponIDChange(currentLeftID);
                previousLeftHandWeaponID = currentLeftHandWeaponID;
                OnCurrentWeaponBeingUsedIDChange(currentLeftID);
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

        public void OnCurrentWeaponBeingUsedIDChange(int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponItem(newID));
            playerCombatManager.currentWeapon = newWeapon;
        }

        public void SetCharacterActionHand(bool rightHandAction)
        {

            if(rightHandAction)
            {
                isUsingRightHand = true;
                isUsingLeftHand = false;
            }
            else
            {
                isUsingRightHand = false;
                isUsingLeftHand = true;
            }

        }

        public void PerformWeaponBasedAction(int actionID, int weaponID)
        {
            WeaponItemAction weaponAction = WorldActionManager.instance.GetWeaponItemAction(actionID);

            if(weaponAction != null)
            {
                weaponAction.AttemptToPerformAction(this, WorldItemDatabase.instance.GetWeaponItem(weaponID));
            }
            else
            {
                Debug.Log("Weapon action not found");
            }

        }

        #endregion
        #region Save and Load
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
        #endregion

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
