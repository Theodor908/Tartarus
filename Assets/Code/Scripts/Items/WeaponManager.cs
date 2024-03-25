using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] MeleeWeaponDamageCollider meleeWeaponDamageCollider;

        public void Awake()
        {
            meleeWeaponDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        }

        public void SetWeaponDamage(CharacterManager characterWieldingWeapon, WeaponItem weapon)
        {
            meleeWeaponDamageCollider.characterCausingDamage = characterWieldingWeapon;
            meleeWeaponDamageCollider.physicalDamage = weapon.physicalDamage;
        }

    }
}