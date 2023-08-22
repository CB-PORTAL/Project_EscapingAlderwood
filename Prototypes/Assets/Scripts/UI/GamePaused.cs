using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DuRound
{
    public class GamePaused : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = transform.GetChild(2).GetComponent<CanvasGroup>();
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        //set from button
        public void PauseStatus()
        {
            var time = Time.timeScale;
            if (time == 0)
            {
                Time.timeScale = 1;
                _canvasGroup.alpha = 0;
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
            }
            else if (time == 1)
            {
                Time.timeScale = 0;
                _canvasGroup.alpha = 1;
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            }
        }
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
