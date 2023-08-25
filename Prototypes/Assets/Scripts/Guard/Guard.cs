using DuRound;
using DuRound.MiniGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;

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
        protected async override void Awake()
        {
            base.Awake();
            var movePointLength = parentMovePoint.childCount;
            movePoints = new Transform [movePointLength];
            for (int p = 0; p < movePointLength; p++)
            {
                movePoints [p] = parentMovePoint.GetChild(p);
            }


            m_Mabel = GameObject.FindWithTag("Mabel").GetComponent<Mabel>();

            await CheckForDistance();
        }
        // Start is called before the first frame update
        public Task Initialize()
        {
            isCollide = false;
            moveIncrement = 0;
            isMoving = true;
            shouldDestroy = false;
            m_animator?.SetBool("isMove", true);
            pathTrailAfterMabel.Clear();
            lastPositionAfterTrack.Clear();
            return Task.CompletedTask;
        }
        public void ResetMoveIncrement()
        {
            moveIncrement = 0;
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
            //     m_animator.SetBool("isMove", false);
            //     m_animator.SetFloat("MoveX", 0);
            //     m_animator.SetFloat("IdleY", 1);
            // }
            //if (lastIncrement != distanceIncrement)
            //{
            //    lastIncrement = distanceIncrement;
            //}
        }
        // private int distanceIncrement;
        private bool m_foundMabel = false;
        private List<Vector2> pathTrailAfterMabel = new List<Vector2>();
        private List<Vector2> lastPositionAfterTrack = new List<Vector2>();
        private Vector2 m_MabelTrailPosition = new Vector2();
        public float guardLineSight = 2f;
        private int lastIncrement { get; set; } = 0;
        private bool m_checkForDistance { get; set; } = true;
        private bool m_moveToMabel { get; set; } = true;
        private bool trailMoving { get; set; } = false;
        private async Task CheckForDistance()
        {
            while (m_checkForDistance)
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

                    //var found = mapTilePoints [guardPos];
                    if (!m_foundMabel)
                    {
                        m_foundMabel = true;
                        trailMoving = true;
                        var isComplete = CheckMinusXPosition();
                        if (isComplete.Result == true)
                        {
                            isMoving = false;
                            shouldDestroy = true;
                            //Debug.Log(firstMove);
                            //Debug.Log(secondMove);
                            //m_foundMabel = false;
                            if (firstMove != Vector2.zero && secondMove != Vector2.zero)
                            {
                                var tempBeforeFirst = ConvertIntoInteger(m_rigidBody2D.position);
                                lastPositionAfterTrack.Add(tempBeforeFirst);
                                Vector2 movePosition = firstMove;
                                while (m_moveToMabel)
                                {
                                    if (trailMoving)
                                    {
                                        //adding Mabel trail to guard memory
                                        var currentMabelPath = m_Mabel.currentPath;
                                        if (m_MabelTrailPosition == Vector2.zero || m_MabelTrailPosition != currentMabelPath)
                                        {
                                            m_MabelTrailPosition = currentMabelPath;
                                            pathTrailAfterMabel.Add(m_MabelTrailPosition);
                                        }
                                        if (movePosition != Vector2.zero)
                                        {
                                            if (m_rigidBody2D.position != movePosition)
                                            {
                                                m_rigidBody2D.position = Vector2.MoveTowards(m_rigidBody2D.position, movePosition, moveSpeed * Time.fixedDeltaTime);
                                            }
                                            else
                                            {
                                                var tempPos = ConvertIntoInteger(m_rigidBody2D.position);
                                                lastPositionAfterTrack.Add(tempPos);
                                                movePosition = secondMove;
                                            }
                                        }
                                        if (m_rigidBody2D.position == secondMove)
                                        {
                                            var tempPos = ConvertIntoInteger(m_rigidBody2D.position);
                                            lastPositionAfterTrack.Add(tempPos);
                                            movePosition = Vector2.zero;
                                        }
                                        if (movePosition == Vector2.zero)
                                        {
                                            if (pathTrailAfterMabel.Count > 0)
                                            {
                                                var newPosition = pathTrailAfterMabel [0];
                                                if (m_rigidBody2D.position != newPosition)
                                                {
                                                    m_rigidBody2D.position = Vector2.MoveTowards(m_rigidBody2D.position, newPosition, moveSpeed * Time.fixedDeltaTime);
                                                }
                                                else
                                                {
                                                    var tempPos = ConvertIntoInteger(m_rigidBody2D.position);
                                                    lastPositionAfterTrack.Add(tempPos);
                                                    pathTrailAfterMabel.RemoveAt(0);
                                                }
                                            }
                                        }
                                    }
                                    await Task.Yield();
                                }
                            }
                        }
                       else if(isComplete.Result == false)
                        {
                            firstMove = Vector2.zero;
                            secondMove = Vector2.zero;
                            isMoving = true;
                            m_foundMabel = false;
                        }
                    }

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

                await Task.Yield();
            }
        }

        private Vector2 firstMove, secondMove;
        private Task<bool> CheckMinusXPosition()
        {
            //TODO FIX ALL
            var guardPos = ConvertIntoInteger(m_rigidBody2D.position);
            var mPos = ConvertIntoInteger(m_Mabel.currentPath);
            //Mabel position
            var distanceBetween = mPos - guardPos;

            //should keep 1, 1 or 2,0 not more than 2
            //if (distanceBetween.x == guardLineSight && distanceBetween.y != 0 || distanceBetween.x == -guardLineSight && distanceBetween.y != 0 ||
            //    distanceBetween.y == guardLineSight && distanceBetween.x != 0 || distanceBetween.y == -guardLineSight && distanceBetween.x != 0)
            //{
            //    return Task.CompletedTask;
            //}
            if (distanceBetween.x == 0 && distanceBetween.y == 0)
            {
                return Task.FromResult(false);
            }
            //var checkPosition = m_rigidBody2D.position.x + distanceBetween.x;
            //Debug.Log(distanceBetween.x + " distance X");
            //Debug.Log(distanceBetween.y + " distance Y");
            //Debug.Log(distanceBetween.y != -guardLineSight);
            //Debug.Log(distanceBetween.y != guardLineSight);
            //Debug.Log(distanceBetween.x != -guardLineSight);
            //Debug.Log(distanceBetween.x != guardLineSight);
            if (distanceBetween.x != guardLineSight && distanceBetween.y != guardLineSight && distanceBetween.x != -guardLineSight && distanceBetween.y != -guardLineSight)
            {
                //  Debug.Log("move L");
                if (guardPos.x > mPos.x)
                {
                   // var currentPosition = ConvertIntoInteger(m_rigidBody2D.position);
                    var move = mapTilePoints [guardPos];
                   if (move.Left)
                    {
                        var tempLeft = guardPos.x + distanceBetween.x;
                        var newPosition = new Vector2(tempLeft, guardPos.y);
                        firstMove = newPosition;
                        var currentPosition1 = ConvertIntoInteger(newPosition);
                        if (guardPos.y < mPos.y)
                        {
                            var move1 = mapTilePoints [currentPosition1];
                            if (move1.Upper)
                            {
                                var tempDown = guardPos.y + distanceBetween.y;
                                var newPosition1 = new Vector2(newPosition.x, tempDown);
                                secondMove = newPosition1;
                                return Task.FromResult(true);
                            }
                            else
                            {
                                return Task.FromResult(false);
                            }
                        }
                        else if (guardPos.y > mPos.y)
                        {
                            var move1 = mapTilePoints [currentPosition1];
                            if (move1.Down)
                            {
                                var tempUp = guardPos.y + distanceBetween.y;
                                var newPosition1 = new Vector2(newPosition.x, tempUp);
                                secondMove = newPosition1;
                                return Task.FromResult(true);
                            }
                            else
                            {
                                return Task.FromResult(false);
                            }
                        }
                        else
                        {
                            return Task.FromResult(false);
                        }
                    }
                    else if (move.Upper)
                    {
                        var tempUpper = guardPos.y + distanceBetween.y;
                        var newPosition = new Vector2(guardPos.x, tempUpper);
                        firstMove = newPosition;
                        var currentPosition1 = ConvertIntoInteger(newPosition);
                        if (guardPos.x < mPos.x)
                        {
                            var move1 = mapTilePoints [currentPosition1];
                            if (move1.Left)
                            {
                                var tempLeft = guardPos.x + distanceBetween.x;
                                var newPosition1 = new Vector2(tempLeft, newPosition.y);
                                secondMove = newPosition1;
                                return Task.FromResult(true);
                            }
                            else
                            {
                                return Task.FromResult(false);
                            }
                        }
                        else
                        {
                            return Task.FromResult(false);
                        }
                    }
                    else if (move.Down)
                    {
                        var tempDown = guardPos.y + distanceBetween.y;
                        var newPosition = new Vector2(guardPos.x, tempDown);
                        firstMove = newPosition;
                        var currentPosition1 = ConvertIntoInteger(newPosition);
                        if (guardPos.x < mPos.x)
                        {
                            var move1 = mapTilePoints [currentPosition1];
                            if (move1.Left)
                            {
                                var tempLeft = guardPos.x + distanceBetween.x;
                                var newPosition1 = new Vector2(tempLeft, newPosition.y);
                                secondMove = newPosition1;
                                return Task.FromResult(true);
                            }
                            else
                            {
                                return Task.FromResult(false);
                            }
                        }
                        else
                        {
                            return Task.FromResult(false);
                        }
                    }
                    else
                    {
                        return Task.FromResult(false);
                    }
                }
                else if (guardPos.x < mPos.x)
                {
                    var currentPosition = ConvertIntoInteger(m_rigidBody2D.position);
                    var move = mapTilePoints [guardPos];
                    if (move.Right)
                    {
                        var tempRight = guardPos.x + distanceBetween.x;
                        var newPosition = new Vector2(tempRight, guardPos.y);
                        firstMove = newPosition;
                        var currentPosition1 = ConvertIntoInteger(newPosition);
                        if (guardPos.y < mPos.y)
                        {
                            //new tilePoints
                            var move1 = mapTilePoints [currentPosition1];
                            if (move1.Upper)
                            {
                                var tempUpper = guardPos.y + distanceBetween.y;
                                var newPosition1 = new Vector2(newPosition.x, tempUpper);
                                secondMove = newPosition1;
                                return Task.FromResult(true);
                            }
                            else
                            {
                                return Task.FromResult(false);
                            }
                        }
                        else if (guardPos.y > mPos.y)
                        {
                            var move1 = mapTilePoints [currentPosition1];
                            if (move1.Down)
                            {
                                var tempDown = guardPos.y + distanceBetween.y;
                                var newPosition1 = new Vector2(newPosition.x, tempDown);
                                secondMove = newPosition1;
                                return Task.FromResult(true);
                            }
                            else
                            {
                                return Task.FromResult(false);
                            }
                        }
                        else
                        {
                            return Task.FromResult(false);
                        }
                    }
                    else if (move.Upper)
                    {
                        var tempUpper = guardPos.y + distanceBetween.y;
                        var newPosition = new Vector2(guardPos.x, tempUpper);
                        firstMove = newPosition;
                        var currentPosition1 = ConvertIntoInteger(newPosition);
                        if (guardPos.x > mPos.x)
                        {
                            var move1 = mapTilePoints [currentPosition1];
                            if (move1.Right)
                            {
                                var tempRight = guardPos.x + distanceBetween.x;
                                var newPosition1 = new Vector2(tempRight, newPosition.y);
                                secondMove = newPosition1;
                                return Task.FromResult(true);
                            }
                            else
                            {
                                return Task.FromResult(false);
                            }
                        }
                        else
                        {
                            return Task.FromResult(false);
                        }
                    }
                    else if (move.Down)
                    {
                        var tempDown = guardPos.y + distanceBetween.y;
                        var newPosition = new Vector2(guardPos.x, tempDown);
                        firstMove = newPosition;
                        var currentPosition1 = ConvertIntoInteger(newPosition);
                        if (guardPos.x > mPos.x)
                        {
                            var move1 = mapTilePoints [currentPosition1];
                            if (move1.Right)
                            {
                                var tempRight = guardPos.x + distanceBetween.x;
                                var newPosition1 = new Vector2(tempRight, newPosition.y);
                                secondMove = newPosition1;
                                return Task.FromResult(true);
                            }
                            else
                            {
                                return Task.FromResult(false);
                            }
                        }
                        else
                        {
                            return Task.FromResult(false);
                        }
                    }
                    else
                    {
                        return Task.FromResult(false);
                    }
                }
            }
            else if (distanceBetween.x == guardLineSight && distanceBetween.y == 0
                || distanceBetween.y == guardLineSight && distanceBetween.x == 0
                || distanceBetween.x == -guardLineSight && distanceBetween.y == 0
                || distanceBetween.y == -guardLineSight && distanceBetween.x == 0)
            {
                // Debug.Log("move straight");
                if (guardPos.x < mPos.x)
                {
                    var move = mapTilePoints [guardPos];
                    if (move.Right)
                    {
                        var calculatePos = guardPos.x + distanceBetween.x -1;
                        var newPosition = new Vector2(calculatePos, guardPos.y);
                        var move1 = mapTilePoints [newPosition];
                        firstMove = newPosition;
                        if (move1.Right)
                        {
                            var calculatePos1 = newPosition.x + distanceBetween.x -1;
                            var newPosition1 = new Vector2(calculatePos1, newPosition.y);
                            secondMove = newPosition1;
                            return Task.FromResult(true);
                        }
                        else
                        {
                            return Task.FromResult(false);
                        }

                    }
                    else
                    {
                        return Task.FromResult(false);
                    }

                }
                else if (guardPos.x > mPos.x)
                {
                    var move = mapTilePoints [guardPos];
                   if (move.Left)
                    {
                        var calculatePos = guardPos.x + distanceBetween.x+1;
                        var newPosition = new Vector2(calculatePos, guardPos.y);
                        var move1 = mapTilePoints [newPosition];
                        firstMove = newPosition;
                        if (move1.Left)
                        {
                            var calculatePos1 = newPosition.x + distanceBetween.x+1;
                            var newPosition1 = new Vector2(calculatePos1, newPosition.y);
                            secondMove = newPosition1;
                            return Task.FromResult(true);

                        }
                        else
                        {
                            return Task.FromResult(false);
                        }
                    }
                    else
                    {
                        return Task.FromResult(false);
                    }
                }
                else if (guardPos.y > mPos.y)
                {
                    var move = mapTilePoints [guardPos];
                    if (move.Down)
                    {
                        var calculatePos = guardPos.y + distanceBetween.y +1;
                        var newPosition = new Vector2(guardPos.x, calculatePos);
                        var move1 = mapTilePoints [newPosition];
                        firstMove = newPosition;
                        if (move1.Down)
                        {
                            var calculatePos1 = newPosition.y + distanceBetween.y +1;
                            var newPosition1 = new Vector2(newPosition.x, calculatePos1);
                            secondMove = newPosition1;
                            return Task.FromResult(true);
                        }
                        else
                        {
                            return Task.FromResult(false);
                        }
                    }
                    else
                    {
                        return Task.FromResult(false);
                    }
                }
                else if (guardPos.y < mPos.y)
               {
                    var move = mapTilePoints [guardPos];
                    if (move.Upper)
                    {
                        var calculatePos = guardPos.y + distanceBetween.y -1;
                        var newPosition = new Vector2(guardPos.x, calculatePos);
                        var move1 = mapTilePoints [newPosition];
                        firstMove = newPosition;
                        if (move1.Upper)
                        {
                            var calculatePos1 = newPosition.y + distanceBetween.y -1;
                            var newPosition1 = new Vector2(newPosition.x, calculatePos1);
                            secondMove = newPosition1;
                            return Task.FromResult(true);
                        }
                        else
                        {
                            return Task.FromResult(false);
                        }
                    }
                    else
                    {
                        return Task.FromResult(false);
                    }
                }
                else
                {
                    return Task.FromResult(false);
                }
            }
            return Task.FromResult(false);
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
            lastPositionAfterTrack.Clear();
            m_moveToMabel = false;
            m_checkForDistance = false;
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
        private async Task<Task> MoveToLastPatrolPosition()
        {
           for (int p = lastPositionAfterTrack.Count-1; p > 0 ; p--)
           {
                Debug.Log(lastPositionAfterTrack [p]);
               // if (isMoving)
               // {
               //     var newPosition = lastPositionAfterTrack [p];
               //     if (m_rigidBody2D.position != newPosition)
               //     {
               //         m_rigidBody2D.position = Vector2.MoveTowards(m_rigidBody2D.position,
               //             newPosition, moveSpeed * Time.fixedDeltaTime);
               //     }
               //     else
               //     {
               //         continue;
               //     }
               //     if (p == 0)
               //     {
               //         return Task.CompletedTask;
               //     }
               // }
                await Task.Yield();
            }
            return Task.CompletedTask;
        }
        protected async void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Mabel"))
            { 
                isCollide = true;
                var m_Mabel = collision.gameObject.GetComponent<Mabel>();
                //TODO guard has Thomas
                //if (this.hasThomas)
                //{
                //    if (m_Mabel.disableMovement)
                //        return;
                //    GuardController.instance.GuardAction(false);
                //    this.m_hasThomas = false;
                //    m_Mabel.disableMovement = true;
                //
                //    _miniCanvas.alpha = 1;
                //    _miniCanvas.interactable = true;
                //    _miniCanvas.blocksRaycasts = true;
                //    var cgPanel = _miniCanvas.transform.GetChild(0).GetComponent<CanvasGroup>();
                //    cgPanel.alpha = 1;
                //    cgPanel.blocksRaycasts = true;
                //    cgPanel.interactable = true;
                //    _miniCanvas.transform.GetChild(0).GetChild(0).GetComponent<GuardWalk>().StartMove();
                //    GuardController.instance.CurrentGuardHasThomas(this);
                //    return;
                //}
                if (m_Mabel.hasThomas)
                {
                    m_Mabel.disableMovement = true;
                  //  moveSpeed = 0.8f;
                    m_Mabel.RemoveThomas();
                    this.m_hasThomas = true;
                    //TODO remove guard going back exit
                   // if (m_checkForDistance)
                   // {
                   //     m_checkForDistance = false;
                   // }
                   // if (m_moveToMabel)
                    //{
                    //    m_moveToMabel = false;
                    //    var isComplete = await MoveToLastPatrolPosition();
                    //    if (isComplete.IsCompleted)
                    //    {
                    //        //TODO
                    //        moveIncrement--;
                    //        shouldMoveBackward = true;
                    //        // GuardMoveBackwards();
                    //        //GuardMoveExit();
                    //        return;
                    //    }
                    //}
                   // moveIncrement--;
                   // shouldMoveBackward = true;
                   // GuardMoveBackwards();
                   // GuardMoveExit();
                    return;
                }
                else
                {
                    var beingCaught = m_Mabel.beingCaught;
                    if (beingCaught) return;
                    if (!beingCaught)
                        m_Mabel.beingCaught  = true;
                    GuardController.instance.GuardAction(false);
                    _miniCanvas.alpha = 1;
                    trailMoving = false;
                    var fadeIn = await Fade.instance.StartFade(true);
                    if (fadeIn.IsCompleted)
                    {
                        var text = _miniCanvas.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                        text.enabled = true;
                        if (UpdateMabelUI.instance._health >=0)
                        {
                            text.text = "The Guard Caught Mabel";
                            m_Mabel.m_hitPoints -= 1;
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
                            Fade.instance.transform.GetChild(1).gameObject.SetActive(true);
                            text.text = "Mabel lost to escape";
                        }

                    }
                }
            }
           // else if (collision.CompareTag("Exit"))
           // {
           //     if (this.hasThomas)
           //     {
           //         m_Mabel.disableMovement = true;
           //         shouldDestroy = true; shouldMoveBackward = false;
           //         m_hasThomas = false;
           //         GuardController.instance.GuardAction(false);
           //         _miniCanvas.alpha = 1;
           //         var fadeIn = await Fade.instance.StartFade(true);
           //         if (fadeIn.IsCompleted)
           //         {
           //     
           //             var text = _miniCanvas.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
           //             text.enabled = true;
           //     
           //     
           //             if (UpdateMabelUI.instance._health >= 0)
           //             {
           //                 text.text = "The Guard Has Exit and Lost Thomas";
           //                 UpdateMabelUI.instance.UpdateHealthMabel();
           //                 GameManager.Instance.EnableThomas();
           //                 await Fade.instance.StartFade(false);
           //                 m_Mabel.ResetPosition();
           //                 _miniCanvas.alpha = 0;
           //                 if (text != null)
           //                     text.enabled = false;
           //                 GuardController.instance.ResetAllGuard();
           //                 GuardController.instance.SetMovementSpeedAllGuard();
           //     
           //             }
           //             else
           //             {
           //                 text.text = "Mabel to Old to continue this escape.";
           //             }
           //     
           //         }
           //     
           //     }
           //     
           // }
        }
        public new void ResetPosition()
        {
            shouldDestroy = true; shouldMoveBackward = false; isCollide = false;
            m_rigidBody2D.position = (Vector2)movePoints [0].position;
            moveIncrement = 0;
        }
        private bool isAccursed { get; set; } = false;
        public async void StopMoving()
        {
            m_hitPoints--;
            isMoving = false;
            await Task.Delay(2000);
            if (m_hitPoints == 0)
            {
                if (!isAccursed)
                {
                    isAccursed = true;
                    if (trailMoving)
                        trailMoving = false;
                    shouldDestroy = true;
                    m_spriteRenderer.color = new Color(1, 1, 1, 0);
                    gameObject.SetActive(false);
                    var isDone = EmergeFromWall();
                    if (isDone.Result == true)
                    {
                        if (!trailMoving)
                            trailMoving = true;

                        return;
                    }
                }
            }
            isMoving = true;
        }
        public Task<bool> EmergeFromWall()
        {
            var GoPos = m_Mabel.currentPath;
            var CheckWall = mapTilePoints [GoPos];
            if (CheckWall.Upper == false)
            {
                var spawnPos = new Vector2(GoPos.x, GoPos.y + .5f);
                m_rigidBody2D.MovePosition(spawnPos);
                gameObject.SetActive(true);
                var increment = 0f;
                while (m_rigidBody2D.position != GoPos)
                {
                    increment += 0.2f * Time.fixedDeltaTime;
                    m_spriteRenderer.color = new Color(1, 1, 1, increment);
                    m_rigidBody2D.position = new Vector2(m_rigidBody2D.position.x, m_rigidBody2D.position.y - moveSpeed * Time.fixedDeltaTime);
                    Task.Yield();
                    if (m_rigidBody2D.position == GoPos)
                    {
                        m_spriteRenderer.color = new Color(1, 1, 1, 1);
                        m_hitPoints = 1;
                        return Task.FromResult(true);
                    }
                }
            }
            else if (CheckWall.Down == false)
            {
                var spawnPos = new Vector2(GoPos.x, GoPos.y - .5f);
                m_rigidBody2D.MovePosition(spawnPos);
                gameObject.SetActive(true);
                var increment = 0f;
                while (m_rigidBody2D.position != GoPos)
                {
                    increment += 0.2f * Time.fixedDeltaTime;
                    m_spriteRenderer.color = new Color(1, 1, 1, increment);
                    m_rigidBody2D.position = new Vector2(m_rigidBody2D.position.x, m_rigidBody2D.position.y + moveSpeed * Time.fixedDeltaTime);
                    Task.Yield();
                    if (m_rigidBody2D.position == GoPos)
                    {
                        m_spriteRenderer.color = new Color(1, 1, 1, 1);
                        m_hitPoints = 1;
                        return Task.FromResult(true);
                    }
                }
            }
            else if (CheckWall.Right == false)
            {
                var spawnPos = new Vector2(GoPos.x + .5f, GoPos.y);
                m_rigidBody2D.MovePosition(spawnPos);
                gameObject.SetActive(true);
                var increment = 0f;
                while (m_rigidBody2D.position != GoPos)
                {
                    increment += 0.2f * Time.fixedDeltaTime;
                    m_spriteRenderer.color = new Color(1, 1, 1, increment);
                    m_rigidBody2D.position = new Vector2(m_rigidBody2D.position.x - moveSpeed * Time.fixedDeltaTime, m_rigidBody2D.position.y);
                    Task.Yield();
                    if (m_rigidBody2D.position == GoPos)
                    {
                        m_spriteRenderer.color = new Color(1, 1, 1, 1);
                        m_hitPoints = 1;
                        return Task.FromResult(true);
                    }
                }
            }
            else if (CheckWall.Left == false)
            {
                var spawnPos = new Vector2(GoPos.x - .5f, GoPos.y);
                m_rigidBody2D.MovePosition(spawnPos);
                gameObject.SetActive(true);
                var increment = 0f;
                while (m_rigidBody2D.position != GoPos)
                {
                    increment += 0.2f * Time.fixedDeltaTime;
                    m_spriteRenderer.color = new Color(1, 1, 1, increment);
                    m_rigidBody2D.position = new Vector2(m_rigidBody2D.position.x + moveSpeed * Time.fixedDeltaTime, m_rigidBody2D.position.y);
                    Task.Yield();
                    if (m_rigidBody2D.position == GoPos)
                    {
                        m_spriteRenderer.color = new Color(1, 1, 1, 1);
                        m_hitPoints = 1;
                        return Task.FromResult(true);
                    }
                }
            }
            return Task.FromResult(false);
        }
    }
}
