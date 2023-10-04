using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DuRound
{
    public class UpdateMabelUI : MonoBehaviour
    {
        private Image m_Thomas, m_Dagger,m_Indicator;
        private GameObject hitPoint0, hitPoint1, hitPoint2;
        public Image Dagger { get { return m_Dagger; } }

        private int GetHit()
        {
            return _Mabel.m_hitPoints;
        }

        public int _health { get { return GetHit(); } }
        public static UpdateMabelUI instance;
       // public Sprite m_youngMabel, m_midMabel, m_oldMabel;

        private Mabel _Mabel;
        private Thomas _Thomas;
        [SerializeField]
        private float adjustCold, adjustWarm, adjustHot,normal;
        private void Awake()
        {
            if(instance == null )
                instance = this;
            _Mabel = GameObject.FindWithTag("Mabel").GetComponent<Mabel>();
            _Thomas = GameObject.FindWithTag("Thomas").GetComponent<Thomas>();
            var parentMabelHit = transform.GetChild(0).GetChild(0);
            hitPoint0 = parentMabelHit.GetChild(0).gameObject;
            hitPoint1 = parentMabelHit.GetChild(1).gameObject;
            hitPoint2 = parentMabelHit.GetChild(2).gameObject;
            m_Thomas = transform.GetChild(0).transform.GetChild(1).GetComponent<Image>();
            m_Dagger = GameObject.Find("Weapon").transform.GetChild(0).GetComponent<Image>();
            m_Indicator = transform.GetChild(0).transform.GetChild(3).GetComponent<Image>();
            hitPoint0.gameObject.SetActive(true);hitPoint1.gameObject.SetActive(true);hitPoint2.gameObject.SetActive(true);
        }
        private void Update()
        {
            if (m_Indicator.enabled)
            {
                IndicatorUpdate();
            }
        }
        public  void UpdateHealthMabel()
        {
            if (GetHit() >= 0) 
            {
                switch(GetHit()) 
                {
                    case 0:
                        hitPoint0.gameObject.SetActive(false);
                        break;
                    case 1:
                        hitPoint1.gameObject.SetActive(false);
                        break;
                    case 2:
                        hitPoint2.gameObject.SetActive(false);
                        break;
                }
            }
        }
        private void IndicatorUpdate()
        {
            var distanceWithThomas = Vector2.Distance(_Mabel.transform.position, _Thomas.transform.position);
            //Debug.Log(distanceWithThomas);
            if (distanceWithThomas >= normal)
            {
                m_Indicator.color = new Color(1, 1, 1);
            }
            else if (distanceWithThomas >= adjustCold && distanceWithThomas < normal)
            {
                m_Indicator.color = new Color(0, 0.2f, 1);
            }
            else if (distanceWithThomas <= adjustWarm && distanceWithThomas > adjustHot)
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
        public void UpdateThomas(bool condition) { m_Thomas.gameObject.SetActive(condition);m_Indicator.enabled = condition; if (!condition) UpdateIndicator(); }
        public void UpdateDagger(bool condition) { m_Dagger.GetComponent<Image>().enabled = condition; }// _Mabel.SetMabelHasDagger(true); }
        public void UpdateIndicator() { m_Indicator.enabled = false; }
    }
}
