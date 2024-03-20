using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {

        PlayerManager playerManager; 
                
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] float verticalMovement;
        [HideInInspector] float moveAmount;

        [Header ("Movement Settings")]
        private Vector3 moveDirection;
        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float sprintingSpeed = 7;
        [SerializeField] float rotationSpeed = 15;
        [SerializeField] float sprintStaminaCost = 2;

        [Header ("Dodge Settings")]
        private Vector3 rollDirection;
        [SerializeField] float dodgeStaminaCost = 5;

        [Header ("Jump Settings")]
        [SerializeField] float jumpStaminaCost = 2;
        [SerializeField] float jumpHeight = 1.25f;
        private Vector3 jumpDirection;
        [SerializeField] float jumpForwardSpeed = 3f;
        [SerializeField] float freeFallSpeed = 5;

        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleJumpMovement();
            HandleRotation();
            HandleFreeFallMovement();
        }

        private void GetVerticalAndHorizontalInputs()
        {
            horizontalMovement = PlayerInputManager.Instance.horizontalInput;
            verticalMovement = PlayerInputManager.Instance.verticalInput;
            moveAmount = PlayerInputManager.Instance.moveAmount;
        }

        public void HandleGroundedMovement()
        {

            if(!playerManager.canMove)
            {
                return;
            }

            GetVerticalAndHorizontalInputs();
            //Camera perspective movement
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection += PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.y = 0;
            moveDirection.Normalize();

            if(PlayerInputManager.Instance.moveAmount > 1 && playerManager.isSprinting)
            {
                playerManager.characterController.Move(sprintingSpeed * Time.deltaTime * moveDirection);
            }
            else if(PlayerInputManager.Instance.moveAmount > 0.5f )
            {
                playerManager.characterController.Move(runningSpeed * Time.deltaTime * moveDirection);
            }
            else if(PlayerInputManager.Instance.moveAmount <= 0.5f && PlayerInputManager.Instance.moveAmount > 0)
            {
                playerManager.characterController.Move(walkingSpeed * Time.deltaTime * moveDirection);
            }

        }

        public void HandleJumpMovement()
        {

            if(playerManager.isJumping)
            {
                playerManager.characterController.Move(jumpDirection * Time.deltaTime * jumpForwardSpeed);
            }

        }

        public void HandleFreeFallMovement()
        {

            if(!playerManager.isGrounded)
            {
                Vector3 freeFallDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
                freeFallDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;

                playerManager.characterController.Move(freeFallDirection * Time.deltaTime * freeFallSpeed);

            }

        }

        public void HandleRotation()
        {

            if(!playerManager.canRotate)
            {
                return;
            }

            Vector3 targetRotation = Vector3.zero;
            targetRotation = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            targetRotation += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;

            targetRotation.y = 0;
            targetRotation.Normalize();

            if(targetRotation == Vector3.zero)
            {
                targetRotation = transform.forward;
            }

            Quaternion turnRotation = Quaternion.LookRotation(targetRotation);
            Quaternion newRotation = Quaternion.Slerp(transform.rotation, turnRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = newRotation;

        }

        public void AttemptToPerformDodge()
        {

            if(playerManager.isInteracting)
            {
                return;
            }

            if(playerManager.isJumping)
            {
                return;
            }

            if (playerManager.currentStamina < dodgeStaminaCost * Time.deltaTime)
            {
                return;
            }

            // If moving then perform a roll

            // Stamina cost
            playerManager.currentStamina -= dodgeStaminaCost;

            if (moveAmount > 0)
            {
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
                rollDirection.y = 0;
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);

                playerManager.transform.rotation = playerRotation;

                playerManager.playerAnimationManager.PlayTargetAnimation("Roll_Forward_01", true, true);

            }
            else // Stationary
            {
                // Perform a backestep, hardcoded maybe? -- No

                playerManager.playerAnimationManager.PlayTargetAnimation("Backstep_01", true, true);
            }
        }

        public void AttemptToPerformJump()
        {
            // Check for already performing actions, including jumping, interacting, etc and check if grounded
            if (playerManager.isInteracting)
            {
                return;
            }

            if(playerManager.isJumping)
            {
                return;
            }

            if(!playerManager.isGrounded)
            {
                return;
            }

            if (playerManager.currentStamina < jumpStaminaCost)
            {
                return;
            }

            // Stamina cost
            playerManager.currentStamina -= jumpStaminaCost;

            playerManager.playerAnimationManager.PlayTargetAnimation("Jump_Start", false);

            playerManager.isJumping = true;

            // Hard coded jump
            //ApplyJumpVelocity();

            jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
            jumpDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;

            // Handle movement during jump
            if(jumpDirection != Vector3.zero)
            {
                if (playerManager.isSprinting)
                {
                    jumpDirection *= 1; // Sprint jump
                }
                else if (moveAmount >= 1)
                {
                    jumpDirection *= 0.75f; // Run jump
                }
                else if (moveAmount < 1)
                {
                    jumpDirection *= 0.25f; // Walk jump
                }
            }

        }

        public void ApplyJumpVelocity()
        {
            yVelocity.y = Mathf.Sqrt(-2 * gravityForce * jumpHeight);

        }

        public void HandleSprinting()
        {
            if(playerManager.isInteracting)
            {
                playerManager.isSprinting = false;
            }

            // No sprinting if no stamina left

            if(playerManager.currentStamina < sprintStaminaCost * Time.deltaTime)
            {
                playerManager.isSprinting = false;
            }

            if (moveAmount < 0.5f && playerManager.currentStamina < sprintStaminaCost * Time.deltaTime)
            {
                playerManager.isSprinting = false;
            }

            // Stamina cost
            if (playerManager.isSprinting)
            {
                playerManager.currentStamina -= sprintStaminaCost * Time.deltaTime;
            }

        }

    }
}
