using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tartarus
{
    public class WorldSaveManager : MonoBehaviour
    {
        public static WorldSaveManager instance;

        [SerializeField]
        int worldSceneIndex = 1;

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

        public void Strart()
        {
            DontDestroyOnLoad(gameObject);
        }

        public IEnumerator LoadNewGame()
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);
            yield return null;
        }

    }
}
