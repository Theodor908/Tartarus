using System.Collections;
using System.Collections.Generic;
using Tartarus;
using UnityEngine;

public class PlayerUIManager : MonoBehaviour
{
    
    public static PlayerUIManager instance;
    [HideInInspector] public PlayerUIHUDManager playerUIHudManager;
    [HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        playerUIHudManager = GetComponentInChildren<PlayerUIHUDManager>();
        playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();

    }

    private void Update()
    {
        
    }
}
