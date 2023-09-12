using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace DuRound
{
    public class Analog : MonoBehaviour
    {
        private Mabel mMabel { get; set; }
        private GameObject touchMid { get; set; }
        private RectTransform touchMidRect { get; set; }
        private Vector2 touchMidOriginalPos { get; set; }
        private float touchMidDeltaX { get; set; }
        private float touchMidDeltaY { get; set; }

        private float leftUpper { get; set; }
        private float leftLower { get; set; }
        private float rightUpper { get; set; }
        private float rightLower { get; set; }
        private float upperLeft { get; set; }
        private float upperRight { get; set; }
        private float lowerLeft { get; set; }
        private float lowerRight { get; set; }


        private void Awake()
        {
            mMabel = GameObject.FindWithTag("Mabel").GetComponent<Mabel>();

            touchMid = transform.GetChild(0).gameObject;
            touchMidRect = touchMid.GetComponent<RectTransform>();
            touchMidOriginalPos = touchMid.transform.position;
            touchMidDeltaX = touchMidOriginalPos.x / 2f;
            touchMidDeltaY = touchMidOriginalPos.y / 2f;
        }
        // Start is called before the first frame update
        void Start()
        {

        }
        // Update is called once per frame
        void Update()
        {
            //var ts = Touchscreen.current;
            //if (mMabel.isAnalog == 1)
            //{
            //    if (ts.delta.left.value >0)
            //    {
            //        Debug.Log("left");
            //    }
            //    else if (ts.delta.right.value >0)
            //    {
            //        Debug.Log("right");
            //    }
            //    else if (ts.delta.up.value > 0)
            //    {
            //        Debug.Log("upp");
            //    }
            //    else if (ts.delta.down.value > 1)
            //    {
            //        Debug.Log("down");
            //    }
            //    //var temp = th.value;
            //    //if (th.value != Vector2.zero)
            //    //{
            //    //    if (th.right.value > 0)
            //    //    {
            //    //        var newPos = new Vector2(1, 0);
            //    //        mMabel.SetMovement(newPos);
            //    //    }
            //    //}
            //}
            //
            //return;
            //ENHANCED TOUCH SCREEN
            var touch = Touch.activeFingers;
            if(touch.Count > 0 ) 
            {
                if (mMabel.isAnalog == 1)
                {
                    foreach (var t in touch)
                    {
                        var screenPos = t.screen.position.value;
                        
                        touchMid.transform.position = screenPos;
     

                        leftUpper = touchMidOriginalPos.y + touchMidDeltaY;
                        leftLower = touchMidOriginalPos.y - touchMidDeltaY;
                        rightUpper = touchMidOriginalPos.y + touchMidDeltaY;
                        rightLower = touchMidOriginalPos.y - touchMidDeltaY;
                        upperLeft = touchMidOriginalPos.x - touchMidDeltaX;
                        upperRight = touchMidOriginalPos.x + touchMidDeltaX;
                        lowerLeft = touchMidOriginalPos.x - touchMidDeltaX;
                        lowerRight = touchMidOriginalPos.x + touchMidDeltaX;

                        if (screenPos.x < touchMidOriginalPos.x && screenPos.y < leftUpper && screenPos.y > leftLower)
                        {
                            var newPos = new Vector2(-1, 0);
                            mMabel.SetMovement(newPos);
                        }
                        else if (screenPos.x > touchMidOriginalPos.x && screenPos.y < rightUpper && screenPos.y > rightLower)
                        {
                            var newPos = new Vector2(1, 0);
                            mMabel.SetMovement(newPos);
                        }
                        else if (screenPos.y < touchMidOriginalPos.y && screenPos.x > lowerLeft && screenPos.x < lowerRight)
                        {
                            var newPos = new Vector2(0, -1);
                            mMabel.SetMovement(newPos);
                        }
                        else if (screenPos.y > touchMidOriginalPos.y && screenPos.x > upperLeft && screenPos.x < upperRight)
                        {
                            var newPos = new Vector2(0, 1);
                            mMabel.SetMovement(newPos);
                        }
                    }
                }
                else if (mMabel.isAnalog == 0)
                {
                    touchMid.transform.position = touchMidOriginalPos;
                    mMabel.SetMovement(Vector2.zero);
                    //foreach (var t in touch)
                    //{
                    //    var screenPos = t.screenPosition;
                    //    var cam = Camera.main.ScreenToWorldPoint(screenPos);
                    //    var cam2 = Camera.main.ScreenToWorldPoint(touchMidOriginalPos);
                    //    touchMid.transform.position = screenPos;
                    //}

                }
                else if (mMabel.isAnalog == -1)
                {
                    touchMid.transform.position = touchMidOriginalPos;
                    mMabel.SetMovement(Vector2.zero);
                }

            }
        }
    }
}
