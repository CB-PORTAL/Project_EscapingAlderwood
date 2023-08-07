using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DuRound
{
    public class UpdateMabelUI : MonoBehaviour
    {
        private Image m_Mabel, m_Thomas, m_Dagger,m_Indicator;
        public Image Dagger { get { return m_Dagger; } }
        private int health { get; set; } = 0;
        public int maxHealth = 3;
        public int _health { get { return health; } }
        public static UpdateMabelUI instance;
        public Sprite m_youngMabel, m_midMabel, m_oldMabel;

        private Mabel _Mabel;
        private Thomas _Thomas;
        [SerializeField]
        private float adjustCold, adjustWarm, adjustHot;
        private void Awake()
        {
            if(instance == null )
                instance = this;
            _Mabel = GameObject.FindWithTag("Mabel").GetComponent<Mabel>();
            _Thomas = GameObject.FindWithTag("Thomas").GetComponent<Thomas>();
            m_Mabel = transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();
            m_Thomas = transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
            m_Dagger = transform.GetChild(0).transform.GetChild(2).GetComponent<Image>();
            m_Indicator = transform.GetChild(0).transform.GetChild(3).GetComponent<Image>();
            health = 1; 
        }
        private void Update()
        {
            IndicatorUpdate();
        }
        public  void UpdateHealthMabel()
        {
            if (health < maxHealth) 
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
        private void IndicatorUpdate()
        {
            var distanceWithThomas = Vector2.Distance(_Mabel.transform.position, _Thomas.transform.position);
            if (distanceWithThomas >= adjustCold)
            {
                m_Indicator.color = new Color(0, 0.2f, 1);
            }
            else if (distanceWithThomas <= adjustWarm  && distanceWithThomas > adjustHot)
            {

                var distanceWarmColor = distanceWithThomas - adjustWarm;
                var newColor = Mathf.Abs(distanceWarmColor / adjustWarm);
                m_Indicator.color = new Color(0, 0.2f, newColor);

            }
            else if (distanceWithThomas <= adjustHot)
            {

                var distanceHotColor = distanceWithThomas - adjustHot;
                var newColor = Mathf.Abs(distanceHotColor / adjustHot);
                m_Indicator.color = new Color(newColor, 0.2f, 0);
            }
        }
        public void UpdateThomas(bool condition) { m_Thomas.gameObject.SetActive(condition);}
        public void UpdateDagger(bool condition) { m_Dagger.gameObject.SetActive(condition); }
    }
}
