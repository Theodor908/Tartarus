using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{

    [System.Serializable]
    // Data is referenced for every save file
    // No monobehaviour
    // Reference of data
    public class CharacterSaveData
    {
        [Header("Character Name")]
        public string characterName = "Blank";

        [Header("Total time played")]
        public float secondsPlayed;

        // Json needs basic data types
        [Header("World Position")]
        public float xPosition;
        public float yPosition;
        public float zPosition;



    }
}
