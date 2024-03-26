using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class WorldUtilityManager : MonoBehaviour
    {
        public static WorldUtilityManager instance;

        [Header ("Layers")]
        [SerializeField] LayerMask characterLayers;
        [SerializeField] LayerMask enviromentalLayers;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public LayerMask GetCharacterLayers()
        {
            return characterLayers;
        }

        public LayerMask GetEnviromentalLayers()
        {
            return enviromentalLayers;
        }
    }
}