using DuRound;
using DuRound.MiniGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

namespace DuRound
{
    public class Guard : Mabel
    {
        private Transform [] movePoints;
        public Transform parentMovePoint;
        // private Transform [] exitPoints { get; set; }
        private int moveIncrement { get; set; } = 0;
        public bool isMoving { get; set; } = true;
        private bool shouldDestroy { get; set; } = false;
        private bool shouldMoveBackward = false;
        private Vector2 m_lastPosition, m_currentPosition;




        private Mabel m_Mabel { get; set; }
        protected override void Awake()
        {
            base.Awake();
            var movePointLength = parentMovePoint.childCount;
            movePoints = new Transform [movePointLength];
            for (int p = 0; p < movePointLength; p++)
            {
                movePoints [p] = parentMovePoint.GetChild(p);
            }


            m_Mabel = GameObject.FindWithTag("Mabel").GetComponent<Mabel>();
        }
        // Start is called before the first frame update
        public Task Initialize()
        {
            isCollide = false;
            moveIncrement = 0;
            isMoving = true;
            shouldDestroy = false;
            m_animator?.SetBool("isMove", true);
            return Task.CompletedTask;
        }
        //private void CheckHasThomas(bool condition)
        //{
        //    //TODO guard has thomas UI stuff
        //}
        public bool isDebug = false;
        // Update is called once per frame
        protected override void Update()
        {
            if (!shouldDestroy)
            {
                if ((int)m_currentPosition.x > (int)m_lastPosition.x)
                {
                    if (!m_animator.GetBool("isMove"))
                        m_animator.SetBool("isMove", true);
                    m_animator.SetFloat("MoveX", 1);
                    m_animator.SetFloat("MoveY", 0);
                    m_animator.SetFloat("IdleY", 0);
                    m_animator.SetFloat("IdleX", 0);
                }
                else if ((int)m_currentPosition.x < (int)m_lastPosition.x)
                {
                    if (!m_animator.GetBool("isMove"))
                        m_animator.SetBool("isMove", true);
                    m_animator.SetFloat("MoveX", -1);
                    m_animator.SetFloat("MoveY", 0);
                    m_animator.SetFloat("IdleY", 0);
                    m_animator.SetFloat("IdleX", 0);
                }
                else if ((int)m_currentPosition.y > (int)m_lastPosition.y)
                {
                    if (!m_animator.GetBool("isMove"))
                        m_animator.SetBool("isMove", true);
                    m_animator.SetFloat("MoveX", 0);
                    m_animator.SetFloat("MoveY", 1);
                    m_animator.SetFloat("IdleY", 0);
                    m_animator.SetFloat("IdleX", 0);
                }
                else if ((int)m_currentPosition.y < (int)m_lastPosition.y)
                {
                    if (!m_animator.GetBool("isMove"))
                        m_animator.SetBool("isMove", true);
                    m_animator.SetFloat("MoveX", 0);
                    m_animator.SetFloat("MoveY", -1);
                    m_animator.SetFloat("IdleY", 0);
                    m_animator.SetFloat("IdleX", 0);
                }
            }
            // else if(m_currentPosition == m_lastPosition)
            // {
            //     //TODO but the guard is always move no stop
            //     m_animator.SetBool("isMove", false);
            //     m_animator.SetFloat("MoveX", 0);
            //     m_animator.SetFloat("IdleY", 1);
            // }
            //if (lastIncrement != distanceIncrement)
            //{
            //    lastIncrement = distanceIncrement;
            //}
            CheckForDistance();
        }
        // private int distanceIncrement;
        private bool m_foundMabel = false;
        private List<Vector2> pathTrailAfterMabel = new List<Vector2>();
        private Vector2 m_lastPositionBeforeTrail = new Vector2();
        public float guardLineSight = 2f;
        private int lastIncrement { get; set; } = 0;
        private async void CheckForDistance()
        {

            var distance = Vector2.Distance(m_rigidBody2D.transform.position, m_Mabel.transform.position);
            if (distance <= guardLineSight)
            {
                //distance Guard can see Mabel
                //guard check front,left,right, down tile with mabel position
                //tile distance two position ahead after mabel
                //  if (!m_foundMabel)
                // {
                //m_foundMabel = true;
                isMoving = false;

                //var found = mapTilePoints [guardPos];


                // await MovePos();
                //  }


                // m_lastPositionBeforeTrail = m_Mabel.currentPosition;

                //shouldDestroy = true;
                //if (!m_Pursue)
                //{
                //    m_Mabel.MabelBeingSee(true);
                //    m_Pursue = true;
                //
                //    await StartPursuing();
                //}

            }
        }
        private string firstTaskMove = "null";
        private string secondTaskMove = "null";
        private string moveLeft = "moveLeft";
        private string moveRight = "moveRight";
        private string moveUp = "moveUp";
        private string moveDown = "moveDown";
        private async Task<Task> CheckMinusXPosition()
        {
            var guardPos = ConvertIntoInteger(m_rigidBody2D.position);
            var mPos = ConvertIntoInteger(m_Mabel.currentPath);
            var distanceBetween = guardPos - mPos;
            if (distanceBetween.x >= 2 || distanceBetween.y >= 2)
            {
                return Task.CompletedTask;
            }
            var movePlusPlus = 0; firstTaskMove = "null"; secondTaskMove = "null";
            //var checkPosition = m_rigidBody2D.position.x + distanceBetween.x;
            var canMove = mapTilePoints [guardPos];
            if (guardPos.x < mPos.x)
            {
                for (int g = (int)guardPos.x; g <= mPos.x; g++)
                {
                    canMove = mapTilePoints [guardPos];
                    if (canMove.Left)
                    {
                        movePlusPlus++;
                        if (movePlusPlus == 1)
                        {
                            firstTaskMove = moveLeft;
                        }
                        else if (movePlusPlus == guardLineSight)
                        {
                            secondTaskMove = moveLeft;
                        }
                        //var tempXMove = m_rigidBody2D.position.x + distanceBetween.x;
                        //var newMove = new Vector2(tempXMove, m_rigidBody2D.position.y);
                        //while (m_rigidBody2D.position != newMove)
                        //{
                        //    m_rigidBody2D.position = Vector2.MoveTowards(m_rigidBody2D.transform.position,
                        //    newMove, moveSpeed * Time.fixedDeltaTime);
                        //    if (m_rigidBody2D.position == newMove)
                        //    {
                        //        guardPos = ConvertIntoInteger(m_rigidBody2D.position);
                        //        canMove = mapTilePoints [guardPos];
                        //        break;
                        //    }
                        //}
                    }
                    else if (canMove.Upper)
                    {
                        //TODO
                        movePlusPlus++;
                        if (movePlusPlus == 1)
                        {
                            firstTaskMove = moveUp;
                        }
                        else if (movePlusPlus == guardLineSight)
                        {
                            secondTaskMove = moveUp;
                        }
                    }
                    else if (canMove.Down)
                    {
                        movePlusPlus++;
                        if (movePlusPlus == 1)
                        {
                            firstTaskMove = moveDown;
                        }
                        else if (movePlusPlus == guardLineSight)
                        {
                            secondTaskMove = moveDown;
                        }
                    }
                    if (distanceBetween.x == guardLineSight)
                    {
                        var tempX = m_rigidBody2D.position.x + distanceBetween.x;
                        var nePos = new Vector2(tempX, m_rigidBody2D.position.y);
                        //guardPos = tempX;
                    }

                    await Task.Yield();
                }
            }
            else if (guardPos.x > mPos.x)
            {
                for (int g = (int)guardPos.x; g >= mPos.x; g++)
                {
                    if (canMove.Right)
                    {
                        movePlusPlus++;
                        if (movePlusPlus == 1)
                        {
                            firstTaskMove = moveRight;
                        }
                        else if (movePlusPlus == guardLineSight)
                        {
                            secondTaskMove = moveRight;
                        }
                        //var tempXMove = m_rigidBody2D.position.x + distanceBetween.x;
                        //var newMove = new Vector2(tempXMove, m_rigidBody2D.position.y);
                        //while (m_rigidBody2D.position != newMove)
                        //{
                        //    m_rigidBody2D.position = Vector2.MoveTowards(m_rigidBody2D.transform.position,
                        //        newMove, moveSpeed * Time.fixedDeltaTime);
                        //    if (m_rigidBody2D.position == newMove)
                        //    {
                        //        guardPos = ConvertIntoInteger(m_rigidBody2D.position);
                        //        canMove = mapTilePoints [guardPos];
                        //        break;
                        //    }
                        //}
                    }
                    else if (canMove.Upper)
                    {
                        movePlusPlus++;
                        if (movePlusPlus == 1)
                        {
                            firstTaskMove = moveUp;
                        }
                        else if (movePlusPlus == guardLineSight)
                        {
                            secondTaskMove = moveUp;
                        }
                    }
                    else if (canMove.Down)
                    {
                        movePlusPlus++;
                        if (movePlusPlus == 1)
                        {
                            firstTaskMove = moveDown;
                        }
                        else if (movePlusPlus == guardLineSight)
                        {
                            secondTaskMove = moveDown;
                        }
                        await Task.Yield();
                    }
                }
                if (guardPos.y < mPos.y)
                {
                    for (int g = (int)guardPos.y; g <= mPos.y; g++)
                    {
                        if (canMove.Upper)
                        {
                            var tempYMove = m_rigidBody2D.position.y + distanceBetween.y;
                            var newMove = new Vector2(m_rigidBody2D.position.y, tempYMove);
                            while (m_rigidBody2D.position != newMove)
                            {
                                m_rigidBody2D.position = Vector2.MoveTowards(m_rigidBody2D.transform.position,
                                    newMove, moveSpeed * Time.fixedDeltaTime);
                                if (m_rigidBody2D.position == newMove)
                                {
                                    guardPos = ConvertIntoInteger(m_rigidBody2D.position);
                                    canMove = mapTilePoints [guardPos];
                                    break;
                                }
                            }
                        }
                        await Task.Yield();
                    }
                }
                else if (guardPos.y > mPos.y)
                {
                    for (int g = (int)guardPos.y; g >= mPos.y; g++)
                    {
                        if (canMove.Down)
                        {
                            var tempYMove = m_rigidBody2D.position.y + distanceBetween.y;
                            var newMove = new Vector2(m_rigidBody2D.position.y, tempYMove);
                            while (m_rigidBody2D.position != newMove)
                            {
                                m_rigidBody2D.position = Vector2.MoveTowards(m_rigidBody2D.transform.position,
                                    newMove, moveSpeed * Time.fixedDeltaTime);
                                if (m_rigidBody2D.position == newMove)
                                {
                                    guardPos = ConvertIntoInteger(m_rigidBody2D.position);
                                    break;
                                }
                            }
                        }
                        await Task.Yield();
                    }
                }

            }
            return Task.CompletedTask;
        }
    
        private void PredictMovement()
        {
            
        }
        private async Task MoveXStraight(Vector2 MabelPath)
        {
            var movePos = new Vector2(MabelPath.x, m_rigidBody2D.position.y);
            while (m_rigidBody2D.position != movePos)
            {
                if (m_rigidBody2D.position == movePos)
                {
                    break;
                }
                else
                {
                    m_rigidBody2D.position = Vector2.MoveTowards(m_rigidBody2D.transform.position, movePos, moveSpeed * Time.fixedDeltaTime);
                }
                await Task.Yield();
            }
        }
        private async Task MoveYStraight(Vector2 MabelPath)
        {
            var movePos = new Vector2(m_rigidBody2D.position.x, MabelPath.y);
            while(m_rigidBody2D.position != movePos) 
            {
                if (m_rigidBody2D.position == movePos)
                {
                    break;
                }
                else
                {
                    m_rigidBody2D.position = Vector2.MoveTowards(m_rigidBody2D.transform.position, movePos, moveSpeed * Time.fixedDeltaTime);
                }
                await Task.Yield();
            }
        }
        private async Task MoveYAxis(Vector2 minusStraightDistance, Vector2 MabelPath)
        {
            var absX = Mathf.Abs(minusStraightDistance.x);
            var absY = Mathf.Abs(minusStraightDistance.y);
            var guardPos = ConvertIntoInteger(m_rigidBody2D.position);
            var found = mapTilePoints [guardPos];
            if (absY <= 2 || absX <= 2)
            {
                if (minusStraightDistance.y < 0)
                {
                    if (found.Down)
                    {
                        Vector2 yPos = new Vector2(m_rigidBody2D.position.x, minusStraightDistance.y);
                        while (m_rigidBody2D.position != yPos)
                        {
                            if (m_rigidBody2D.position == yPos)
                            {
                                if (minusStraightDistance.x != 0)
                                {
                                   // await MoveXPos(minusStraightDistance);
                                }
                                break;

                            }
                            else
                            {
                                m_rigidBody2D.position = Vector2.MoveTowards(m_rigidBody2D.transform.position, yPos, moveSpeed * Time.fixedDeltaTime);
                            }
                            await Task.Yield();
                        }
                    }
                }
                else if (minusStraightDistance.y > 0)
                {
                    if (found.Upper)
                    {
                        Vector2 yPos = new Vector2(m_rigidBody2D.position.x, minusStraightDistance.y);
                        while (m_rigidBody2D.position != yPos)
                        {
                            if (m_rigidBody2D.position == yPos)
                            {
                                if (minusStraightDistance.x != 0)
                                {
                                   // await MoveXPos(minusStraightDistance);
                                }
                                break;
                            }
                            else
                            {
                                m_rigidBody2D.position = Vector2.MoveTowards(m_rigidBody2D.transform.position, yPos, moveSpeed * Time.fixedDeltaTime);
                            }
                            await Task.Yield();
                        }
                    }
                }
            }
        }
        private async Task  PursueMabel()
        {

            await Task.Yield();
        }
        private Dictionary<Vector2,TilePoints>mapTilePoints = new Dictionary<Vector2,TilePoints>(); 
        public void SentMapData(Dictionary<Vector2,TilePoints> mapTile)
        {
            mapTilePoints = mapTile;
        }
       // private async Task StartPursuing()
       // {
       //     while (m_Pursue)
       //     {
       //        var pursueTrail = m_Mabel.GetListTrailPath;
       //        var distanceIncrement = m_Mabel.trailIncrement;
       //        if (distanceIncrement >= 0 && distanceIncrement < m_Mabel.maxTrail)
       //        {
       //             Debug.Log("DISTANCE INCREMENT");
       //             if (pursueTrail.Count > 0)
       //             {
       //                 Debug.Log("pursueTrail " + pursueTrail [distanceIncrement]);
       //                 var movePos = pursueTrail [distanceIncrement];
       //                 if (m_rigidBody2D.position != movePos)
       //                 {
       //                     m_lastPosition = m_rigidBody2D.position;
       //                     m_rigidBody2D.position =
       //                         Vector2.MoveTowards(m_rigidBody2D.transform.position, movePos, moveSpeed * Time.fixedDeltaTime);
       //
       //                     m_currentPosition = m_rigidBody2D.position;
       //                 }
       //                 else
       //                 {
       //                     pathTrailAfterMabel.Add(movePos);
       //                     m_Mabel.trailIncrement++;
       //                 }
       //             }
       //         }
       //         await Task.Yield();
       //     }
       // }
        //private bool pursueMabel { get; set; } = false;
        //private bool currentlyPursue { get; set; } = false;
        //private async Task<Task> PursueMabel()
        //{
        //    while (pursueMabel)
        //    {
        //        isCollide = false; shouldDestroy = true;
        //        shouldMoveBackward = false;
        //        m_rigidBody2D.transform.position = Vector2.MoveTowards(m_rigidBody2D.transform.position, m_Mabel.transform.position, moveSpeed * Time.fixedDeltaTime);
        //        await Task.Yield();
        //    }
        //    currentlyPursue = false;
        //    return Task.CompletedTask;
        //}
        protected override void FixedUpdate()
        {
           // CheckForDistance();
        }
        public override void StartFade()
        {
        }
        protected override void Start()
        {
        }
        private void OnDestroy()
        {
            m_foundMabel = false;
            shouldDestroy = true;
            pathTrailAfterMabel.Clear();
            //pursueMabel = false;
        }
        public async void GuardMoveForwards()
        {
            shouldDestroy = false;
            while (!isCollide && !shouldDestroy)
            {
                if (moveIncrement < movePoints.Length)
                {
                    var movePos = movePoints [moveIncrement]; 
                    if (m_rigidBody2D.position != (Vector2)movePos.position)
                    {
                        if (isMoving)
                        {
                            m_lastPosition = m_rigidBody2D.position;
                            m_rigidBody2D.position = 
                                Vector2.MoveTowards(m_rigidBody2D.transform.position, movePos.position, moveSpeed * Time.fixedDeltaTime);

                            m_currentPosition = m_rigidBody2D.position;

                        }
                    }
                    else
                    {
                        moveIncrement++;
                    }
                }
                else
                {
                    isCollide = true;
                    shouldDestroy = true;
                    m_animator.SetBool("isMove", false);
                    m_animator.SetFloat("MoveY", 0);
                    m_animator.SetFloat("MoveX", 0);
                    m_animator.SetFloat("IdleY", 1);
                    m_animator.SetFloat("IdleX", 0);
                    await Task.Delay(10000);
                    shouldMoveBackward = true;
                    shouldDestroy = false;
                    moveIncrement--;
                    GuardMoveBackwards();             
                    break;
                }
                await Task.Yield();

            }
        }
        private async void GuardMoveBackwards()
        {
            while (shouldMoveBackward && !shouldDestroy)
            {
                if (moveIncrement > -1)
                {
                    var movePos = movePoints [moveIncrement];
                    if (m_rigidBody2D.position != (Vector2)movePos.position)
                    {
                        if (isMoving)
                        {
                            m_lastPosition = m_rigidBody2D.position;
                            m_rigidBody2D.position =
                            Vector2.MoveTowards(m_rigidBody2D.transform.position, movePos.position, moveSpeed * Time.fixedDeltaTime);

                            m_currentPosition = m_rigidBody2D.position;
                        }
                    }
                    else
                    {
                        moveIncrement--;
                    }

                }
                else
                {
                    shouldMoveBackward = false;
                    shouldDestroy = true;
                    m_animator.SetBool("isMove", false);
                    m_animator.SetFloat("MoveY", 0);
                    m_animator.SetFloat("MoveX", 0);
                    m_animator.SetFloat("IdleY", 1);
                    m_animator.SetFloat("IdleX", 0);
                    await Task.Delay(10000);
                    shouldDestroy = false;
                    isCollide = false;
                    moveIncrement++;
                    GuardMoveForwards();
                    break;
                }
                await Task.Yield();
            }
        }
        private async void GuardMoveExit()
        {
            isCollide = true; shouldDestroy = true;
            shouldMoveBackward = false;
            await Task.Delay(1000);
            if (m_Mabel.disableMovement)
                m_Mabel.disableMovement = false;
            shouldDestroy = false;
            while (isCollide && !shouldDestroy) 
            {
                if (moveIncrement >= 0)
                {
                    var movePos = movePoints [moveIncrement]; 
                    if (m_rigidBody2D.position != (Vector2)movePos.position)
                    {
                        if (isMoving)
                        {
                            m_lastPosition = m_rigidBody2D.position;
                            m_rigidBody2D.position =
                            Vector2.MoveTowards(m_rigidBody2D.transform.position, movePos.position, moveSpeed * Time.fixedDeltaTime);

                            m_currentPosition = m_rigidBody2D.position;

                        }
                    }
                    else
                    {
                        moveIncrement--;
                    }
                    if (m_rigidBody2D.position == (Vector2)movePoints [0].position)
                    {
                        isCollide = false; shouldDestroy = true;shouldMoveBackward = false;
                        break;
                    }
                    await Task.Yield();
                }
            }
        }
        public override void AddThomas()
        {
            m_hasThomas = true;
        }
        public override void RemoveThomas()
        {
            m_hasThomas = false;
        }
        protected async void OnTriggerEnter2D(Collider2D collision)
        {
            
            if (collision.CompareTag("Mabel"))
            { 
                isCollide = true;
                var m_Mabel = collision.gameObject.GetComponent<Mabel>();
                if (this.hasThomas)
                {
                    if (m_Mabel.disableMovement)
                        return;
                    GuardController.instance.GuardAction(false);
                    this.m_hasThomas = false;
                    m_Mabel.disableMovement = true;

                    _miniCanvas.alpha = 1;
                    _miniCanvas.interactable = true;
                    _miniCanvas.blocksRaycasts = true;
                    var cgPanel = _miniCanvas.transform.GetChild(0).GetComponent<CanvasGroup>();
                    cgPanel.alpha = 1;
                    cgPanel.blocksRaycasts = true;
                    cgPanel.interactable = true;
                    _miniCanvas.transform.GetChild(0).GetChild(0).GetComponent<GuardWalk>().StartMove();
                    GuardController.instance.CurrentGuardHasThomas(this);
                    return;
                }
                if (m_Mabel.hasThomas)
                {
                    m_Mabel.disableMovement = true;
                    moveSpeed = 0.8f;
                    m_Mabel.RemoveThomas();
                    this.m_hasThomas = true;
                    moveIncrement--;
                    shouldMoveBackward = true;
                   // GuardMoveBackwards();
                    GuardMoveExit();
                }
                else
                {
                    if (m_Mabel.disableMovement)
                        return;
                    m_Mabel.disableMovement = true;
                    GuardController.instance.GuardAction(false);
                    _miniCanvas.alpha = 1;
                    var fadeIn = await Fade.instance.StartFade(true);
                    if (fadeIn.IsCompleted)
                    {

                        var text = _miniCanvas.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                        text.enabled = true;


                        if (UpdateMabelUI.instance._health != UpdateMabelUI.instance.maxHealth)
                        {
                            text.text = "The Guard Caught Mabel";
                            UpdateMabelUI.instance.UpdateHealthMabel();
                            GameManager.Instance.EnableThomas();
                            await Fade.instance.StartFade(false);
                            m_Mabel.ResetPosition();
                            _miniCanvas.alpha = 0;
                            text.enabled = false;
                            GuardController.instance.ResetAllGuard();
                            GuardController.instance.SetMovementSpeedAllGuard();

                        }
                        else
                        {
                            
                            text.text = "Mabel to Old to continue this escape.";
                        }

                    }
                }
            }
            else if (collision.CompareTag("Exit"))
            {
                if (this.hasThomas)
                {
                    m_Mabel.disableMovement = true;
                    shouldDestroy = true; shouldMoveBackward = false;
                    m_hasThomas = false;
                    GuardController.instance.GuardAction(false);
                    _miniCanvas.alpha = 1;
                    var fadeIn = await Fade.instance.StartFade(true);
                    if (fadeIn.IsCompleted)
                    {

                        var text = _miniCanvas.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                        text.enabled = true;


                        if (UpdateMabelUI.instance._health != UpdateMabelUI.instance.maxHealth)
                        {
                            text.text = "The Guard Has Exit and Lost Thomas";
                            UpdateMabelUI.instance.UpdateHealthMabel();
                            GameManager.Instance.EnableThomas();
                            await Fade.instance.StartFade(false);
                            m_Mabel.ResetPosition();
                            _miniCanvas.alpha = 0;
                            if (text != null)
                                text.enabled = false;
                            GuardController.instance.ResetAllGuard();
                            GuardController.instance.SetMovementSpeedAllGuard();

                        }
                        else
                        {
                            text.text = "Mabel to Old to continue this escape.";
                        }

                    }

                }
                
            }
        }
    
        public new void ResetPosition()
        {
            shouldDestroy = true; shouldMoveBackward = false; isCollide = false;
            m_rigidBody2D.position = (Vector2)movePoints [0].position;
            moveIncrement = 0;
        }

        public async void StopMoving()
        {
            isMoving = false;
            await Task.Delay(5000);
            isMoving = true;
        }
    }
}
