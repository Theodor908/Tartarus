using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tartarus
{
    public class WorldItemDatabase : MonoBehaviour
    {
        public static WorldItemDatabase instance;

        public WeaponItem unarmedWeapon;

        // Used to generate unique IDs for items
        [Header("Weapons")]
        [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();

        // List of all items in the game
        private List<Item> items = new List<Item>();
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

            foreach (var weapon in weapons)
            {
                items.Add(weapon);
            }

            // Assign unique IDs to items
            for(int i = 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }

        }

        public WeaponItem GetWeaponItem(int id)
        {
            return weapons.FirstOrDefault(weapon => weapon.itemID == id);
        }

    }
}