using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DuRound
{
    public class UpdateMabelUI : MonoBehaviour
    {
        private Image m_Mabel, m_Thomas, m_Dagger;
        private int health { get; set; } = 0;
        public int maxHealth = 3;
        public static UpdateMabelUI instance;
        public Sprite m_youngMabel, m_midMabel, m_oldMabel;
        private void Awake()
        {
            if(instance == null )
                instance = this;

            m_Mabel = transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            m_Thomas = transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
            m_Dagger = transform.GetChild(0).transform.GetChild(2).GetComponent<Image>();
            health = 0; 
        }
        public async void UpdateHealthMabel()
        {

            if (health >= maxHealth)
            {
                //TODO game over
                await Fade.instance.StartFade(true);
                var text = Fade.instance.gameObject.GetComponentInChildren<TextMeshProUGUI>();
                text.enabled = true;
                text.text = "Mabel to Old to continue this escape.";

                return;
            }
            health++;
            if (health <= maxHealth) 
            {
                switch(health) 
                {
                    case 0:
                        m_Mabel.sprite = m_youngMabel;
                        health = 1;
                        break;
                    case 1:
                        m_Mabel.sprite = m_midMabel;
                        health = 2;
                        break;
                    case 2:
                        m_Mabel.sprite = m_oldMabel;
                        health = 3;
                        break;
                }
            }
        }
        public void UpdateThomas(bool condition) { m_Thomas.gameObject.SetActive(condition);}
        public void UpdateDagger(bool condition) { m_Dagger.gameObject.SetActive(condition); }
    }
}
