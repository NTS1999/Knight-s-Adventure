using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Section4
{
    public class HPbarController : MonoBehaviour
    {
        [SerializeField] private Slider health;
        [SerializeField] private Text curHP;
        [SerializeField] private Text maxHP;

        public void SetMaxHealth(int maxhp)
        {
            health.maxValue = maxhp;
            health.value = maxhp;
            maxHP.text = "/ " + maxhp;
            curHP.text = " " + maxhp;
        }

        public void SetHealth(int hp)
        {
            health.value = hp;
            curHP.text = " " + hp;
        }
    }
}

