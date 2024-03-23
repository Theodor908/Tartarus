using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class CharacterAnimationManager : MonoBehaviour
    {
        CharacterManager characterManager;

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
            Debug.Log(targetAnimation);
            characterManager.applyRootMotion = applyRootMotion;
            characterManager.animator.applyRootMotion = applyRootMotion;
            characterManager.animator.CrossFade(targetAnimation, 0.2f);
            characterManager.isInteracting = isInteracting; // Allow or stop certain actions while interacting
            characterManager.canRotate = canRotate;
            characterManager.canMove = canMove;
        }

    }
}

