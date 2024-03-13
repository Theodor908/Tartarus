using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class CharacterManager : MonoBehaviour
    {

        public CharacterController characterController;

        protected virtual void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        protected virtual void Update()
        {

        }

        protected virtual void LateUpdate()
        {

        }

    }
}
