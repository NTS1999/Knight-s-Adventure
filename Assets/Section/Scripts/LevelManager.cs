using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Section4
{
    public enum LevelState
    {
        Chooselevel
    }

    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private LevelPanel levelPanel;
        public Button[] lvlButtons;

        private LevelState levelState;

        public void Start()
        {
            levelPanel.gameObject.SetActive(false);
            SetState(LevelState.Chooselevel);

            int levelAt = PlayerPrefs.GetInt("levelAt", 2);

            for (int i = 0; i < lvlButtons.Length; i++)
            {
                if (i + 2 > levelAt)
                    lvlButtons[i].interactable = false;
            }    
        }

        private void SetState(LevelState state)
        {
            levelState = state;
            levelPanel.gameObject.SetActive(levelState == LevelState.Chooselevel);
        }

        public bool IsActive()
        {
            return levelState == LevelState.Chooselevel;
        }
    }
}