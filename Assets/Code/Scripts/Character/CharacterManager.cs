using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class CharacterManager : MonoBehaviour
    {

        [HideInInspector]public CharacterController characterController;
        [HideInInspector]public Animator animator;

        [Header("Flags")]
        public bool isInteracting = false;
        public bool applyRootMotion = false;
        public bool canRotate = true;
        public bool canMove = true;

        protected virtual void Awake()
        {
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {

        }

        protected virtual void LateUpdate()
        {

        }

    }
}
