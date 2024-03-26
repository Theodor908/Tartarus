using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager playerManager;
        public WeaponModelInstantiationSlot rightHandSlot;
        public WeaponModelInstantiationSlot leftHandSlot;

        [SerializeField] WeaponManager rightHandWeaponManager;
        [SerializeField] WeaponManager leftHandWeaponManager;

        private GameObject rightHandWeaponModel;
        private GameObject leftHandWeaponModel;

        protected override void Awake()
        {
            base.Awake();

            playerManager = GetComponent<PlayerManager>();

            InitializeWeaponSlots();
        }

        protected override void Start()
        {
            base.Start();

            LoadWeaponsOnBothHands();
        }

        private void InitializeWeaponSlots()
        {
            WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

            foreach (var weaponSlot in weaponSlots)
            {
                if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
                {
                    leftHandSlot = weaponSlot;
                }
            }

        }

        public void LoadWeaponsOnBothHands()
        {
            LoadWeaponOnLeftHand();
            LoadWeaponOnRightHand();
        }

        #region Right Hand

        public void SwitchRightWeapon()
        {
            playerManager.playerAnimationManager.PlayTargetAnimation("Swap_Right_Weapon_01", false, true, true, true);
            WeaponItem selectedWeapon = null;

            // Get next possible weapon
            playerManager.playerInventoryManager.rightHandSlotIndex++;
            // Reset index if it goes out of bounds
            if (playerManager.playerInventoryManager.rightHandSlotIndex < 0 || playerManager.playerInventoryManager.rightHandSlotIndex > 2)
            {
                playerManager.playerInventoryManager.rightHandSlotIndex = 0;

                // If there are more than one weapons dont go to unarmed
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponIndex = 0;

                for (int i = 0; i < playerManager.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
                {

                    if (playerManager.playerInventoryManager.weaponsInRightHandSlots[i].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                    {
                        weaponCount++;
                        if (firstWeapon == null)
                        {
                            firstWeapon = playerManager.playerInventoryManager.weaponsInRightHandSlots[i];
                            firstWeaponIndex = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {

                    playerManager.playerInventoryManager.rightHandSlotIndex = -1;
                    selectedWeapon = Instantiate(WorldItemDatabase.instance.unarmedWeapon);
                    playerManager.currentRightHandWeaponID = selectedWeapon.itemID;
                }
                else
                {

                    playerManager.playerInventoryManager.rightHandSlotIndex = firstWeaponIndex;
                    playerManager.currentRightHandWeaponID = firstWeapon.itemID;

                }

                return;

            }

            // Get the weapon from the inventory

            foreach (WeaponItem weapon in playerManager.playerInventoryManager.weaponsInRightHandSlots)
            {
                // IF the weapon is not unarmed we proceed

                if (playerManager.playerInventoryManager.weaponsInRightHandSlots[playerManager.playerInventoryManager.rightHandSlotIndex].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = playerManager.playerInventoryManager.weaponsInRightHandSlots[playerManager.playerInventoryManager.rightHandSlotIndex];
                    playerManager.currentRightHandWeaponID = selectedWeapon.itemID;
                    return;
                }
            }

            if(selectedWeapon == null && playerManager.playerInventoryManager.rightHandSlotIndex <= 2)
            {
                SwitchRightWeapon();
            }

        }

        public void LoadWeaponOnRightHand()
        {
            if(playerManager.playerInventoryManager.currentRightHandWeaponItem != null)
            {
                rightHandSlot.UnloadWeapon();
                rightHandWeaponModel = Instantiate(playerManager.playerInventoryManager.currentRightHandWeaponItem.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
                rightHandWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightHandWeaponManager.SetWeaponDamage(playerManager, playerManager.playerInventoryManager.currentRightHandWeaponItem);
            }
        }

        #endregion

        #region Left Hand

        public void SwitchLeftWeapon()
        {

        }

        public void LoadWeaponOnLeftHand()
        {
            if (playerManager.playerInventoryManager.currentLeftHandWeaponItem != null)
            {
                leftHandSlot.UnloadWeapon();
                leftHandWeaponModel = Instantiate(playerManager.playerInventoryManager.currentLeftHandWeaponItem.weaponModel);
                leftHandSlot.LoadWeapon(leftHandWeaponModel);
                leftHandWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
                leftHandWeaponManager.SetWeaponDamage(playerManager, playerManager.playerInventoryManager.currentLeftHandWeaponItem);
            }
        }

        #endregion

        // Damage colliders

        public void OpenDamageCollider()
        {
            // Open right weapon damage collider
            if(playerManager.isUsingRightHand)
            {
                rightHandWeaponManager.meleeWeaponDamageCollider.EnableDamageCollider();
            }

            if(playerManager.isUsingLeftHand)
            {
                leftHandWeaponManager.meleeWeaponDamageCollider.EnableDamageCollider();
            }

            // Maybe whoosh sfx?

        }

        public void CloseDamageCollider()
        {
            // Close right weapon damage collider
            if (playerManager.isUsingRightHand)
            {
                rightHandWeaponManager.meleeWeaponDamageCollider.DisableDamageCollider();
            }

            if (playerManager.isUsingLeftHand)
            {
                leftHandWeaponManager.meleeWeaponDamageCollider.DisableDamageCollider();
            }
        }

    }
}