using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace DuRound
{
    public class Instruction : MonoBehaviour
    {
        public static Instruction instance;
        public string AvoidingText,WeaponText;
        private TextMeshProUGUI m_Text { get; set; }
        private CanvasGroup m_Canvas { get; set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            m_Text = GetComponentInChildren<TextMeshProUGUI>();
            m_Canvas = GetComponent<CanvasGroup>();
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void EnableCanvas(bool condition)
        {
            if (condition)
            {
                m_Canvas.alpha = 1;
                m_Canvas.interactable = true;
                m_Canvas.blocksRaycasts = true;
            }
            else
            {
                m_Canvas.alpha = 0;
                m_Canvas.interactable = false;
                m_Canvas.blocksRaycasts = false;
            }
        }
        public void SetAvoidingGuardText()
        {
            m_Text.text = AvoidingText;
            EnableCanvas(true);
        }
        public void SetPickWeapon()
        {
            m_Text.text = WeaponText;
            EnableCanvas(true);
        }
    }
}
