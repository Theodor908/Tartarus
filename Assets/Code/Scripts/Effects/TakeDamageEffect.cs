using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {

        [Header("Character causing damage")]
        public CharacterManager characterCausingDamage; //Take the associated damage from this character

        [Header("Damage")]
        public float physicalDamage = 0; // Standard, strike, slash, pierce
        public float magicalDamage = 0;
        public float fireDamage = 0;

        [Header("Direction of damage taken")]
        public float angleOfDamage = 0; // For choosing animations
        public Vector3 contactPoint;

        [Header("Final damage")]
        [SerializeField] int totalDamage = 0;

        [Header("Poise")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;

        [Header ("Animation")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header ("Sound effects")]
        public bool playDamageSound = true;
        public AudioClip damageSound;

        public override void ProcessEffect(CharacterManager characterManager)
        {
            base.ProcessEffect(characterManager);

            // Character is dead
            if (characterManager.isDead)
            {
                return;
            }

            // Is invulnerable

            if (characterManager.isInvulnerable)
            {
                return;
            }

            CalculateDamage(characterManager);

        }

        private void CalculateDamage(CharacterManager characterManager)
        {
            if (characterCausingDamage != null)
            {
                // Calculate damage based on the character causing damage
            }

            // Add flat damage ? If I add fire resistance, I can add a flat damage to the fire damage

            totalDamage = Mathf.RoundToInt(physicalDamage + magicalDamage + fireDamage);

            if(totalDamage <= 0)
            {
                totalDamage = 1;
            }

            characterManager.currentHealth -= totalDamage;
            Debug.Log("Character " + characterManager.characterName + " took " + totalDamage + " damage");
        }

    }
}
