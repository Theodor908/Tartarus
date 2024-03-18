using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tartarus
{
    public class WorldSaveManager : MonoBehaviour
    {

        public static WorldSaveManager instance;
        public PlayerManager playerManager;

        [Header("SAVE/LOAD")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;

        [Header ("Save file data wirter")]
        private SaveFile saveFileDataWriter;

        [Header("Current character data")]
        public CharacterSlot currentCharacterSlot;
        public CharacterSaveData currentCharacterData;
        private string fileName;

        [Header ("Character slots")]
        public CharacterSaveData[] characterSlots = new CharacterSaveData[10];

        [Header("World Scene Index")]
        [SerializeField] int worldSceneIndex = 1;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else // Only one instance of WorldSaveManager is allowed
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            LoadAllCharacterSlots();
        }

        private void Update()
        {
            if(saveGame)
            {
                SaveGame();
                saveGame = false;
            }

            if(loadGame)
            {
                LoadGame();
                loadGame = false;
            }
        }

        public string DecideFileNameBasedOnCharacterSlotUsed(CharacterSlot characterSlot)
        {
            string fileName = "";
            for(int i = 0; i < characterSlots.Length; i++)
            {
                if(characterSlot == (CharacterSlot)i)
                {
                    fileName = "CharacterSlot_" + i;
                    Debug.Log("File name: " + fileName);
                    break;

                }
            }
            return fileName;
        }

        public void  AttemptToCreateNewGame()
        {

            saveFileDataWriter = new SaveFile();
            saveFileDataWriter.saveDataPath = Application.persistentDataPath; // Works on all
            // Verify if we can create a new save file

            for (int i = 0; i < characterSlots.Length; i++)
            {
                saveFileDataWriter.saveFileName = DecideFileNameBasedOnCharacterSlotUsed((CharacterSlot)i);
               
                if(!saveFileDataWriter.CheckForSaveFile())
                {
                    currentCharacterSlot = (CharacterSlot)i;
                    currentCharacterData = new CharacterSaveData();
                    StartCoroutine(LoadWorldScene());
                    return;
                }

            }

            // No free slots

            TitleScreenManager.instance.DisplayNoFreeCharacterSlotsPopUp();

        }

        public void LoadGame()
        {
            fileName = DecideFileNameBasedOnCharacterSlotUsed(currentCharacterSlot);
            saveFileDataWriter = new SaveFile();
            saveFileDataWriter.saveDataPath = Application.persistentDataPath; // Works on all 
            saveFileDataWriter.saveFileName = fileName;
            currentCharacterData = saveFileDataWriter.LoadSaveFile();
            StartCoroutine(LoadWorldScene());
        }

        public void SaveGame()
        {
            fileName = DecideFileNameBasedOnCharacterSlotUsed(currentCharacterSlot);
            saveFileDataWriter = new SaveFile();
            saveFileDataWriter.saveDataPath = Application.persistentDataPath; // Works on all
            saveFileDataWriter.saveFileName = fileName;

            // Need to pass the player info
            playerManager.SaveGameToCurrentCharacterData(ref currentCharacterData);


            saveFileDataWriter.CreateNewSaveFile(currentCharacterData);
        }

        public void DeleteGame(CharacterSlot characterSlot)
        {
            saveFileDataWriter = new SaveFile();
            saveFileDataWriter.saveDataPath = Application.persistentDataPath; // Works on all
            saveFileDataWriter.saveFileName = DecideFileNameBasedOnCharacterSlotUsed(characterSlot);    
            saveFileDataWriter.DeleteSaveFile();
        }

        // Load everything when starting the game

        private void LoadAllCharacterSlots()
        {
            saveFileDataWriter = new SaveFile();
            saveFileDataWriter.saveDataPath = Application.persistentDataPath;

            for(int i = 0; i < characterSlots.Length; i++)
            {
                saveFileDataWriter.saveFileName = DecideFileNameBasedOnCharacterSlotUsed((CharacterSlot)i);
                if(saveFileDataWriter.CheckForSaveFile())
                {
                    Debug.Log("Loading character slot: " + i);
                    characterSlots[i] = saveFileDataWriter.LoadSaveFile();
                } 
            }

        }

        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
            while(!loadOperation.isDone)
            {
                yield return null;
            }
            playerManager.LoadGameFromCurrentCharacterData(ref currentCharacterData);
            yield return null;
        }

    }
}
