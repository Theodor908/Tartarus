using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class CharacterManager : MonoBehaviour
    {

        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;

        [Header("Stats")]
        public int vitality;
        public float currentHealth;
        public int endurance;
        public float currentStamina;
        public bool consumesStamina = false;

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
    }
}
