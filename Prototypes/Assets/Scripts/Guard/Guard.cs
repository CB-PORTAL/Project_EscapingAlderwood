using DuRound;
using DuRound.MiniGame;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace DuRound
{
    public class BlockMove
    {
        public List<Vector2> listMoves = new List<Vector2>();
    }
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
        public void Initialize()
        {
            isAccursed = false;
            isCollide = false;
            moveIncrement = 0;
            isMoving = true;
            shouldDestroy = false;
            m_animator?.SetBool("isMove", true);
            pathTrailAfterMabel.Clear();
            lastPositionAfterTrack.Clear();
            m_lastPosition = m_rigidBody2D.position;
            CheckForDistance();
           // return Task.CompletedTask;
        }
        public void ResetMoveIncrement()
        {
            moveIncrement = 0;
        }
        public bool isDebug = false;
        // Update is called once per frame
        protected override void Update()
        {
            if (m_currentPosition != Vector2.zero)
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
        }
        // private int distanceIncrement;
        private bool m_foundMabel = false;
        private List<Vector2> pathTrailAfterMabel = new List<Vector2>();
        private List<Vector2> lastPositionAfterTrack = new List<Vector2>();
        private Vector2 m_MabelTrailPosition = new Vector2();
        [Header("GuardRangeSight", order = 0)]
        [Range(2f,7f,order = 2)]
        public float farLineSight = 5f;
        [Range(0f,2f,order = 1)]
        public float nearLineSight = 2f;
        private int lastIncrement { get; set; } = 0;
        private bool m_checkForDistance { get; set; } = true;
        private bool m_moveToMabel { get; set; } = true;
        private bool trailMoving { get; set; } = false;
        //private bool b_finishLastMove { get; set; } = false;
        private bool m_MabelIsNear { get; set; } = false;
        
        private async void CheckForDistance()
        {
            while (m_checkForDistance)
            {
                float CheckDistance()
                {
                    return Vector2.Distance(m_rigidBody2D.transform.position, m_Mabel.transform.position);
                }
                var distance = CheckDistance();
                if (distance <= farLineSight)
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
                        var isComplete = await CheckMinusXPosition();
                        if (isComplete == true)
                        {
                            m_moveToMabel = true;
                            shouldDestroy = true;
                            //enable trail in here
                            trailMoving = true;
                            if (NewGuardPosition.Count > 0)
                            {
                                var tempBeforeFirst = ConvertIntoInteger(m_rigidBody2D.position);
                                lastPositionAfterTrack.Add(tempBeforeFirst);
                                await Task.Delay(500);
                                while (m_moveToMabel)
                                {
                                    /*
                                    if (!b_finishLastMove)
                                    {
                                        if (m_rigidBody2D.position != finishLastMove)
                                        {
                                            m_lastPosition = m_rigidBody2D.position;
                                            m_rigidBody2D.position = Vector2.MoveTowards(m_rigidBody2D.position, finishLastMove, moveSpeed * Time.fixedDeltaTime);
                                            m_currentPosition = m_rigidBody2D.position;
                                        }
                                        else
                                        {
                                            b_finishLastMove = true;
                                            trailMoving = true;
                                        }
                                    }*/
                                    if (trailMoving)
                                    {
                                        for (int move = 0; move < NewGuardPosition.Count; move++)
                                        {
                                            m_lastPosition = m_rigidBody2D.position;
                                            var newPosition = NewGuardPosition [move];
                                            while (m_rigidBody2D.position != newPosition)
                                            {
                                                if (!m_MabelIsNear)
                                                {
                                                    var distance1 = CheckDistance();
                                                    if (distance1 <= nearLineSight)
                                                    {
                                                        var isComplete2 = await CheckMinusXPosition();
                                                        if (isComplete2 == true)
                                                        {
                                                            m_MabelIsNear = true;
                                                            m_moveToMabel = false;
                                                            m_foundMabel = false;
                                                        }
                                                    }
                                                }
                                                if (isMoving)
                                                {
                                                    //Debug.Log("here " + newPosition);
                                                    m_rigidBody2D.position = Vector2.MoveTowards(m_rigidBody2D.transform.position, newPosition, moveSpeed * Time.fixedDeltaTime);
                                                    m_currentPosition = m_rigidBody2D.position;
                                                }
                                                AddingTrailPath();
                                                await Task.Yield();
                                            }
                                            await Task.Yield();
                                        }
                                        NewGuardPosition.Clear();
                                        AddingTrailPath();
                                        void AddingTrailPath()
                                        {
                                            //adding Mabel trail to guard memory
                                            var currentMabelPath = m_Mabel.currentPath;
                                            if (m_MabelTrailPosition == Vector2.zero || m_MabelTrailPosition != currentMabelPath)
                                            {
                                                m_MabelTrailPosition = currentMabelPath;
                                                //if (m_MabelTrailPosition != finishLastMove)
                                                //{
                                                //    finishLastMove = m_MabelTrailPosition;
                                                //}
                                                if (!pathTrailAfterMabel.Contains(m_MabelTrailPosition))
                                                {
                                                    pathTrailAfterMabel.Add(m_MabelTrailPosition);
                                                }
                                            }
                                        }
                                        /*
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
                                        */
                                        if (pathTrailAfterMabel.Count > 0)
                                        {
                                            var newPosition = pathTrailAfterMabel [0];
                                            if (m_rigidBody2D.position != newPosition)
                                            {
                                                if (!m_MabelIsNear)
                                                {
                                                    var distance1 = CheckDistance();
                                                    if (distance1 <= nearLineSight)
                                                    {
                                                        var isComplete3 = await CheckMinusXPosition();
                                                        if (isComplete3 == true)
                                                        {
                                                            m_MabelIsNear = true;
                                                            m_moveToMabel = false;
                                                            m_foundMabel = false;
                                                            pathTrailAfterMabel.Clear();
                                                        }
                                                    }
                                                }
                                                if (isMoving)
                                                {
                                                    // Debug.Log("here 2" + newPosition);
                                                    m_rigidBody2D.position = Vector2.MoveTowards(m_rigidBody2D.transform.position, newPosition, moveSpeed * Time.fixedDeltaTime);
                                                    m_currentPosition = m_rigidBody2D.position;
                                                }

                                            }
                                            else
                                            {
                                                m_lastPosition = m_rigidBody2D.position;
                                                var tempPos = ConvertIntoInteger(m_rigidBody2D.position);
                                                lastPositionAfterTrack.Add(tempPos);
                                                pathTrailAfterMabel.RemoveAt(0);
                                            }
                                        }

                                    }
                                    await Task.Yield();
                                }
                            }
                        }
                        else if (isComplete == false)
                        {
                            m_foundMabel = false;
                        }
                    }

                }
                else if (distance >= 4) m_MabelIsNear = false;
               // if(distance >= farLineSight) m_MabelIsNear = false;
                await Task.Yield();
            }
        }
        private List<Vector2> NewGuardPosition = new List<Vector2>();

        private async Task<bool> CheckMinusXPosition()
        {
            Vector2 guardPos = ConvertIntoInteger(m_rigidBody2D.position);
            Vector2 mPos = ConvertIntoInteger(m_Mabel.currentPath);
            #region "At certain position"
            //position on outside edge screen of game
            if (mPos.y == 19 || guardPos.y == 19)
            {
                mPos.y -= 1;guardPos.y -= 1;
            }
            if (mPos.x == 11 || guardPos.x == 11)
            {
                mPos.x -= 1; guardPos.x -= 1;
            }
            if (mPos.x == -12 || guardPos.x == -12)
            {
                mPos.x -= 1; guardPos.x -= 1;
            }
            if (mPos.y == -5 || guardPos.y == -5)
            {
                mPos.y -= 1; guardPos.y -= 1;
            }
            #endregion
            //Mabel position
            Vector2 xOne = new Vector2(1, 0);
            Vector2 yOne = new Vector2(0, 1);
            var distanceBetween = mPos - guardPos;
            Debug.Log(distanceBetween);
            var saveDistanceBetween = distanceBetween;
            int tempDistX = (int)distanceBetween.x;
            int tempDistY = (int)distanceBetween.y;
            #region "Converting to Positive"
            if (tempDistX < 0)
                tempDistX = Mathf.Abs(tempDistX);
            if(tempDistY < 0)
                tempDistY = Mathf.Abs(tempDistY);
            #endregion
            int totalPosXY = tempDistX + tempDistY;
            int XYLowest = 0;
            List<Vector2> positionList = new List<Vector2>();
            #region "Init Position List"
            if (tempDistX < tempDistY) {
                XYLowest = tempDistX;
                for (int xPos = 0; xPos < tempDistX; xPos++)
                {
                    positionList.Add(xOne);
                }
                for (int yPos = 0; yPos < tempDistY; yPos++)
                {
                    positionList.Add(yOne);
                }
            }
            else if (tempDistY < tempDistX) {
                XYLowest = tempDistY;
                for (int yPos = 0; yPos < tempDistY; yPos++)
                {
                    positionList.Add(yOne);
                }
                for (int xPos = 0; xPos < tempDistX; xPos++)
                {
                    positionList.Add(xOne);
                }
            } else
            {
                XYLowest = tempDistX;
                for (int xPos = 0; xPos < tempDistX; xPos++)
                {
                    positionList.Add(xOne);
                }
                for (int yPos = 0; yPos < tempDistY; yPos++)
                {
                    positionList.Add(yOne);
                }
            }
            #endregion
            List<BlockMove> blocklist = new List<BlockMove>();
            BlockMove bm = new BlockMove();
            foreach (var item in positionList)
            {
                bm.listMoves.Add(item);
            }
            blocklist.Add(bm);
            var tempFirstRow = blocklist [0].listMoves;
            List<Vector2> lastFirstRow = tempFirstRow;
            FirstRow();
            SecondRow();
            ThirdRow();
            FourthRow();
            void FirstRow()
            {
                //Loop from lowest num and get all num lowest from high to low and do reposition or shuffle
                for (int f = 0; f < tempFirstRow.Count; f++)
                {
                    var firstRow = tempFirstRow;
                    if (f >= XYLowest)
                    {
                        var diffDistance = f - XYLowest;
                        var firstF = f;
                        var firstBlocks = new Vector2();
                        for (int df = f; df >= diffDistance; df--)
                        {
                            if (firstF == df)
                            {
                                firstBlocks = firstRow [df];
                            }
                            if (df > 0)
                            {
                                var lastBlock = firstRow [df - 1];
                                firstRow [df] = lastBlock;
                            }
                            if (df == diffDistance)
                            {
                                firstRow [df] = firstBlocks;
                                break;
                            }
                        }
                        BlockMove bm = new BlockMove();
                        for (int f2 = 0; f2 < firstRow.Count; f2++)
                        {
                            bm.listMoves.Add(tempFirstRow [f2]);
                        }
                        blocklist.Add(bm);
                    }
                }

            }
            void SecondRow()
            {
                if (XYLowest < 1) return;

                BlockMove bm = new BlockMove();
                int [] arrInvolve = new int [XYLowest];
                int firstNumAhead = XYLowest - 1;
                int decrementFirstNumAhead = firstNumAhead;
                for (int involve = 0; involve < arrInvolve.Length; involve++)
                {
                    arrInvolve [involve] = involve;
                }
                bool init = false;
                bool isBreak = false;
                while (true)
                {
                    for (int cInvAr = arrInvolve.Length-1; cInvAr > -1; cInvAr--)
                    {
                        if (arrInvolve [arrInvolve.Length - 1] + 1 >= totalPosXY)
                        {
                            for (int arIn = arrInvolve.Length - 2; arIn > -1; arIn--)
                            {
                                var ar = lastFirstRow [arrInvolve [arIn]];
                                var nextAr = lastFirstRow [arrInvolve [arIn] + 1];
                                var nextInvolve = arrInvolve [arIn] + 1;

                                lastFirstRow [arrInvolve [arIn]] = nextAr;
                                lastFirstRow [arrInvolve [arIn] + 1] = ar;
                                arrInvolve [arIn] = nextInvolve;
                            }
                            isBreak = true;
                            break;
                        }
                        else
                        {
                            if (!init)
                            {
                                init = true;
                                for (int cInvArInit = arrInvolve.Length - 1; cInvArInit > 0; cInvArInit--)
                                {
                                    var ar = lastFirstRow [arrInvolve [cInvArInit]];
                                    var nextAr = lastFirstRow [arrInvolve [cInvArInit] + decrementFirstNumAhead];
                                    var nextDec = arrInvolve [cInvArInit] + decrementFirstNumAhead;

                                    lastFirstRow [arrInvolve [cInvArInit]] = nextAr;
                                    lastFirstRow [arrInvolve [cInvArInit] + decrementFirstNumAhead] = ar;
                                    arrInvolve [cInvArInit] = nextDec;
                                    decrementFirstNumAhead--;
                                }
                                break;
                            }
                            else
                            {
                               var ar = lastFirstRow [arrInvolve [cInvAr]];
                               var nextAr = lastFirstRow [arrInvolve [cInvAr] + 1];
                               var nextDec = arrInvolve [cInvAr] + 1;

                               lastFirstRow [arrInvolve [cInvAr]] = nextAr;
                               lastFirstRow [arrInvolve [cInvAr] + 1] = ar;
                               arrInvolve [cInvAr] = nextDec;
                                
                            }
                        }

                    }
                    BlockMove tempBm = new BlockMove();
                    for (int lastRow = 0; lastRow < lastFirstRow.Count; lastRow++)
                    {
                        tempBm.listMoves.Add(lastFirstRow [lastRow]);
                    }
                    blocklist.Add(tempBm);
                    if (isBreak)
                        break;
                }
            }
            void ThirdRow()
            {
                if (XYLowest < 1)
                    return;
                int [] arrInvolve = new int [XYLowest];
                for (int arr = 0; arr < arrInvolve.Length; arr++)
                {
                    arrInvolve [arr] = arr;
                }
                List<Vector2> thirdRowStarted = new List<Vector2>();
                List<Vector2> reverseThirdRow = new List<Vector2>();
                for (int starter = 0; starter < positionList.Count; starter++)
                {
                    thirdRowStarted.Add(positionList [starter]);
                }
                int ThirdRowLoop = XYLowest - 1;
                while (ThirdRowLoop > -1)
                {
                    for (int thirdRow = 0; thirdRow < thirdRowStarted.Count; thirdRow++)
                    {
                        if (arrInvolve [ThirdRowLoop] == thirdRow)
                        {
                            if (thirdRow + 1 > thirdRowStarted.Count - 1)
                            {
                                break;
                            }
                            var arr = thirdRowStarted [thirdRow];
                            var nextArr = thirdRowStarted [thirdRow + 1];

                            thirdRowStarted [thirdRow] = nextArr;
                            thirdRowStarted [thirdRow + 1] = arr;

                            arrInvolve [ThirdRowLoop] = thirdRow + 1;

                            if (arrInvolve [ThirdRowLoop] > thirdRowStarted.Count - 1)
                            {
                                break;
                            }
                            BlockMove bm = new BlockMove ();
                            for (int tempXY = 0; tempXY < thirdRowStarted.Count; tempXY++)
                            {
                                bm.listMoves.Add(thirdRowStarted [tempXY]);
                            }
                            blocklist.Add(bm);
                        }
                    }
                    if (ThirdRowLoop == 0)
                    {
                        BlockMove tempBm = new BlockMove();
                        for (int tempXY = 0; tempXY < thirdRowStarted.Count; tempXY++)
                        {
                            tempBm.listMoves.Add(thirdRowStarted [tempXY]);
                            reverseThirdRow.Add(thirdRowStarted [tempXY]);
                        }
                        blocklist.Add(tempBm);
                    }
                    ThirdRowLoop--;
                }

                int ReverseThirdRow = 0;
                
                arrInvolve = new int [XYLowest];
                var starterReverse = totalPosXY - XYLowest;
                int increment = 0;
                for (int starter = starterReverse; starter < totalPosXY; starter++)
                {
                    arrInvolve [increment] = starter;
                    increment++;
                }
                while (ReverseThirdRow < XYLowest)
                {
                    for (int tempXY = reverseThirdRow.Count-1; tempXY > 0; tempXY--)
                    {
                        if (arrInvolve [ReverseThirdRow] == tempXY)
                        {
                            var arr = reverseThirdRow [tempXY];
                            var nextArr = reverseThirdRow [tempXY - 1];

                            reverseThirdRow [tempXY] = nextArr;
                            reverseThirdRow [tempXY - 1] = arr;

                            arrInvolve [ReverseThirdRow] = tempXY - 1;

                            BlockMove bm = new BlockMove();
                            for (int fXY = 0; fXY < reverseThirdRow.Count; fXY++)
                            {
                                bm.listMoves.Add(reverseThirdRow [fXY]);
                            }
                            blocklist.Add(bm);
                        }
                    }
                    ReverseThirdRow++;
                }
            }
            void FourthRow()
            {
                if (XYLowest < 2)
                    return;

                var thirdRowStarter = positionList;

                int [] arrInvolve = new int [XYLowest];
                for (int starter = 0; starter < arrInvolve.Length; starter++)
                {
                    arrInvolve [starter] = starter;
                }
                int incrementArr = 0;
                for (int starter2 = XYLowest; starter2 > 1; starter2--)
                {
                    incrementArr = starter2;
                }
                List<Vector2> fourthRowArr = new List<Vector2>();
                List<Vector2> reverseFourthRowArr = new List<Vector2>();
               // List<Vector2> resetFourthRowArr = new List<Vector2>();

                for (int tempXY = 0; tempXY < positionList.Count; tempXY++)
                {
                   // var lastArr = positionList.Count - 1;
                   // var newArr = lastArr - tempXY;
                    fourthRowArr.Add(positionList [tempXY]);
                   // resetFourthRowArr.Add(positionList [newArr]);
                    
                }
                for (int reverseXY = positionList.Count-1; reverseXY >= 0; reverseXY--)
                {
                    reverseFourthRowArr.Add(positionList [reverseXY]);
                }
                var XYHighest = totalPosXY - XYLowest;
                int incrementPlus = 1;
                int incrementNormal = 0;
                List<Vector2> firstArr = new List<Vector2> ();
                
                var FourthArrBig = totalPosXY - XYLowest;
                while (FourthArrBig > 0)
                {
                    
                    //if (FourthArrBig >= XYHighest)
                    //{
                    //    for (int resetFourth = 0; resetFourth < resetFourthRowArr.Count; resetFourth++)
                    //    {
                    //        fourthRowArr [resetFourth] = resetFourthRowArr [resetFourth];
                    //    }
                    //    for(int arrInvolve =)
                    //}
                    
                    for (int tempXY = 0; tempXY < fourthRowArr.Count; tempXY++)
                    {
                        if (tempXY == 0)
                        {
                            if (firstArr.Count > 0)
                            {
                                for (int tempXY2 = 0; tempXY2 < firstArr.Count; tempXY2++)
                                {
                                    fourthRowArr [tempXY2] = firstArr [tempXY2];
                                }
                            }
                            for (int CArr = arrInvolve.Length - 1; CArr >= incrementArr - 1; CArr--)
                            {
                                if (CArr < 0)
                                    break;
                                if (arrInvolve [CArr] == fourthRowArr.Count - 1)
                                    break;

                                var arr = fourthRowArr [arrInvolve [CArr] + incrementNormal];
                                var nextArr = fourthRowArr [arrInvolve [CArr] + incrementPlus];
                                var CInvolveArr = arrInvolve [CArr];

                                fourthRowArr [arrInvolve [CArr] + incrementNormal] = nextArr;
                                fourthRowArr [arrInvolve [CArr] + incrementPlus] = arr;

                                arrInvolve [CArr] = CInvolveArr + incrementPlus;
                                if (arrInvolve [CArr] == arrInvolve.Length - 1)
                                {
                                    arrInvolve [CArr] = arrInvolve.Length - 1;
                                }
                            }
                            firstArr.Clear();
                            BlockMove bm = new BlockMove();
                            for (int tempXY2 = 0; tempXY2 < fourthRowArr.Count; tempXY2++)
                            {
                                bm.listMoves.Add(fourthRowArr [tempXY2]);
                                firstArr.Add(fourthRowArr [tempXY2]);
                            }
                            blocklist.Add(bm);
                            tempXY = arrInvolve [arrInvolve.Length - 1];
                            if (FourthArrBig == 1)
                            {
                                int finalArrCalculate = 0;
                                if (XYLowest > 1)
                                {
                                    finalArrCalculate = XYLowest - 1;
                                }
                                else
                                {
                                    finalArrCalculate = 1;
                                }
                                var totalArrCalculate = totalPosXY - finalArrCalculate;
                                BlockMove bm2 = new BlockMove();
                                for (int tempXY3 = 0; tempXY3 < fourthRowArr.Count; tempXY3++)
                                {
                                    bm2.listMoves.Add(fourthRowArr [tempXY3]);
                                }
                                blocklist.Add(bm2);
                            }
                        }
                        else
                        {
                            var newTempXY = tempXY - 1;

                            var arr = fourthRowArr [newTempXY + incrementNormal];
                            var nextArr = fourthRowArr [newTempXY + 1];

                            fourthRowArr [newTempXY + incrementNormal] = nextArr;
                            fourthRowArr [newTempXY + 1] = arr;
                            

                            BlockMove bm = new BlockMove();
                            for (int tempXY2 = 0; tempXY2 < fourthRowArr.Count; tempXY2++)
                            {
                                bm.listMoves.Add(fourthRowArr [tempXY2]);
                            }
                            blocklist.Add(bm);
                            if (newTempXY + 1 >= fourthRowArr.Count -1)
                            {
                                if (arrInvolve.Length > 2)
                                {
                                    var starterArr = 2;
                                    for (int tempXY3 = arrInvolve.Length - 2; tempXY3 > 0; tempXY3--)
                                    {
                                        var CInvolve = arrInvolve [tempXY3];
                                        for (int tempXY4 = 0; tempXY4 < fourthRowArr.Count; tempXY4++)
                                        {
                                            if (tempXY4 == arrInvolve [tempXY3])
                                            {
                                                if (tempXY4 >= fourthRowArr.Count - starterArr)
                                                {
                                                    break;
                                                }

                                                var arr1 = fourthRowArr [tempXY4];
                                                var nextArr1 = fourthRowArr [tempXY4 + 1];
                                                var nextInvolve = tempXY4 + 1;

                                                fourthRowArr [tempXY4 + 1] = arr1;
                                                fourthRowArr [tempXY4] = nextArr1;
                                                arrInvolve [tempXY3] = nextInvolve;

                                                BlockMove bm1 = new BlockMove();
                                                for (int tempXY5 = 0; tempXY5 < fourthRowArr.Count; tempXY5++)
                                                {
                                                    bm1.listMoves.Add(fourthRowArr [tempXY5]);
                                                }
                                                blocklist.Add(bm1);
                                            }
                                        }
                                        arrInvolve [tempXY3] = CInvolve;
                                        starterArr++;
                                    }

                                }
                                break;
                            }
                        }

                    }
                    FourthArrBig--;
                }
                int FourthArrReverse = totalPosXY;
                int incrementPlus2 = 1;
                int incrementNormal2 = 0;
                int incrementInvolveArr2 = 0;
                var ReverseLowest = totalPosXY - XYLowest;
                for (int reverseStarter = totalPosXY - 1; reverseStarter >= totalPosXY - XYLowest; reverseStarter--)
                {
                    incrementInvolveArr2 = reverseStarter;
                }

                int [] arrInvolve2 = new int [totalPosXY];
                for (int CInvolve = ReverseLowest; CInvolve < reverseFourthRowArr.Count; CInvolve++)
                {
                    arrInvolve2 [CInvolve] = CInvolve;
                }
                List<Vector2> resetInvolveArray = new List<Vector2>();
                for (int tempReverseXY = 0; tempReverseXY < reverseFourthRowArr.Count; tempReverseXY++)
                {
                    resetInvolveArray.Add(reverseFourthRowArr [tempReverseXY]);
                }
                firstArr.Clear();
                while (FourthArrReverse > 0)
                {
                    if (FourthArrReverse >= totalPosXY)
                    {
                        if (resetInvolveArray.Count > 0)
                        {
                            for (int resetInvolve = 0; resetInvolve < resetInvolveArray.Count; resetInvolve++)
                            {
                                reverseFourthRowArr [resetInvolve] = resetInvolveArray [resetInvolve];
                            }
                            for (int firstReverse = ReverseLowest; firstReverse < reverseFourthRowArr.Count; firstReverse++)
                            {
                                arrInvolve2 [firstReverse] = firstReverse;
                            }
                            //incrementPlus2++;
                        }
                    }
                    for (int reverseArr = reverseFourthRowArr.Count - 1; reverseArr > -1; reverseArr--)
                    {
                        if (reverseArr == reverseFourthRowArr.Count - 1)
                        {
                            if (firstArr.Count > 0)
                            {
                                for (int fArr = 0; fArr < firstArr.Count; fArr++)
                                {
                                    reverseFourthRowArr [fArr] = firstArr [fArr];
                                }
                            }
                            for (int involveArr = incrementInvolveArr2; involveArr < totalPosXY - 1; involveArr++)
                            {
                                if (involveArr < 0) break;
                                if (arrInvolve2 [involveArr] <= 0) break;

                                var arr = reverseFourthRowArr [arrInvolve2 [involveArr] - incrementNormal2];
                                var nextArr = reverseFourthRowArr [arrInvolve2 [involveArr] - incrementPlus2];
                                var nextInvolveArr = arrInvolve2 [involveArr] - incrementPlus2;

                                reverseFourthRowArr [arrInvolve2 [involveArr] - incrementNormal2] = nextArr;
                                reverseFourthRowArr [arrInvolve2 [involveArr] - incrementPlus2] = arr;
                                arrInvolve2 [involveArr] = nextInvolveArr;
                                if (arrInvolve2 [involveArr] <= 0)
                                {
                                    arrInvolve2 [involveArr] = 0;
                                }
                            }
                            firstArr.Clear();
                            BlockMove bm = new BlockMove();
                            for (int reverseTempXY = 0; reverseTempXY < reverseFourthRowArr.Count; reverseTempXY++)
                            {
                                bm.listMoves.Add(reverseFourthRowArr [reverseTempXY]);
                                firstArr.Add(reverseFourthRowArr [reverseTempXY]);
                            }
                            blocklist.Add(bm);
                            reverseArr = arrInvolve2 [incrementInvolveArr2] + 1;
                        }
                        else
                        {
                            var currentReverse = reverseArr;
                            if (currentReverse == 0)
                                break;
                            if (XYLowest <= 1)
                                break;
                            
                            var arr = reverseFourthRowArr [currentReverse - incrementNormal2];
                            var nextArr = reverseFourthRowArr [currentReverse - incrementPlus2];
                            
                            reverseFourthRowArr [currentReverse - incrementNormal2] = nextArr;
                            reverseFourthRowArr [currentReverse - incrementPlus2] = arr;
                            
                            BlockMove bm = new BlockMove();
                            for (int reverseArr2 = 0; reverseArr2 < reverseFourthRowArr.Count; reverseArr2++)
                            {
                                bm.listMoves.Add(reverseFourthRowArr [reverseArr2]);
                            }
                            blocklist.Add(bm);
                            if (currentReverse - 1 == 0)
                            {
                                if (XYLowest > 2)
                                {
                                    var starterArr = 2;
                                    for (int tempXY = ReverseLowest + 1; tempXY < reverseFourthRowArr.Count -1; tempXY++)
                                    {
                                        var currentInvolveArr = arrInvolve2 [tempXY];
                                        for (int tempXY2 = reverseFourthRowArr.Count - 1; tempXY2 >= 0; tempXY2--)
                                        {
                                            if (tempXY2 == arrInvolve2 [tempXY])
                                            {
                                                if (tempXY2 < starterArr)
                                                {
                                                    break;
                                                }
                                                
                                                var arr2 = reverseFourthRowArr [tempXY2];
                                                var nextArr2 = reverseFourthRowArr [tempXY2 - 1];
                                                var nextInvolveArr2 = tempXY2 - 1;
                            
                                                reverseFourthRowArr [tempXY2] = nextArr2;
                                                reverseFourthRowArr [tempXY2 - 1] = arr2;
                                                arrInvolve2 [tempXY] = nextInvolveArr2;
                            
                                                BlockMove bm1 = new BlockMove();
                                                for (int tempXY3 = 0; tempXY3 < reverseFourthRowArr.Count; tempXY3++)
                                                {
                                                    bm1.listMoves.Add(reverseFourthRowArr [tempXY3]);
                                                }
                                                blocklist.Add(bm1);
                                            }
                                        }
                                        arrInvolve2 [tempXY] = currentInvolveArr;
                                        starterArr++;
                                    }
                                }
                                break;
                            }
                        }
                    }
                    FourthArrReverse--;
                }


            }

            RemoveUnnecessaryBlocks();
            ConvertDistanceBetweenBack();
            List<Vector2> MovePosition = new List<Vector2>();
            MovePosition.Clear();
            CheckForMapWithGivenPosition();
            NewGuardPosition.Clear();
            MovePosition.ForEach(Move => { NewGuardPosition.Add(Move); }) ;
            
            if (NewGuardPosition.Count > 0)
            {
                return await Task.FromResult(true);
            }
            return await Task.FromResult(false);

            void RemoveUnnecessaryBlocks()
            {
                List<BlockMove> blockMoveList = new List<BlockMove>();
                for (int currentBlocks = 0; currentBlocks < blocklist.Count; currentBlocks++)
                {
                    //if (currentBlocks == blocklist.Count)
                    //{
                    //    break;
                    //}
                    var currentMove = blocklist [currentBlocks];
                    int maxMove = blocklist [currentBlocks].listMoves.Count;
                    for (int compareBlocks = 0; compareBlocks < blocklist.Count; compareBlocks++)
                    {
                        if (currentBlocks == compareBlocks)
                            break;

                        var compareMove = blocklist [compareBlocks];
                        int incrementMove = 0;
                        for (int CM = 0; CM < blocklist [compareBlocks].listMoves.Count; CM++)
                        {
                            var compare = CheckForBlocks(currentMove.listMoves [CM], compareMove.listMoves [CM]);
                            if (compare)
                            {
                                break;
                            }
                            else
                            {
                                incrementMove++;
                                if (incrementMove == maxMove)
                                {
                                    // Debug.Log("from " + currentBlocks + "remove " +  compareBlocks);
                                    var removeBlocks = blocklist [compareBlocks];
                                    if (!blockMoveList.Contains(removeBlocks))
                                    {
                                        blockMoveList.Add(removeBlocks);
                                    }
                                    break;
                                }
                            }

                        }
                    }
                }
                if (blockMoveList.Count > 0)
                {
                    for (int bml = 0; bml < blockMoveList.Count; bml++)
                    {
                        blocklist.Remove(blockMoveList [bml]);
                    }
                }
                for (int bl = 0; bl < blocklist.Count; bl++)
                {
                    for (int moveList = 0; moveList < blocklist [bl].listMoves.Count; moveList++)
                    {
                       // Debug.Log(blocklist [bl].listMoves [moveList] + " move list " + moveList);
                    }
                }
            }
            void ConvertDistanceBetweenBack()
            {

              for (int b = 0; b < blocklist.Count; b++)
               {
                   for (int i = 0; i < blocklist [b].listMoves.Count; i++)
                   {
                        if (saveDistanceBetween.x < 0)
                        {
                            if (blocklist [b].listMoves[i].x > 0)
                            {
                                var temp = new Vector2(-blocklist [b].listMoves [i].x, blocklist [b].listMoves [i].y);
                                blocklist [b].listMoves [i] = temp;
                            }
                        }
                   }
               }
                
               
              for (int b = 0; b < blocklist.Count; b++)
              {
                  for (int i = 0; i < blocklist [b].listMoves.Count; i++)
                  {
                      if (saveDistanceBetween.y < 0)
                      {
                            if (blocklist [b].listMoves [i].y > 0)
                            {
                                var temp = new Vector2(blocklist [b].listMoves [i].x, -blocklist [b].listMoves [i].y);
                                blocklist [b].listMoves [i] = temp;
                            }
                      }
                  }
              }
               
            }
            bool CheckForBlocks(Vector2 currentBlocks, Vector2 compareBlocks)
            {
                if (currentBlocks == compareBlocks)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            void CheckForMapWithGivenPosition()
            {
                MovePosition.Clear();
                MovePosition.Add(guardPos);
                for (int b = 0; b < blocklist.Count; b++)
                {
                    var currentPos = guardPos;
                    for (int m = 0; m < blocklist [b].listMoves.Count; m++)
                    {
                        var tempMoveXY = blocklist [b].listMoves [m];
                        var canMove = CheckForMapWall(tempMoveXY, out Vector2 outPosition, currentPos);
                        if (canMove)
                        {
                            currentPos = outPosition;
                        }
                        else break;
                    }
                    if (MovePosition.Count == totalPosXY + 1)
                    {
                        break;
                    }
                    else
                    {
                        MovePosition.Clear();
                    }
                }
                
            }
            bool CheckForMapWall(Vector2 nextPosition, out Vector2 outPosition, Vector2 guardPosition)
            {
                var newPosition = guardPosition + nextPosition;
                outPosition = newPosition;
                var mapPosition = mapTilePoints [guardPosition];
                if (nextPosition.x > 0)
                {
                    if (mapPosition.Right)
                    {
                        MovePosition.Add(newPosition);
                        return true;
                    }
                    else return false;
                }
                else if (nextPosition.x < 0)
                {
                    if (mapPosition.Left)
                    {
                        MovePosition.Add(newPosition);
                        return true; 
                    }
                    else return false;
                }
                else if (nextPosition.y > 0)
                {
                    if (mapPosition.Upper) 
                    {
                        MovePosition.Add(newPosition);
                        return true; 
                    } 
                    else return false;
                }
                else if (nextPosition.y < 0)
                {
                    if (mapPosition.Down)
                    {
                        MovePosition.Add(newPosition);
                        return true;
                    }
                    else return false;
                }
                return false;

            }
        }
       // private bool m_hasMapData { get; set; } = false;
        private Dictionary<Vector2,TilePoints>mapTilePoints = new Dictionary<Vector2,TilePoints>(); 
        public void SentMapData(Dictionary<Vector2,TilePoints> mapTile)
        {
            mapTilePoints = mapTile;
           // m_hasMapData = true;
        }
        protected override void FixedUpdate()
        {
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
        //should guard finish las move
       // private Vector2 finishLastMove { get; set; }
        public async void GuardMoveForwards()
        {
            shouldDestroy = false;
            while (!isCollide && !shouldDestroy)
            {
                if (moveIncrement < movePoints.Length)
                {

                    var movePos = movePoints [moveIncrement];
                    //finishLastMove = movePos.position;
                    trailMoving = false;//b_finishLastMove = false;
                    if (m_rigidBody2D.position != (Vector2)movePos.position)
                    {
                        if (isMoving)
                        {
                            m_rigidBody2D.position = 
                                Vector2.MoveTowards(m_rigidBody2D.transform.position, movePos.position, moveSpeed * Time.fixedDeltaTime);

                            m_currentPosition = m_rigidBody2D.position;
                        }
                    }
                    else
                    {
                        m_lastPosition = m_rigidBody2D.position;
                        moveIncrement++;
                    }
                }
                else
                {
                    isCollide = true;
                    shouldDestroy = true;
                    UnSetAnimation();
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
        private void UnSetAnimation()
        {
            m_animator.SetBool("isMove", false);
            m_animator.SetFloat("MoveY", 0);
            m_animator.SetFloat("MoveX", 0);
            m_animator.SetFloat("IdleY", 1);
            m_animator.SetFloat("IdleX", 0);
        }
        private async void GuardMoveBackwards()
        {
            while (shouldMoveBackward && !shouldDestroy)
            {
                if (moveIncrement > -1)
                {
                    var movePos = movePoints [moveIncrement];
                   // finishLastMove = movePos.position;
                    trailMoving = false;//b_finishLastMove = false;
                    if (m_rigidBody2D.position != (Vector2)movePos.position)
                    {
                        if (isMoving)
                        {

                            m_rigidBody2D.position =
                            Vector2.MoveTowards(m_rigidBody2D.transform.position, movePos.position, moveSpeed * Time.fixedDeltaTime);

                            m_currentPosition = m_rigidBody2D.position;
                        }
                    }
                    else
                    {
                        m_lastPosition = m_rigidBody2D.position;
                        moveIncrement--;
                    }

                }
                else
                {
                    shouldMoveBackward = false;
                    shouldDestroy = true;
                    UnSetAnimation();
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
        /*
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
        }*/
        public override void AddThomas()
        {
            m_hasThomas = true;
        }
        public override void RemoveThomas()
        {
            m_hasThomas = false;
        }
        private bool invisible { get; set; } = false;
        protected async void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Mabel"))
            { 
                isCollide = true;
                var m_Mabel = collision.gameObject.GetComponent<Mabel>();
                /*
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
                }*/
                if (m_Mabel.hasThomas)
                {
                   m_Mabel.disableMovement = true;
                   moveSpeed = 0.8f;
                   m_Mabel.RemoveThomas();
                   this.m_hasThomas = true;/*
                   //TODO remove guard going back exit
                   if (m_checkForDistance)
                   {
                       m_checkForDistance = false;
                   }
                   if (m_moveToMabel)
                   {
                       m_moveToMabel = false;
                       var isComplete = await MoveToLastPatrolPosition();
                       if (isComplete.IsCompleted)
                       {
                           //TODO
                           moveIncrement--;
                           shouldMoveBackward = true;
                           // GuardMoveBackwards();
                           //GuardMoveExit();
                           return;
                       }
                   }
                    moveIncrement--;
                    shouldMoveBackward = true;
                    GuardMoveBackwards();
                    GuardMoveExit();*/
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
                           // _miniCanvas.alpha = 0;
                           if(text != null)
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
            /*
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
                
                
                        if (UpdateMabelUI.instance._health >= 0)
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
                
            }*/
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
                    m_moveToMabel = false;
                    shouldDestroy = true;
                    m_spriteRenderer.color = new Color(1, 1, 1, 0);
                    gameObject.SetActive(false);
                    var isDone = EmergeFromWall();
                    if (isDone.Result == true)
                    {
                        if (!trailMoving)
                            trailMoving = true;
                        m_foundMabel = false;
                        m_checkForDistance = true;
                        CheckForDistance();
                        return;
                    }
                }
            }
            isMoving = true;
        }
        public Task<bool> EmergeFromWall()
        {
            //TODO MOVE TO MIDDLE POSITION
            var GoPos = m_Mabel.transform.position;
            var mapGO = m_Mabel.currentPath;
            var CheckWall = mapTilePoints [mapGO];
            if (CheckWall.Upper == false)
            {
                var spawnPos = new Vector2(GoPos.x, GoPos.y + .5f);
                m_rigidBody2D.MovePosition(spawnPos);
                gameObject.SetActive(true);
                var increment = 0f;
                while (true)
                {
                    increment += 0.01f * Time.fixedDeltaTime;
                    m_spriteRenderer.color = new Color(1, 1, 1, increment);

                    if (increment >= 1)
                    {
                        m_spriteRenderer.color = new Color(1, 1, 1, 1);
                        m_hitPoints = 1;
                        return Task.FromResult(true);
                    }

                    Task.Yield();
                }
            }
            else if (CheckWall.Down == false)
            {
                var spawnPos = new Vector2(GoPos.x, GoPos.y - .5f);
                m_rigidBody2D.MovePosition(spawnPos);
                gameObject.SetActive(true);
                var increment = 0f;
                while (true)
                {
                    increment += 0.01f * Time.fixedDeltaTime;
                    m_spriteRenderer.color = new Color(1, 1, 1, increment);

                    if (increment >= 1)
                    {
                        m_spriteRenderer.color = new Color(1, 1, 1, 1);
                        m_hitPoints = 1;
                        return Task.FromResult(true);
                    }

                    Task.Yield();
                }
            }
            else if (CheckWall.Right == false)
            {
                var spawnPos = new Vector2(GoPos.x + .5f, GoPos.y);
                Debug.Log(spawnPos + " spawn pos");
                m_rigidBody2D.transform.position = spawnPos;
                gameObject.SetActive(true);
                var increment = 0f;
                while (true)
                {
                    increment += 0.01f * Time.fixedDeltaTime;
                    m_spriteRenderer.color = new Color(1, 1, 1, increment);

                    if (increment >= 1)
                    {
                        m_spriteRenderer.color = new Color(1, 1, 1, 1);
                        m_hitPoints = 1;
                        return Task.FromResult(true);
                    }

                    Task.Yield();
                }
            
            }
            else if (CheckWall.Left == false)
            {
                var spawnPos = new Vector2(GoPos.x - .5f, GoPos.y);
                m_rigidBody2D.MovePosition(spawnPos);
                gameObject.SetActive(true);
                var increment = 0f;
                while (true)
                {
                    increment += 0.01f * Time.fixedDeltaTime;
                    m_spriteRenderer.color = new Color(1, 1, 1, increment);

                    if (increment >= 1)
                    {
                        m_spriteRenderer.color = new Color(1, 1, 1, 1);
                        m_hitPoints = 1;
                        return Task.FromResult(true);
                    }

                    Task.Yield();
                }
            }
            return Task.FromResult(false);
        }
    }
}
