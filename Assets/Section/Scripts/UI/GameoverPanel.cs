using System.Collections;
using System.Collections.Generic;
using UnityEngine;  
using TMPro;
using UnityEngine.UI;

namespace Section4
{
    public class GameoverPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_TxtResult;
        [SerializeField] private GameObject nextLevel;

        public void DisplayResult(bool isWin)
        {
            if (isWin)
                m_TxtResult.text = "YOU WON";
            else
            {
                m_TxtResult.text = "GAME OVER";
                nextLevel.SetActive(false);
            }
                
        }

        public void BtnNextLevel_Pressed()
        {
            GamePlayManager.Instance.NextLevel();
        }

        public void BtnRestart_Pressed()
        {
            GamePlayManager.Instance.Restart();
        }
    }
}