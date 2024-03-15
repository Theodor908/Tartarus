using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerAnimationManager playerAnimationManager;

        protected override void Awake()
        {
            base.Awake();

            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimationManager = GetComponent<PlayerAnimationManager>();
        }

        protected override void Update()
        {
            base.Update();
            playerLocomotionManager.HandleAllMovement();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            PlayerCamera.instance.HandleAllCameraActions();
        }

    }
}
