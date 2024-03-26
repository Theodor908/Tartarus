using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class CharacterAnimationManager : MonoBehaviour
    {
        CharacterManager characterManager;

        [Header("Damage animations")]
        public string hit_Forward_Medium_01 = "hit_Forward_Medium_01";
        public string hit_Backward_Medium_01 = "hit_Backward_Medium_01";
        public string hit_Left_Medium_01 = "hit_Left_Medium_01";
        public string hit_Right_Medium_01 = "hit_Right_Medium_01";

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        public void UpdateAnimatorMovementParameters(float horizontalValue, float verticalValue)
        {
            characterManager.animator.SetFloat("Horizontal", horizontalValue, 0.1f, Time.deltaTime);
            characterManager.animator.SetFloat("Vertical", verticalValue, 0.1f, Time.deltaTime);
        }

        public virtual void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
        {
            Debug.Log("Playing animation " + targetAnimation);
            characterManager.applyRootMotion = applyRootMotion;
            characterManager.animator.applyRootMotion = applyRootMotion;
            characterManager.animator.CrossFade(targetAnimation, 0.2f);
            characterManager.isInteracting = isInteracting; // Allow or stop certain actions while interacting
            characterManager.canRotate = canRotate;
            characterManager.canMove = canMove;
        }

        public virtual void PlayTargetAttackActionAnimation(AttackType attackType,string targetAnimation, bool isInteracting, bool applyRootMotion = true, bool canRotate = false, bool canMove = false)
        {
            //Keep track of the last action performed to look for combos
            //Keep track of current attack type
            //Update animation set to the current weapons animations
            // decide if our attack can be parried
            // we are in an attacking flag

            Debug.Log("Playing action animation " + targetAnimation);

            characterManager.characterCombatManager.currentAttackType = attackType;
            characterManager.applyRootMotion = applyRootMotion;
            characterManager.animator.applyRootMotion = applyRootMotion;
            characterManager.animator.CrossFade(targetAnimation, 0.2f);
            characterManager.isInteracting = isInteracting; // Allow or stop certain actions while interacting
            characterManager.canRotate = canRotate;
            characterManager.canMove = canMove;
        }

    }
}

