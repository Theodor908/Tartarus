using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Colliders")]
        protected Collider damageCollider;

        [Header ("Damage")]
        public float physicalDamage = 0; // Standard, strike, slash, pierce
        public float magicalDamage = 0;
        public float fireDamage = 0;

        [Header ("Contact points")]
        private Vector3 contactPoint;

        [Header ("Characters damaged")]
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

        private void OnTriggerEnter(Collider other)
        {

            CharacterManager damageTarget = other.GetComponentInParent<CharacterManager>();

            if(damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // Check if we can damage the target

                // Invluernable

                // BLocking

                // Damage the target

                Debug.Log(other);

                DamageTarget(damageTarget);

            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
            // Instantiate damage effect
            // Must check if multiple colliders are hit to not instantiate multiple effects

            if(charactersDamaged.Contains(damageTarget))
            {
                return;
            }

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicalDamage = magicalDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.contactPoint = contactPoint;

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }   

        public virtual void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public virtual void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            charactersDamaged.Clear();
        }

    }
}