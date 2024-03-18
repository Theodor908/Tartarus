using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tartarus
{
    public class TitleScreenManager : MonoBehaviour
    {

        public static TitleScreenManager instance;

        [Header ("Menu objects")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleSreenLoadMenu;

        [Header ("Pop-up objects")]
        [SerializeField] GameObject noFreeCharacterSlotsPopUp;

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

        public void StartNewGame()
        {

            WorldSaveManager.instance.AttemptToCreateNewGame();
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

        public void DisplayNoFreeCharacterSlotsPopUp()
        {
            noFreeCharacterSlotsPopUp.SetActive(true);
        }

        public void CloseNoFreeCharacterSlotsPopUp()
        {
            Debug.Log("Called");
            noFreeCharacterSlotsPopUp.SetActive(false);
        }
    }
}
    