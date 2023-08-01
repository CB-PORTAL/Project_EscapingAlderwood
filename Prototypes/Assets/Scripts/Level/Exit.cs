using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DuRound
{
    public class Exit : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        // Start is called before the first frame update
        void Start()
        {
            _canvasGroup = GameObject.Find("MiniGame").GetComponent<CanvasGroup>(); 
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Mabel"))
            {
                var Mabel = collision.GetComponent<Mabel>();
                if (Mabel.hasThomas)
                {
                    _canvasGroup.alpha = 1;
#pragma warning disable CS4014
                    Fade.instance.StartFade(true);
#pragma warning restore CS4014
                    var text = Fade.instance.gameObject.GetComponentInChildren<TextMeshProUGUI>();
                    text.text = "You Just Save Thomas and Leave Dungeon";
                    text.enabled = true;
                }
            }
        }
    }
}
