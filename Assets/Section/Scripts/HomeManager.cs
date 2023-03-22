using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Section4
{
    public enum HomeState
    {
        Home
    }

    public class HomeManager : MonoBehaviour
    {
        [SerializeField] private HomePanel homePanel;

        private HomeState homeState;

        public void Start()
        {
            homePanel.gameObject.SetActive(false);
            SetState(HomeState.Home);
        }

        private void SetState(HomeState state)
        {
            homeState = state;
            homePanel.gameObject.SetActive(homeState == HomeState.Home);
        }

        public bool IsActive()
        {
            return homeState == HomeState.Home;
        }

        public void Play()
        {
            SceneManager.LoadScene("LevelSelection");
        }

        public void DeleteData()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}