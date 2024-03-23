using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{

    [System.Serializable]
    // Reference of data
    public class CharacterSaveData
    {
        [Header ("Scene Index")]
        public int sceneIndex;

        [Header("Character Name")]
        public string characterName = "Blank";

        [Header("Total time played")]
        public float secondsPlayed;

        // Json needs basic data types
        [Header("World Position")]
        public float xPosition;
        public float yPosition;
        public float zPosition;

        [Header("Resources")]
        public float currentHealth = 100;
        public float currentStamina = 100;

        [Header("Character Stats")]
        public int vitality = 1;
        public int endurance = 1;

    }
}
