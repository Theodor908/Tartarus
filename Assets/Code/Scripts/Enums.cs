using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{

}

public enum CharacterSlot
{
    CharacterSlot_0 = 0,
    CharacterSlot_1 = 1,
    CharacterSlot_2 = 2,
    CharacterSlot_3 = 3,
    CharacterSlot_4 = 4,
    CharacterSlot_5 = 5,
    CharacterSlot_6 = 6,
    CharacterSlot_7 = 7,
    CharacterSlot_8 = 8,
    CharacterSlot_9 = 9,
    NO_CHARACTER_SLOT = 10
}

public enum WeaponModelSlot
{
    RightHand,
    LeftHand
}

public enum AttackType
{
    LightAttack01,
    HeavyAttack
}