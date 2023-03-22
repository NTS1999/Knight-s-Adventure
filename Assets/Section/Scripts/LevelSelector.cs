using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Section4
{
    public class LevelSelector : MonoBehaviour
    {
        public int level;

        public void OpenScene()
        {
            SceneManager.LoadScene("Level_" + level.ToString());
        }
    }
}