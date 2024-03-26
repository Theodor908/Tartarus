using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class PlayerInputManager : MonoBehaviour
    {

        PlayerControls playerControls;
        public static PlayerInputManager instance;
        public PlayerManager playerManager;

        [Header("Player Move Input")]
        [SerializeField] Vector2 movementInput;
        public float horizontalInput;
        public float verticalInput;
        public float moveAmount;

        [Header("Player Action Input")]
        [SerializeField] bool walkInput = false;
        [SerializeField] bool dodgeInput = false;
        [SerializeField] bool jumpInput = false;
        [SerializeField] bool sprintInput = false;
        [SerializeField] bool RMB_Input = false;
        

        [Header("Camera Move Input")]
        [SerializeField] Vector2 cameraInput;
        public float cameraVerticalInput;
        public float cameraHorizontalInput;


        public static PlayerInputManager Instance { get => instance; }

        private void Awake()
        {
            
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                // Movement
                playerControls = new PlayerControls();
                playerControls.PlayerMovement.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
                playerControls.CameraMovement.Movement.performed += ctx => cameraInput = ctx.ReadValue<Vector2>();
                // Walk
                playerControls.PlayerActions.Walk.performed += ctx => walkInput = true;
                playerControls.PlayerActions.Walk.canceled += ctx => walkInput = false;
                // Dodge
                playerControls.PlayerActions.Dodge.performed += ctx => dodgeInput = true;
                // Jump
                playerControls.PlayerActions.Jump.performed += ctx => jumpInput = true;

                // RMB
                playerControls.PlayerActions.RMB.performed += ctx => RMB_Input = true;

                // Sprint
                playerControls.PlayerActions.Sprint.performed += ctx => sprintInput = true;
                playerControls.PlayerActions.Sprint.canceled += ctx => sprintInput = false;
            }

            playerControls.Enable();

        }

        private void Update()
        {
            HandleRMBInput();
            HandleJumpInput();
            HandleDodgeInput();
            HandleSprintInput();
            HandleMovementInput();
            HandleCameraMovementInput();

        }

        #region MovementInput

        private void HandleMovementInput()
        {
            horizontalInput = movementInput.x;
            verticalInput = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput)); 

            if(moveAmount > 0 && walkInput)
            {
                moveAmount = 0.5f;
            }
            else if(moveAmount > 0 && !sprintInput)
            {
                moveAmount = 1f;
            }
            else if(moveAmount > 0 && sprintInput)
            {
                moveAmount = 2f;
            }

            // Not locked-on
            playerManager.playerAnimationManager.UpdateAnimatorMovementParameters(0,moveAmount);

            // Locked-on
            // to implement

        }

        private void HandleCameraMovementInput()
        {
            cameraHorizontalInput = cameraInput.x;
            cameraVerticalInput = cameraInput.y;
        }

        #endregion

        #region ActionInput

        private void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false;
                playerManager.playerLocomotionManager.AttemptToPerformDodge();
            }
        }

        private void HandleSprintInput()
        {
            playerManager.isSprinting = sprintInput;
            if (sprintInput)
            {
                playerManager.playerLocomotionManager.HandleSprinting();
            }
        }

        private void HandleJumpInput()
        {
            if(jumpInput)
            {
                jumpInput = false;
                playerManager.playerLocomotionManager.AttemptToPerformJump();
            }
        }

        public void HandleRMBInput()
        {
            if (RMB_Input)
            {
                RMB_Input = false;
                // If ui is opne return
                playerManager.SetCharacterActionHand(true);

                // Maybe some two handed weapon logic ?

                playerManager.playerCombatManager.PerformWeaponBasedAction(playerManager.playerInventoryManager.currentRightHandWeaponItem.oh_RMB_Action, playerManager.playerInventoryManager.currentRightHandWeaponItem);

            }
        }

        #endregion

    }
}
