using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class CharacterLocomotionManager : MonoBehaviour
    {

        CharacterManager characterManager;

        [Header("Ground check && Jumping")]
        [SerializeField] protected float gravityForce = -9.8f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckSphereRadius = 0.3f;
        [SerializeField] protected Vector3 yVelocity; // gravity
        [SerializeField] protected float groundedYVelocity = -40f;
        [SerializeField] protected float fallStartYVelocity = -2f; // velocity gain per frame when falling
        protected bool fallingVelocityHasBeenSet = false;
        protected float inAirTimer = 0;
        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            HandleGroundCheck();

            if(characterManager.isGrounded)
            {
                // characterManager.isJumping = false; for hard coded jump and also make radius of sphere smaller/ use raycast
                if(yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                if(!characterManager.isJumping && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }

                inAirTimer += Time.deltaTime;
                characterManager.animator.SetFloat("inAirTimer", inAirTimer);

                yVelocity.y += gravityForce * Time.deltaTime;

            }

            characterManager.characterController.Move(yVelocity * Time.deltaTime);

        }

        protected void HandleGroundCheck()
        {
            characterManager.isGrounded = Physics.CheckSphere(characterManager.transform.position, groundCheckSphereRadius, groundLayer);
        }

        protected void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, groundCheckSphereRadius);
        }

    }
}
