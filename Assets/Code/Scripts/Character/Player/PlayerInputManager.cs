using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class PlayerInputManager : MonoBehaviour
    {

        PlayerControls playerControls;
        static PlayerInputManager instance;

        [Header("Player Move Input")]
        [SerializeField] Vector2 movementInput;
        [SerializeField] Vector2 cameraInput;
        public float horizontalInput;
        public float verticalInput;
        public float moveAmount;

        [Header("Camera Move Input")]
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

            // Idle, Walking or Running
            if(moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5 && moveAmount <= 1)
            {
                moveAmount = 1f;
            }

        }

        private void HandleCameraMovementInput()
        {
            cameraHorizontalInput = cameraInput.x;
            cameraVerticalInput = cameraInput.y;
        }



    }
}
