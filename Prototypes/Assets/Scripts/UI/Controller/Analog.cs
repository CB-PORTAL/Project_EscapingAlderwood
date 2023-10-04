
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
        private GameObject tempTouchMid { get; set; }
        private RectTransform touchMidRect { get; set; }
        private Vector2 touchMidOriginalPos { get; set; }
        private float touchMidDeltaX { get; set; }
        private float touchMidDeltaY { get; set; }

        private Vector2 leftUpper { get; set; }
        private Vector2 leftLower { get; set; }
        private Vector2 rightUpper { get; set; }
        private Vector2 rightLower { get; set; }
        private Vector2 upperLeft { get; set; }
        private Vector2 upperRight { get; set; }
        private Vector2 lowerLeft { get; set; }
        private Vector2 lowerRight { get; set; }

        Vector2 screenMid { get; set; }
        private bool currentOrientation { get; set; } = false;
        private void Awake()
        {
            mMabel = GameObject.FindWithTag("Mabel").GetComponent<Mabel>();


            //touchMidRect = touchMid.GetComponent<RectTransform>();
            // var screenM = Camera.main.WorldToScreenPoint(touchMid.transform.position);
            // Debug.LogWarning(screenM);


        }
        // Start is called before the first frame update
        void Start()
        {
            touchMid = transform.GetChild(0).gameObject;
            tempTouchMid = transform.GetChild(1).gameObject;
            touchMidOriginalPos = tempTouchMid.transform.position;
            touchMidDeltaX = touchMidOriginalPos.x / 2f;
            touchMidDeltaY = touchMidOriginalPos.y / 2f;
        }
        private void ScreenOrientationUpdate()
        {
            if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
            {
                touchMidOriginalPos = tempTouchMid.transform.position;
            }
            else if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
            {
                touchMidOriginalPos = tempTouchMid.transform.position;
            }
        }



            // Update is called once per frame
        void Update()
        {
            ScreenOrientationUpdate();
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
            if (touch.Count > 0)
            {
                if (mMabel.isAnalog == 1)
                {
                    foreach (var t in touch)
                    {
                        var screenPos = t.screen.position.value;
                        Vector2 worldScreen = Camera.main.ScreenToWorldPoint(screenPos);
                        touchMid.transform.position = worldScreen;
                        var analogLeft = touchMid.transform.position;
                        //left side
                        var UpperLeft0 = touchMidOriginalPos.y + 0.10f;
                        var UpperLeft1 = touchMidOriginalPos.y + 0.20f;
                        var DownLeft0 = touchMidOriginalPos.y - 0.10f;
                        var DownLeft1 = touchMidOriginalPos.y - 0.20f;
                        // Debug.Log(leftSideUpperMax + " left side uppermax " + leftSideUpperMin + " left side upper min");
                        if (analogLeft.x <= touchMidOriginalPos.x && analogLeft.y <= UpperLeft0 && analogLeft.y >= touchMidOriginalPos.y ||
                            analogLeft.x <= touchMidOriginalPos.x && analogLeft.y <= touchMidOriginalPos.y && analogLeft.y >= DownLeft0)
                        {
                            var newPos = new Vector2(-1, 0);
                            mMabel.SetMovement(newPos);
                        }
                        else if (analogLeft.x >= touchMidOriginalPos.x && analogLeft.y <= UpperLeft0 && analogLeft.y >= touchMidOriginalPos.y ||
                             analogLeft.x >= touchMidOriginalPos.x && analogLeft.y <= touchMidOriginalPos.y && analogLeft.y >= DownLeft0)
                        {
                            var newPos = new Vector2(1, 0);
                            mMabel.SetMovement(newPos);
                        }
                        else if (analogLeft.y <= touchMidOriginalPos.y && analogLeft.x <= touchMidOriginalPos.x + 0.10f && analogLeft.x >= touchMidOriginalPos.x ||
                            analogLeft.y <= touchMidOriginalPos.y && analogLeft.x <= touchMidOriginalPos.x && analogLeft.x >= touchMidOriginalPos.x - 0.10f)
                        {
                            var newPos = new Vector2(0, -1);
                            mMabel.SetMovement(newPos);
                        }
                        else if (analogLeft.y >= touchMidOriginalPos.y && analogLeft.x <= touchMidOriginalPos.x + 0.10f && analogLeft.x >= touchMidOriginalPos.x ||
                            analogLeft.y >= touchMidOriginalPos.y && analogLeft.x >= touchMidOriginalPos.x - 0.10f && analogLeft.x <= touchMidOriginalPos.x)
                        {
                            var newPos = new Vector2(0, 1);
                            mMabel.SetMovement(newPos);
                        }
                       // else
                       // {
                       //     var newPos = new Vector2(0, -1);
                       //     mMabel.SetMovement(newPos);
                       // }
                    }
                }
                if (mMabel.isAnalog == 0)
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
                if (mMabel.isAnalog == -1)
                {
                    touchMid.transform.position = touchMidOriginalPos;
                    mMabel.SetMovement(Vector2.zero);
                }

            }
        }
        
    }
}
