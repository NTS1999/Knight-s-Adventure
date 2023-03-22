using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

namespace Section4
{
    public class GameplayPanel : MonoBehaviour
    {
        public void BtnPause_Pressed()
        {
            GamePlayManager.Instance.Pause();
        }
    }
}