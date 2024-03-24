using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {

        [Header("Attacking character")]
        public CharacterManager characterCausingDamage;

    }
}