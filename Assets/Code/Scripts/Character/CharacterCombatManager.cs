using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class CharacterCombatManager : MonoBehaviour
    {

        CharacterManager characterManager;

        [Header("Attack Target")]
        public CharacterManager currentTarget;

        [Header("Attack Type")]
        public AttackType currentAttackType;

        [Header ("Lock on transform")]
        public Transform lockOnTransform;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        public virtual void SetLockOnTarget(CharacterManager newTarget)
        {
            if(newTarget != null)
            {
                currentTarget = newTarget;
            }
        }
    }
}