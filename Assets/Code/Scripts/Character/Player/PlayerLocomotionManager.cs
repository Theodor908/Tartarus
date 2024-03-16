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

        [Header ("Dodge Settings")]
        private Vector3 rollDirection;

        protected override void Awake()
        {
            base.Awake();
            playerManager = GetComponent<PlayerManager>();
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            //Jump
            HandleRotation();
            //Fall
            //...
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

            if(PlayerInputManager.Instance.moveAmount > 1)
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
            else
            {
                //Idle
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

            // If moving then perform a roll
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

        public void HandleSprinting()
        {
            if(playerManager.isInteracting)
            {
                return;
            }

            // Stamina check
            // Not stationary -> sprint allowed
            // Stationary -> sprint not allowed



        }

    }
}
