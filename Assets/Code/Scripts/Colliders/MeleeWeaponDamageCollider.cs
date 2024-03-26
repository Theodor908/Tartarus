using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {

        [Header("Attacking character")]
        public CharacterManager characterCausingDamage;

        [Header("Weapon Attack Modifiers")]
        public float light_Attack_01_Modifier;

        protected override void Awake()
        {
            base.Awake();

            if(damageCollider == null)
            {
                damageCollider = GetComponent<Collider>();
            }

            damageCollider.enabled = false;

        }

        protected override void OnTriggerEnter(Collider other)
        {

            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            if (damageTarget != null)
            {
                // Dont damage ourselves
                if (damageTarget == characterCausingDamage)
                    return;

                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // Check if we can damage the target

                // Invluernable

                // BLocking

                // Damage the target

                Debug.Log(other);

                DamageTarget(damageTarget);

            }
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            if (charactersDamaged.Contains(damageTarget))
            {
                return;
            }

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicalDamage = magicalDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.contactPoint = contactPoint;

            switch (characterCausingDamage.characterCombatManager.currentAttackType)
            {
                case AttackType.LightAttack01:
                    ApplyAttackDamageModifiers(light_Attack_01_Modifier, damageEffect);
                    break;
                default:
                    break;
            }

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);

        }

        private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damageEffect)
        {
            damageEffect.physicalDamage += modifier;
            damageEffect.fireDamage += modifier;
            damageEffect.poiseDamage += modifier;

            // Attack is fully charged heavy then reapply modifiers
        }

    }
}