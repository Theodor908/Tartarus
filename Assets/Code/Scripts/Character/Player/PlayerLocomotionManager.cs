using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {

        PlayerManager playerManager; 
                
        public float horizontalMovement;
        public float verticalMovement;
        public float moveAmount;

        private Vector3 moveDirection;
        [SerializeField]
        float walkingSpeed = 2;
        float runningSpeed = 5;
        float rotationSpeed = 15;

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
            GetVerticalAndHorizontalInputs();
            //Camera perspective movement
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection += PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.y = 0;
            moveDirection.Normalize();

           if(PlayerInputManager.Instance.moveAmount > 0.5f )
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

    }
}
