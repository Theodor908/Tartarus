using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class CharacterCombatManager : MonoBehaviour
    {

        [Header("Attack Target")]
        public CharacterManager currentTarget;

        [Header("Attack Type")]
        public AttackType currentAttackType;

        [Header ("Lock on transform")]
        public Transform lockOnTransform;

        protected virtual void Awake()
        {

        }

    }
}