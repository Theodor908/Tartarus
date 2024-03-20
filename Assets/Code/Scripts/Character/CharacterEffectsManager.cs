using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        
        CharacterManager characterManager;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffect effect)
        { 
            effect.ProcessEffect(characterManager);
        }

    }
}