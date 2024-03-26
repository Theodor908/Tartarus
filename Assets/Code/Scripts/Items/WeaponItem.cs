using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class WeaponItem : Item
    {
        //Animator controller
        [Header ("Weapon model")]
        public GameObject weaponModel;

        [Header("Weapon requirements")]
        public int strengthRequirement = 0;
        public int dexterityRequirement = 0;

        [Header("Weapon base damage")]
        public int physicalDamage = 0;

        // Weapon guard break (blocking system)

        [Header("Weapon poise")]
        public float poiseDamage = 10;

        [Header ("Attack modifiers")]
        public float light_Attack_01_Modifier = 1.1f;
        // Light attack
        // Heavy attack
        // Critical attack

        [Header("Stamina cost modifiers")]
        public int baseStaminaCost = 20;
        public float lightAttackStaminaCostMultiplier = 0.9f;
        // Running attack stamina cost
        // Light attack stamina cost
        // Heavy attack stamina cost

        // Weapon deflection (melee) - bounce off shields

        // Item based actions
        [Header("Weapon actions")]
        public WeaponItemAction oh_RMB_Action;

       
    }
}