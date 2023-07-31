using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace DuRound
{
    public class Fade : MonoBehaviour
    {
        public static Fade instance;

        private Image m_image;
        private CanvasGroup m_canvasGroup;
        private void Awake()
        {
            if (instance == null) instance = this;

            m_image = GetComponent<Image>();
            m_canvasGroup = transform.parent.GetComponent<CanvasGroup>();
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public async Task StartFade(bool condition)
        {
            if (condition)
            {
                m_canvasGroup.alpha = 1f;
                m_canvasGroup.interactable = true;
                m_canvasGroup.blocksRaycasts = true;
                await FadingImageIn();
            }
            else
            {
                await FadingImageOut();
                m_canvasGroup.alpha = 0;
                m_canvasGroup.interactable = false;
                m_canvasGroup.blocksRaycasts = false;
            }
        }
        private  Task FadingImageIn()
        {
            var increment = 0f;
            while (increment != 1)
            {
                m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, increment);
                increment += 0.1f * Time.deltaTime;
                Task.Yield();
            }
            return Task.CompletedTask;
        }
        private Task FadingImageOut()
        {
            var increment = 1f;
            while (increment != 0)
            {
                m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, increment);
                increment -= 0.1f * Time.deltaTime;
                Task.Yield();
            }
            return Task.CompletedTask;
        }
    }
}
