using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tartarus
{
    public class WorldActionManager : MonoBehaviour
    {
        public static WorldActionManager instance;

        [Header ("Weapon item actions")]
        public WeaponItemAction[] weaponItemActions;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            for(int i = 0; i < weaponItemActions.Length; i++)
            {
                weaponItemActions[i].actionID = i;
            }
        }

        public WeaponItemAction GetWeaponItemAction(int actionID)
        {
            return weaponItemActions.FirstOrDefault(x => x.actionID == actionID);
        }

    }
}
