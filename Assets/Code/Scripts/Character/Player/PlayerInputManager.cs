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
        [SerializeField] bool dodgeInput = false;
        

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
                
                playerControls = new PlayerControls();
                playerControls.PlayerMovement.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
                playerControls.CameraMovement.Movement.performed += ctx => cameraInput = ctx.ReadValue<Vector2>();
                playerControls.PlayerActions.Dodge.performed += ctx => dodgeInput = true;
            }

            playerControls.Enable();

        }

        private void Update()
        {
            HandleMovementInput();
            HandleCameraMovementInput();
        }

        private void HandleMovementInput()
        {
            horizontalInput = movementInput.x;
            verticalInput = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput)); 

            // Idle or Walking/Running
            if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1f;
            }

            // Not locked-on
            playerManager.playerAnimationManager.UpdateAnimatorMovementParameters(0,moveAmount);

            // Locked-on
            // to implemenet

        }

        private void HandleCameraMovementInput()
        {
            cameraHorizontalInput = cameraInput.x;
            cameraVerticalInput = cameraInput.y;
        }

        private void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false;
                // Call the dodge function
            }
        }

    }
}
