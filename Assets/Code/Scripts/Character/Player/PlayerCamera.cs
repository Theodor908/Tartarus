using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Tartarus
{
    public class PlayerCamera : MonoBehaviour
    {

        public static PlayerCamera instance;
        public Camera cameraObject;
        public Transform cameraPivotTransform;
        public PlayerManager playerManager;

        [Header("Camera Settings")]
        [SerializeField] float cameraSmoothSpeed = 1;
        [SerializeField] float upAndDownRotationSpeed = 220; // Look up and down
        [SerializeField] float leftAndRightRotationSpeed = 220; // Look left and right
        [SerializeField] float maximumPivot = 60; // Highest look up
        [SerializeField] float minimumPivot = -30; // Lowest look down
        [SerializeField] float cameraCollisionRadius = 0.03f; // How much the camera will move when it collides with the enviroment
        [SerializeField] LayerMask collisionLayers; // What the camera will collide with

        [Header("Camera values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition; // Used camera for collision (Moves camera object to this position)
        [SerializeField] float leftAndRightLookAngle;
        [SerializeField] float upAndDownLookAngle;
        private float cameraZPosition; //Value for camera collision
        private float targetCameraZPosition; // Value for camera collision

        [Header ("Lock On")]
        [SerializeField] private float lockOnRadius = 25;
        [SerializeField] private float minimumViewableAngle = -50;
        [SerializeField] private float maximumViewableAngle = 50;
        [SerializeField] private float maximumLockOnDistance = 25;


        private void Awake()
        {

            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

        }

        private void Start()
        {
            cameraZPosition = cameraObject.transform.localPosition.z;
        }

        public void HandleAllCameraActions()
        {
            if (playerManager != null)
            {
                HandleFollowPlayer();
                HandleCameraRotation();
                HandleCollisions();
            }
        }

        private void HandleFollowPlayer()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, playerManager.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleCameraRotation()
        {
            // Rotate the camera around the player
            // If locked-on to an enemy, rotate towards the enemy -- to implement
            
            // Rotate the camera left and right
            leftAndRightLookAngle += PlayerInputManager.Instance.cameraHorizontalInput * leftAndRightRotationSpeed * Time.deltaTime;
            leftAndRightLookAngle = Mathf.Repeat(leftAndRightLookAngle, 360);
            // Rotate the camera up and down
            upAndDownLookAngle -= PlayerInputManager.Instance.cameraVerticalInput * upAndDownRotationSpeed * Time.deltaTime;
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

            // Left and right rotation
            Vector3 cameraRotation = Vector3.zero;

            cameraRotation.y = leftAndRightLookAngle;
            Quaternion targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;

            // Up and down rotation
            cameraRotation = Vector3.zero;

            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);
            cameraPivotTransform.localRotation = targetRotation;

        }

        private void HandleCollisions()
        {
            // If the camera collides with the enviroment, move the camera to the collision point
            targetCameraZPosition = cameraZPosition;
            RaycastHit hit;
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collisionLayers))
            {
                float distanceFromObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetCameraZPosition = -(distanceFromObject - cameraCollisionRadius);
            }

            // If the camera is closer to the player than the collision radius, move the camera to the collision point

            if (Mathf.Abs(targetCameraZPosition) > Mathf.Abs(cameraZPosition))
            {
                targetCameraZPosition = -cameraCollisionRadius;

            }

            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, Time.deltaTime * 2.5f);
            cameraObject.transform.localPosition = cameraObjectPosition;

        }

        public void HandleLocatingLockOnTargets()
        {
            float shortDistance = Mathf.Infinity; // Shortest distance to a target
            float shortDistanceOfRightTarget = Mathf.Infinity; // Shortest distance from the target to another on its right (used for already lock on feature)
            float shortDistanceOfLeftTarget = -Mathf.Infinity; // Shortest distance from the target to another on its left (used for already lock on feature)

            Collider[] colliders = Physics.OverlapSphere(playerManager.transform.position, lockOnRadius, WorldUtilityManager.instance.GetCharacterLayers()); // Find all colliders within a radius of 25

            for(int i = 0; i < colliders.Length; i++)
            {
                CharacterManager lockOnTarget = colliders[i].transform.GetComponent<CharacterManager>();

                if(lockOnTarget != null)
                {

                    if (lockOnTarget.transform.root == playerManager.transform.root)
                        continue;

                    if (lockOnTarget.isDead)
                        continue;

                    // Check if in fov
                    Vector3 lockOnTargetDirection = lockOnTarget.transform.position - playerManager.transform.position;
                    float distanceFromTarget = Vector3.Distance(playerManager.transform.position, lockOnTarget.transform.position);
                    float viewableAngle = Vector3.Angle(lockOnTargetDirection, cameraObject.transform.forward);

                    if (distanceFromTarget > maximumLockOnDistance)
                        continue;

                    Debug.Log("Distance from target: " + distanceFromTarget + " Viewable angle: " + viewableAngle + " Minimum viewable angle: " + minimumViewableAngle + " Maximum viewable angle: " + maximumViewableAngle);

                    if (viewableAngle > minimumViewableAngle && viewableAngle < maximumViewableAngle)
                    {
                        RaycastHit hit;

                        Debug.Log("Only Linecast to pass");

                        if(Physics.Linecast(playerManager.playerCombatManager.lockOnTransform.position, lockOnTarget.characterCombatManager.lockOnTransform.position, out hit, WorldUtilityManager.instance.GetEnviromentalLayers()))
                        {
                            //Something is blocking the view
                            continue;
                        }
                        else
                        {
                            //Debug.Log(hit.collider.gameObject.name);
                            Debug.Log("We made it");
                        }
                    }

                }

            }

        }

    }
}
