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
        [HideInInspector] public CharacterCombatManager characterCombatManager;
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
        public bool isLockedOn = false;

        protected virtual void Awake()
        {
            characterController = GetComponent<CharacterController>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterAnimationManager = GetComponent<CharacterAnimationManager>();
            animator = GetComponent<Animator>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
        }

        protected virtual void Start()
        {
            IgnoreMyOwnColliders();
        }

        protected virtual void Update()
        {
            animator.SetBool("isGrounded", isGrounded);
            CheckHealthPoints();
        }

        protected virtual void LateUpdate()
        {

        }

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            currentHealth = 0;
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
            if(currentHealth <= 0 && !isDead)
            {
                animator.SetBool("isDead", true);
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

        public virtual void IgnoreMyOwnColliders()
        {
            // Take every colliders on the character and ignore them
            Collider characterControllerCollider = GetComponent<Collider>();
            Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();
            List<Collider> ignoreColliders = new List<Collider>();

            foreach (var collider in damageableCharacterColliders)
            {
                ignoreColliders.Add(collider);
            }

            ignoreColliders.Add(characterControllerCollider);

            foreach(var collider in ignoreColliders)
            {
                foreach(var otherCollider in ignoreColliders)
                {
                    Physics.IgnoreCollision(collider, otherCollider, true);
                }
            }

        }

    }
}
