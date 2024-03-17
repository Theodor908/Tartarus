using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tartarus
{
    public class WorldSaveManager : MonoBehaviour
    {

        public static WorldSaveManager instance;
        [SerializeField] PlayerManager playerManager;

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

        public void DecideFileNameBasedOnCharacterSlotUsed()
        {
            for(int i = 0; i < characterSlots.Length; i++)
            {
                if(currentCharacterSlot == (CharacterSlot)i)
                {
                    fileName = "CharacterSlot_" + i;
                    Debug.Log("File name: " + fileName);
                    break;

                }
            }
        }

        public void  CreateNewGame()
        {
            DecideFileNameBasedOnCharacterSlotUsed();
            currentCharacterData = new CharacterSaveData();

        }

        public void LoadGame()
        {
            DecideFileNameBasedOnCharacterSlotUsed();
            saveFileDataWriter = new SaveFile();
            saveFileDataWriter.saveDataPath = Application.persistentDataPath; // Works on all 
            saveFileDataWriter.saveFileName = fileName;
            currentCharacterData = saveFileDataWriter.LoadSaveFile();

            StartCoroutine(LoadWorldScene());
        }

        public void SaveGame()
        {
            DecideFileNameBasedOnCharacterSlotUsed();
            saveFileDataWriter = new SaveFile();
            saveFileDataWriter.saveDataPath = Application.persistentDataPath; // Works on all
            saveFileDataWriter.saveFileName = fileName;

            // Need to pass the player info
            playerManager.SaveGameToCurrentCharacterData(ref currentCharacterData);


            saveFileDataWriter.CreateNewSaveFile(currentCharacterData);
        }

        public IEnumerator LoadWorldScene()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
            yield return null;
        }

    }
}
