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
        public async Task<Task> StartFade(bool condition)
        {
            if (condition)
            {
                var cg = m_canvasGroup.transform.GetChild(0).GetComponent<CanvasGroup>();
                cg.alpha = 0;
                cg.interactable = false;
                cg.blocksRaycasts = false;
                m_canvasGroup.alpha = 1f;
                m_canvasGroup.interactable = true;
                m_canvasGroup.blocksRaycasts = true;

                await FadingImageIn();
                await Task.Delay(2000);
                return Task.CompletedTask;
                

            }
            else
            {
                await Task.Delay(5000);
                m_canvasGroup.alpha = 0;
                m_canvasGroup.interactable = false;
                m_canvasGroup.blocksRaycasts = false;

                await FadingImageOut();
                var cg = m_canvasGroup.transform.GetChild(0).GetComponent<CanvasGroup>();
                cg.alpha = 1;
                cg.interactable = true;
                cg.blocksRaycasts = true;

                return Task.CompletedTask;
            }
        }
        private async Task<Task> FadingImageIn()
        {
            var increment = 0f;
            while (increment <= 1f)
            {
                m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, increment);
                increment += 0.5f * Time.deltaTime;
                
                if (increment >= 1f)
                {
                    m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, 1);
                    return Task.CompletedTask;
                }
                await Task.Yield();
            }
            return Task.CompletedTask;
        }
        private async Task<Task> FadingImageOut()
        {
            var increment = 1f;
            while (increment <= 0f)
            {
                m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, increment);
                increment -= 0.5f  * Time.deltaTime;

                if (increment == 0)
                {
                    m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, 0);
                    return Task.CompletedTask;
                }
            }
            await Task.Yield();
            return Task.CompletedTask;
        }
    }
}
