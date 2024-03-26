using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackWeaponItemAction : WeaponItemAction
    {

        [SerializeField] string light_Attack_01 = "RightHand_Light_Attack_01";
        [SerializeField] string light_Attack_02 = "LeftHand_Light_Attack_01";

        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            //Check for stops

            if(playerPerformingAction.currentStamina <= 0)
            {
                return;
            }

            if(!playerPerformingAction.isGrounded)
            {
                return;
            }

            if(playerPerformingAction.isInteracting)
            {
                return;
            }

            PerformLightAttack(playerPerformingAction, weaponPerformingAction);

        }

        private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if(playerPerformingAction.isUsingRightHand)
            {
                playerPerformingAction.playerAnimationManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01,light_Attack_01, true);
                return;
            }
            if(playerPerformingAction.isUsingLeftHand)
            {
               playerPerformingAction.playerAnimationManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01,light_Attack_02, true);
                return;
            }
        }

    }
}
