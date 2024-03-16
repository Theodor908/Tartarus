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

        protected override void Start()
        {
            base.Start();
            currentStamina = PlayerStatsManager.instance.CalculateStaminaBasedOnEnduranceLevel(endurance);
            PlayerUIManager.instance.playerUIHudManager.setMaxStaminaValue(currentStamina);
        }

        protected override void Update()
        {
            base.Update();
            //Handle movement
            playerLocomotionManager.HandleAllMovement();
            // Regen stamina
            PlayerStatsManager.instance.RegenerateStamina();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            PlayerCamera.instance.HandleAllCameraActions();
        }

    }
}
