using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace DuRound
{
    public class Analog : MonoBehaviour
    {
        private Mabel mMabel { get; set; }
        private GameObject touchMid { get; set; }
        private RectTransform touchMidRect { get; set; }
        private Vector2 touchMidOriginalPos { get; set; }


        private void Awake()
        {
            mMabel = GameObject.FindWithTag("Mabel").GetComponent<Mabel>();

            //touchMid = transform.GetChild(0).gameObject;
            //touchMidRect = touchMid.GetComponent<RectTransform>();
            //touchMidOriginalPos = touchMidRect.transform.position;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var touch = Touch.activeTouches;
            if(touch.Count > 0 ) 
            {
                if (mMabel.isAnalog == 1)
               {

                    //var posDelta = touch [0].screen.delta.valueSizeInBytes;
                    foreach (var t in touch)
                    {
                        var deltaPos = t.delta.normalized;
                        if (deltaPos.x > 0.9f)
                        {
                            var newPos = new Vector2(deltaPos.x, 0);
                            mMabel.SetMovement(newPos);
                        }
                        else if (deltaPos.x < -0.9f)
                        {
                            var newPos = new Vector2(deltaPos.x, 0);
                            mMabel.SetMovement(newPos);
                        }
                        else if (deltaPos.y > 0.9f)
                        {
                            var newPos = new Vector2(0, deltaPos.y);
                            mMabel.SetMovement(newPos);
                        }
                        else if (deltaPos.y < -0.9f)
                        {
                            var newPos = new Vector2(0, deltaPos.y);
                            mMabel.SetMovement(newPos);
                        }

                    }
                }

            }
        }
    }
}
