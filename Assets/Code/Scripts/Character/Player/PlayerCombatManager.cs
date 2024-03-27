using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager playerManager;
        public WeaponItem currentWeapon;

        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
        }

        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            weaponAction.AttemptToPerformAction(playerManager, weaponPerformingAction);
            playerManager.PerformWeaponBasedAction(weaponAction.actionID, weaponPerformingAction.itemID);
        }

        public virtual void DrainStaminaBasedOnAttack()
        {
            float staminaDeducted = 0;

            if(currentWeapon == null)
            {
                Debug.Log("No weapon assigned to player");
                return;
            }

            switch(currentAttackType)
            {
                case AttackType.LightAttack01:
                    staminaDeducted = currentWeapon.baseStaminaCost * currentWeapon.lightAttackStaminaCostMultiplier;
                    break;
                default:
                    break;
            }

            playerManager.currentStamina -= Mathf.RoundToInt(staminaDeducted);
        }

        public override void SetLockOnTarget(CharacterManager newTarget)
        {
            base.SetLockOnTarget(newTarget);
            Debug.Log("Setting lock on target");
            PlayerCamera.instance.SetLockCameraHeight();
        }

    }
}