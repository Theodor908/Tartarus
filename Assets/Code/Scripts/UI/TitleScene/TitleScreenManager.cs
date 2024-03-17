using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tartarus
{
    public class TitleScreenManager : MonoBehaviour
    {
        public void StartNewGame()
        {
            StartCoroutine(WorldSaveManager.instance.LoadWorldScene());
        }
    }
}
    