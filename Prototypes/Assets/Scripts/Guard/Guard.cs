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
            m_animator.SetBool("isMove", true);
            return Task.CompletedTask;
        }
        private void CheckHasThomas(bool condition)
        {
            //TODO guard has thomas UI stuff
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
        private async void CheckForDistance()
        {
            var distance = Vector2.Distance(m_rigidBody2D.transform.position, m_Mabel.transform.position);
            if (distance < 2)
            {
                pursueMabel = true;
                if (!currentlyPursue)
                {
                    currentlyPursue = false;
                    await PursueMabel();
                }
            }
            //else
            //{
            //    pursueMabel = false;
            //}
        }
        private bool pursueMabel { get; set; } = false;
        private bool currentlyPursue { get; set; } = false;
        private async Task<Task> PursueMabel()
        {
            while (pursueMabel)
            {
                isCollide = false; shouldDestroy = true;
                shouldMoveBackward = false;
                m_rigidBody2D.transform.position = Vector2.MoveTowards(m_rigidBody2D.transform.position, m_Mabel.transform.position, moveSpeed * Time.fixedDeltaTime);
                await Task.Yield();
            }
            currentlyPursue = false;
            return Task.CompletedTask;
        }
        protected override void FixedUpdate()
        {
            CheckForDistance();
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
