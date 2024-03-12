using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class PlayerInputManager : MonoBehaviour
    {

        PlayerControls playerControls;
        PlayerInputManager instance;

        [SerializeField]
        Vector2 movementInput;

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

            }

            playerControls.Enable();

        }



    }
}
