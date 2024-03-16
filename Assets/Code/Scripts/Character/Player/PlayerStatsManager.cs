using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        
        public static PlayerStatsManager instance;

        protected override void Awake()
        {

            base.Awake();

            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

    }
}
