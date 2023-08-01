using DuRound;
using DuRound.MiniGame;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private CanvasGroup miniGame { get; set; }
        private bool shouldDestroy { get; set; } = false;
        private bool shouldMoveBackward = false;
        private Vector2 m_lastPosition, m_currentPosition;
        

        protected override void Awake()
        {
            base.Awake();
            miniGame = GameObject.Find("MiniGame").GetComponent<CanvasGroup>();
            var movePointLength = parentMovePoint.childCount;
            movePoints = new Transform [movePointLength];
            for (int p = 0; p < movePointLength; p++)
            {
                movePoints [p] = parentMovePoint.GetChild(p);
            }
        }
        // Start is called before the first frame update
        public Task Initialize()
        {
            isCollide = false;
            moveIncrement = 0;
            isMoving = true;
            shouldDestroy = false;

            return Task.CompletedTask;
        }
        public bool isDebug = false;
        // Update is called once per frame
        void Update()
        {
            if (m_currentPosition.x > m_lastPosition.x)
            {
                m_animator.SetBool("isMove", true);
                m_animator.SetFloat("MoveX", 1);
                m_animator.SetFloat("MoveY", 0);
                m_animator.SetFloat("IdleY", 0);
            }
            else if (m_currentPosition.x < m_lastPosition.x)
            {
                m_animator.SetBool("isMove", true);
                m_animator.SetFloat("MoveX", -1);
                m_animator.SetFloat("MoveY", 0);
                m_animator.SetFloat("IdleY", 0);
            }
            else  if (m_currentPosition.y > m_lastPosition.y)
            {
                m_animator.SetBool("isMove", true);
                m_animator.SetFloat("MoveX", 0);
                m_animator.SetFloat("MoveY", 1);
                m_animator.SetFloat("IdleY", 0);
            }
            else if (m_currentPosition.y < m_lastPosition.y)
            {
                m_animator.SetBool("isMove", true);
                m_animator.SetFloat("MoveX", 0);
                m_animator.SetFloat("MoveY", -1);
                m_animator.SetFloat("IdleY", 0);
            }
            else
            {
                //TODO but the guard is always move no stop
                m_animator.SetBool("isMove", false);
                m_animator.SetFloat("MoveX", 0);
                m_animator.SetFloat("IdleY", 1);
            }
        }
        private void OnDestroy()
        {
            shouldDestroy = true;
        }
        public async void GuardMoveForwards()
        {
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
                                Vector2.MoveTowards(m_rigidBody2D.position, movePos.position, moveSpeed * Time.fixedDeltaTime);
                            await Task.Yield();
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
                    await Task.Delay(10000);
                    shouldMoveBackward = true;
                    moveIncrement--;
#pragma warning disable CS4014
                    GuardMoveBackwards();

#pragma warning restore CS4014                    
                    break;

                }


            }
        }
        private async Task<Task> GuardMoveBackwards()
        {
            while (shouldMoveBackward && !shouldDestroy)
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
                            await Task.Yield();
                            m_currentPosition = m_rigidBody2D.position;
                        }
                    }
                    else
                    {
                        moveIncrement--;
                    }
                    if (moveIncrement == 0)
                    {
                        shouldMoveBackward = false;
                        await Task.Delay(10000);
                        isCollide = false;
                        GuardMoveForwards();
                        break;
                    }

                }
            }
            return Task.CompletedTask;
        }
        private async Task<Task> GuardMoveExit()
        {
            while (isCollide && !shouldDestroy) 
            {
                if (moveIncrement >= 0 && moveIncrement < movePoints.Length)
                {
                    var movePos = movePoints [moveIncrement]; 
                    if (m_rigidBody2D.position != (Vector2)movePos.position)
                    {
                        if (isMoving)
                        {
                            m_lastPosition = m_rigidBody2D.position;
                            m_rigidBody2D.position =
                            Vector2.MoveTowards(m_rigidBody2D.transform.position, movePos.position, moveSpeed * Time.fixedDeltaTime);
                            await Task.Yield();
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
                        //TODO GUARD WIN
                        break;
                    }

                }
            }
            return Task.CompletedTask;
        }
        protected async void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Mabel"))
            {
                isCollide = true;
                var m_Mabel = collision.gameObject.GetComponent<Mabel>();
                if (m_hasThomas)
                {

                    GuardController.instance.GuardAction(false);
                    miniGame.alpha = 1;
                    miniGame.interactable = true;
                    miniGame.blocksRaycasts = true;
                    miniGame.transform.GetChild(0).GetChild(0).GetComponent<GuardWalk>().StartMove();
                    ResetPosition();
                    m_hasThomas = false;
                    return;
                }
                if (m_Mabel.hasThomas)
                {
                    moveSpeed = .5f;
                    m_Mabel.RemoveThomas();
                    m_hasThomas = true;
                    moveIncrement--;
                    shouldMoveBackward = false;
                    await GuardMoveExit();
                }
                else if (!m_Mabel.hasThomas)
                {
                    GuardController.instance.ResetAllGuard();
                    UpdateMabelUI.instance.UpdateHealthMabel();
                    GameManager.Instance.EnableThomas();
                    m_Mabel.StartFade();
                }

            }
        }
        public void ResetPosition()
        {
            shouldDestroy = true;shouldMoveBackward = false;isCollide = false;
            m_rigidBody2D.position = (Vector2)movePoints [0].position;
            moveIncrement = 0;
            shouldDestroy = false;
        }
    }
}
