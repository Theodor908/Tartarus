using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tartarus
{
    public class TitleScreenManager : MonoBehaviour
    {

        [Header ("Menu objects")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleSreenLoadMenu;

        public void StartNewGame()
        {
            StartCoroutine(WorldSaveManager.instance.LoadWorldScene());
        }

        public void OpenLoadGameMenu()
        {

            // Close the main menu and open the load menu

            titleScreenMainMenu.SetActive(false);
            titleSreenLoadMenu.SetActive(true);



        }

        public void CloseLoadGameMenu()
        {

            // Close the load menu and open the main menu

            titleScreenMainMenu.SetActive(true);
            titleSreenLoadMenu.SetActive(false);

        }

    }
}
    