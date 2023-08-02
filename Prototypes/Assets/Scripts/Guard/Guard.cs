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
        private CanvasGroup miniGame { get; set; }
        private bool shouldDestroy { get; set; } = false;
        private bool shouldMoveBackward = false;
        private Vector2 m_lastPosition, m_currentPosition;


        private Mabel m_Mabel { get; set; }
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


            m_Mabel = GameObject.FindWithTag("Mabel").GetComponent<Mabel>();
        }
        // Start is called before the first frame update
        public Task Initialize()
        {
            isCollide = false;
            moveIncrement = 0;
            isMoving = true;
            shouldDestroy = false;
            m_animator.SetBool("isMove", true);
            return Task.CompletedTask;
        }
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
            shouldDestroy = true;
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
                                Vector2.MoveTowards(m_rigidBody2D.position, movePos.position, moveSpeed * Time.fixedDeltaTime);

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
                            Vector2.MoveTowards(m_rigidBody2D.position, movePos.position, moveSpeed * Time.fixedDeltaTime);

                            m_currentPosition = m_rigidBody2D.position;
                            Mathf.RoundToInt(m_rigidBody2D.position.x);
                            Mathf.RoundToInt(m_rigidBody2D.position.y);
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
            shouldDestroy = false;
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
        protected async   void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Mabel"))
            { 
                isCollide = true;
                var m_Mabel = collision.gameObject.GetComponent<Mabel>();
                if (this.hasThomas)
                {
                    this.m_hasThomas = false;
                    m_Mabel.disableMovement = true;
                    GuardController.instance.GuardAction(false);
                    miniGame.alpha = 1;
                    miniGame.interactable = true;
                    miniGame.blocksRaycasts = true;
                    var cgPanel = miniGame.transform.GetChild(0).GetComponent<CanvasGroup>();
                    cgPanel.alpha = 1;
                    cgPanel.blocksRaycasts = true;
                    cgPanel.interactable = true;
                    miniGame.transform.GetChild(0).GetChild(0).GetComponent<GuardWalk>().StartMove();
                    GuardController.instance.CurrentGuardHasThomas(this);
                    return;
                }
                if (m_Mabel.hasThomas)
                {
                    moveSpeed = .8f;
                    m_Mabel.RemoveThomas();
                    this.m_hasThomas = true;
                    moveIncrement--;
                    shouldMoveBackward = false;
                    GuardMoveExit();
                }
                else
                {
                    _miniCanvas.alpha = 1;
                    var fadeIn = await Fade.instance.StartFade(true);
                    if (fadeIn.IsCompleted)
                    {
                        var text = _miniCanvas.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                        text.enabled = true;


                        if (UpdateMabelUI.instance._health < UpdateMabelUI.instance.maxHealth)
                        {
                            text.text = "The Guard Caught Mabel";
                            UpdateMabelUI.instance.UpdateHealthMabel();
                            GameManager.Instance.EnableThomas();
                            m_Mabel.ResetPosition();

                            GuardController.instance.ResetAllGuard();
                            await Fade.instance.StartFade(false);
                            _miniCanvas.alpha = 0;
                            text.enabled = false;
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
                    shouldDestroy = true; shouldMoveBackward = false;
                    m_hasThomas = false;
                    _miniCanvas.alpha = 1;
                    var fadeIn = await Fade.instance.StartFade(true);
                    if (fadeIn.IsCompleted)
                    {
                        var text = _miniCanvas.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                        text.enabled = true;


                        if (UpdateMabelUI.instance._health < UpdateMabelUI.instance.maxHealth)
                        {
                            text.text = "The Guard Has Exit and Lost Thomas";
                            UpdateMabelUI.instance.UpdateHealthMabel();
                            GameManager.Instance.EnableThomas();
                            m_Mabel.ResetPosition();

                            GuardController.instance.ResetAllGuard();
                            await Fade.instance.StartFade(false);
                            _miniCanvas.alpha = 0;
                            text.enabled = false;
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
        
    }
}
