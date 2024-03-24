using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class CharacterManager : MonoBehaviour
    {
        [Header ("Status of character")]
        public bool isDead = false;
        public bool isInvulnerable = false;

        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public CharacterEffectsManager characterEffectsManager;
        [HideInInspector] public CharacterAnimationManager characterAnimationManager;
        [HideInInspector] public Animator animator;
        public string characterName = "";

        [Header("Stats")]
        public float maxHealth = 100;
        public int vitality = 1;
        public float currentHealth;
        public float maxStamina = 100;
        public int endurance = 1;
        public float currentStamina;

        [Header("Flags")]
        public bool isInteracting = false;
        public bool isGrounded = true;
        public bool isJumping = false;
        public bool isSprinting = false;
        public bool applyRootMotion = false;
        public bool canRotate = true;
        public bool canMove = true;

        protected virtual void Awake()
        {
            characterController = GetComponent<CharacterController>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterAnimationManager = GetComponent<CharacterAnimationManager>();
            animator = GetComponent<Animator>();
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            animator.SetBool("isGrounded", isGrounded);
        }

        protected virtual void LateUpdate()
        {

        }

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            currentHealth = 1;
            isDead = true;

            // Reset all flags

            // if in air death air anim

            // if on ground death ground anim

            if(!manuallySelectDeathAnimation)
            {
                characterAnimationManager.PlayTargetAnimation("Dead_01", true);
            }
            else
            {
                // Play default death animation
            }

            // Random death animation

            yield return new WaitForSeconds(5);

            // Award players with runes

            // Disable character

        }

        public void CheckHealthPoints()
        {
            if(currentHealth <= 0)
            {
                StartCoroutine(ProcessDeathEvent());
            }

            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

        }

        public virtual void ReviveCharacter()
        {

        }

    }
}
