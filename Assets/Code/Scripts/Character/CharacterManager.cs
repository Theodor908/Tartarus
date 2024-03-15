using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class CharacterManager : MonoBehaviour
    {

        public CharacterController characterController;
        public Animator animator;
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
