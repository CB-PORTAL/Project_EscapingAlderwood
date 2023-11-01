using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.Utilities;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace DuRound
{
    public class Mabel : MonoBehaviour
    {
        protected Animator m_animator { get; set; }
        protected Rigidbody2D m_rigidBody2D { get; set; }
        protected bool isCollide { get; set; } = false;
        public int m_hitPoints { get; set; } = 3;
        public bool hasThomas { get { return m_hasThomas; } }
        public bool disableMovement { get; set; } = false;
        protected bool m_hasThomas { get; set; } = false;

        public float moveSpeed = 2f;
        public float shortDistanceSpeed = 5;
        public float normalSpeed;
        public float throwDaggerStrength = 20f;
        private Vector2 m_movement { get; set; }
        private Vector2 startPosition { get; set; }
        public Action<bool> PickThomas, PickDagger;
        private bool statUp, statRight, statDown, statLeft;
        private bool onGame { get; set; } = true;
        protected CanvasGroup _miniCanvas;
        private Vector2 m_currentPath { get; set; }
        public Vector2 currentPath { get { return m_currentPath; } }
        protected SpriteRenderer m_spriteRenderer { get; set; }
        protected BoxCollider2D m_boxCollider2D { get; set; }

        private GameObject [] guardList { get; set; }
        //private bool m_Mobile, m_Window = false;
        protected virtual void Awake()
        {
            _miniCanvas = GameObject.FindWithTag("MiniGame").GetComponent<CanvasGroup>();
            m_animator = GetComponent<Animator>();
            m_rigidBody2D = GetComponent<Rigidbody2D>(); startPosition = this.transform.position;
            m_spriteRenderer = GetComponent<SpriteRenderer>();m_boxCollider2D = GetComponent<BoxCollider2D>();
            m_hitPoints = 3;
            
        }
        // Start is called before the first frame update
        protected virtual void Start()
        {
            m_animator.SetFloat("IdleY", -1);
            PickThomas += UpdateMabelUI.instance.UpdateThomas;
            PickDagger += UpdateMabelUI.instance.UpdateDagger;
            m_hasThomas = false;
            disableMovement = false;
            statUp = false;
            statDown = true;
            statLeft = false;
            statRight = false;
            EnhancedTouchSupport.Enable();
            var position = ConvertIntoInteger(m_rigidBody2D.position);
            m_currentPath = position;
            m_movement = Vector2.zero;
            MabelStartMove();
            guardList = GameObject.FindGameObjectsWithTag("Guard");
            //Touch.onFingerDown += SwipeIn;
            //Touch.onFingerUp += SwipeOut;
            //moveSpeed = walkSpeed;
            if (GameManager.Instance.isBegin)
            {
                tempDetectGuard = false;
            }
            PlatformInit();
            isAnalog = 0;
        }
        private void PlatformInit()
        {
           // if (Application.isMobilePlatform) m_Mobile = true;
           // else m_Window = true;
        }
        protected Vector2 ConvertIntoInteger(Vector2 currentPos)
        {
            var x = Mathf.RoundToInt(currentPos.x);
            var y = Mathf.RoundToInt(currentPos.y);
            var newPos = new Vector2(x, y);
            return newPos;
        }
        private void UpdateThrowDagger(ReadOnlyArray<Touch> touch)
        {
            var lastPosition = Vector2.zero;
            int steps = 0;
            foreach (var t in touch)
            {
                //Debug.Log(t.screenPosition + "dagger");
                Vector2Int positionInteger = new Vector2Int(Mathf.RoundToInt(lastPosition.x), Mathf.RoundToInt(lastPosition.y));
                Vector2Int screenPosition = new Vector2Int(Mathf.RoundToInt(t.screenPosition.x), Mathf.RoundToInt(t.screenPosition.y));
                if (t.screenPosition.x > lastPosition.x && t.screenPosition.y == lastPosition.y)
                {
                    steps++;
                    if (steps > 2)
                    {
                        statRight = true;
                        statLeft = false;
                        statUp = false;
                        statDown = false;
                        ThrowDagger();
                        break;
                    }
                }
                else if (t.screenPosition.x < lastPosition.x && t.screenPosition.y == lastPosition.y)
                {
                    steps++;
                    if (steps > 2)
                    {
                        statRight = false;
                        statLeft = true;
                        statUp = false;
                        statDown = false;
                        ThrowDagger();
                        break;
                    }
                }
                else if (t.screenPosition.y > lastPosition.y && t.screenPosition.x == lastPosition.y)
                {
                    steps++;
                    if (steps > 2)
                    {
                        statRight = false;
                        statLeft = false;
                        statUp = true;
                        statDown = false;
                        ThrowDagger();
                        break;
                    }
                }
                else if (t.screenPosition.y < lastPosition.y && t.screenPosition.x == lastPosition.x)
                {
                    steps++;
                    if (steps > 2)
                    {
                        statRight = false;
                        statLeft = false;
                        statUp = false;
                        statDown = true;
                        ThrowDagger();
                        break;
                    }
                }
                lastPosition = t.screenPosition;
            }
        }
        private bool tempDetectGuard { get; set; } = false;
        private void MabelDetectGuard()
        {
            if (!tempDetectGuard)
            {
                if (GameManager.Instance.isBegin)
                {
                    foreach (var g in guardList)
                    {
                        var distance = Vector2.Distance(this.transform.position, g.transform.position);
                        if (distance <= 5)
                        {
                            tempDetectGuard = true;
                            Instruction.instance.SetAvoidingGuardText();

                        }
                    }
                }
            }
        }
        // Update is called once per frame
        protected virtual void Update()
        {
            MabelDetectGuard();
            var touch = Touch.activeTouches;
            UpdateThrowDagger(touch);
        }
        protected virtual  void FixedUpdate()
        {
           
        }
        private void OnDestroy()
        {
            onGame = false;
            PickThomas -= UpdateMabelUI.instance.UpdateThomas;
            PickDagger -= UpdateMabelUI.instance.UpdateDagger;
            
        }
        private void UpdateMabelAnimationAndMovement()
        {
            return;
            m_movement = Vector2.zero;
            var touch = Touch.activeFingers;
            
            foreach (var t in touch)
            {
                m_movement = t.screenPosition;
                //Debug.Log(m_movement + "movement");
                if (m_movement.x > 0)
                {
                    //statRight = true;
                    //statLeft = false;
                    //statUp = false;
                    //statDown = false;
                    m_animator.SetBool("isMove", true);
                    m_animator.SetFloat("IdleX", 1);
                    m_animator.SetFloat("IdleY", 0);
                    m_animator.SetFloat("MoveX", 1);
                    m_animator.SetFloat("MoveY", 0);
                    var newPosition = m_rigidBody2D.position.x + moveSpeed * Time.fixedDeltaTime;
                    var movement = new Vector2(newPosition, m_rigidBody2D.transform.position.y);
                    m_rigidBody2D.MovePosition(movement);
                }
                else if (m_movement.x < 0)
                {
                    //statLeft = true;
                    //statRight = false;
                    //statUp = false;
                    //statDown = false;
                    m_animator.SetBool("isMove", true);
                    m_animator.SetFloat("IdleX", -1);
                    m_animator.SetFloat("IdleY", 0);
                    m_animator.SetFloat("MoveX", -1);
                    m_animator.SetFloat("MoveY", 0);
                    var currentPosition = m_rigidBody2D.position.x + -moveSpeed * Time.fixedDeltaTime;
                    var movement = new Vector2(currentPosition, m_rigidBody2D.position.y);
                    m_rigidBody2D.MovePosition(movement);
                }
                else if (m_movement.y > 0)
                {
                    //statUp = true;
                    //statDown = false;
                    //statRight = false;
                    //statLeft = false;
                    m_animator.SetBool("isMove", true);
                    m_animator.SetFloat("IdleX", 0);
                    m_animator.SetFloat("IdleY", 1);
                    m_animator.SetFloat("MoveY", 1);
                    m_animator.SetFloat("MoveX", 0);
                    var currentPosition = m_rigidBody2D.position.y + moveSpeed * Time.fixedDeltaTime;
                    var movement = new Vector2(m_rigidBody2D.position.x, currentPosition);
                    m_rigidBody2D.MovePosition(movement);
                }
                else if (m_movement.y < 0)
                {
                    //statDown = true;
                    //statUp = false;
                    //statLeft = false;
                    //statRight = false;
                    m_animator.SetBool("isMove", true);
                    m_animator.SetFloat("IdleX", 0);
                    m_animator.SetFloat("IdleY", -1);
                    m_animator.SetFloat("MoveY", -1);
                    m_animator.SetFloat("MoveX", 0);
                    var currentPosition = m_rigidBody2D.position.y + -moveSpeed * Time.fixedDeltaTime;
                    var movement = new Vector2(m_rigidBody2D.position.x, currentPosition);
                    m_rigidBody2D.MovePosition(movement);
                }
            }
        }
       
        private void HorizontalMovement()
        {
            if (m_movement.x > 0)
            {
                statRight = true;
                statLeft = false;
                statUp = false;
                statDown = false;
                m_animator.SetBool("isMove", true);
                m_animator.SetFloat("IdleX", 1);
                m_animator.SetFloat("IdleY", 0);
                m_animator.SetFloat("MoveX", 1);
                m_animator.SetFloat("MoveY", 0);
                var newPosition = m_rigidBody2D.position.x + moveSpeed * Time.fixedDeltaTime;
                var movement = new Vector2(newPosition, m_rigidBody2D.transform.position.y);
                m_rigidBody2D.MovePosition(movement);
            }
            else if (m_movement.x < 0)
            {
                statLeft = true;
                statRight = false;
                statUp = false;
                statDown = false;
                m_animator.SetBool("isMove", true);
                m_animator.SetFloat("IdleX", -1);
                m_animator.SetFloat("IdleY", 0);
                m_animator.SetFloat("MoveX", -1);
                m_animator.SetFloat("MoveY", 0);
                var currentPosition = m_rigidBody2D.position.x + -moveSpeed * Time.fixedDeltaTime;
                var movement = new Vector2(currentPosition, m_rigidBody2D.position.y);
                m_rigidBody2D.MovePosition(movement);
            }
            else if (m_movement.y > 0)
            {
                statUp = true;
                statDown = false;
                statRight = false;
                statLeft = false;
                m_animator.SetBool("isMove", true);
                m_animator.SetFloat("IdleX", 0);
                m_animator.SetFloat("IdleY", 1);
                m_animator.SetFloat("MoveY", 1);
                m_animator.SetFloat("MoveX", 0);
                var currentPosition = m_rigidBody2D.position.y + moveSpeed * Time.fixedDeltaTime;
                var movement = new Vector2(m_rigidBody2D.position.x, currentPosition);
                m_rigidBody2D.MovePosition(movement);
            }
            else if (m_movement.y < 0)
            {
                statDown = true;
                statUp = false;
                statLeft = false;
                statRight = false;
                m_animator.SetBool("isMove", true);
                m_animator.SetFloat("IdleX", 0);
                m_animator.SetFloat("IdleY", -1);
                m_animator.SetFloat("MoveY", -1);
                m_animator.SetFloat("MoveX", 0);
                var currentPosition = m_rigidBody2D.position.y + -moveSpeed * Time.fixedDeltaTime;
                var movement = new Vector2(m_rigidBody2D.position.x, currentPosition);
                m_rigidBody2D.MovePosition(movement);
            }
            else
            {
                m_animator.SetBool("isMove", false);
            }
        }
        public void SetMovement(Vector2 movement)
        {
            m_movement = movement;
            //Debug.Log(m_movement);
           // Debug.LogWarning(xTemp + ", " + yTemp);
            //m_rigidBody2D.transform.position = new Vector2(xTemp, yTemp);
            if (GameManager.Instance.isBegin)
            {
               // instructionArrow.alpha = 0;
            }
        }
        private async void MabelStartMove()
        {
            while (onGame)
            {
                if (disableMovement) return;
                //if (m_Mobile)
                UpdateMabelAnimationAndMovement();
                //else if (m_Window)
                    HorizontalMovement();
                m_currentPath = ConvertIntoInteger(m_rigidBody2D.position);
                await Task.Yield();
            }
           
        }
        public void MabelMovement(InputAction.CallbackContext context)
        {
            m_movement = context.ReadValue<Vector2>();
        }
        public void MoveLeftDown(BaseEventData data)
        {
            m_movement = new Vector2(-1, 0);
        }
        public void MoveLeftUp(BaseEventData data)
        {
            m_movement = Vector2.zero;
        }
        public void MoveRightDown(BaseEventData data)
        {
            m_movement = new Vector2(1, 0);
        }
        public void MoveRightUp(BaseEventData data)
        {
            m_movement = Vector2.zero;
        }
        public void MoveUpDown(BaseEventData data)
        {
            m_movement = new Vector2(0, 1);
        }
        public void MoveUpUp(BaseEventData data)
        {
            m_movement = Vector2.zero;
        }
        public void MoveDownDown(BaseEventData data)
        {
            m_movement = new Vector2(0, -1);
        }
        public void MoveDownUp(BaseEventData data)
        {
            m_movement = Vector2.zero;
        }
        public void ThrowDagger(BaseEventData data)
        {
            if (UpdateMabelUI.instance.Dagger.gameObject.activeInHierarchy)
            {
                ThrowDagger();
            }
        }
        public int isAnalog { get; set; } = -1;
        public void OnPointerEnter(BaseEventData data)
        {
            isAnalog = 1;
        }
        public void OnPointerExit(BaseEventData data)
        {
            isAnalog = 0;
        }
        public void OnPointerUp(BaseEventData eventData)
        {
            isAnalog = -1;
            m_movement = Vector2.zero;
        }
        
        public void MeleeAttack(BaseEventData data)
        {
           // if (!m_hasDagger) return;
            if (statRight)
            {
                m_animator.SetTrigger("isAttack");
                m_animator.SetFloat("MeleeX", 1);
                m_animator.SetFloat("MeleeY", 0);
            }
            else if (statLeft)
            {
                m_animator.SetTrigger("isAttack");
                m_animator.SetFloat("MeleeX", -1);
                m_animator.SetFloat("MeleeY", 0);
            }
            else if (statUp)
            {
                m_animator.SetTrigger("isAttack");
                m_animator.SetFloat("MeleeY", 1);
                m_animator.SetFloat("MeleeX", 0);
            }
            else if (statDown)
            {
                m_animator.SetTrigger("isAttack");
                m_animator.SetFloat("MeleeY", -1);
                m_animator.SetFloat("MeleeX", 0);
            }
        }
        public GameObject dagger;
        private void ThrowDagger()
        {
            UpdateMabelUI.instance.UpdateDagger(false);
            if (statUp)
            {
                var m_dagger = Instantiate(dagger, transform.position, Quaternion.Euler(0, 0, 0));
                m_dagger.GetComponent<PickUp>().enabled = false;
                var force = new Vector2(0, throwDaggerStrength);
                var rigid = m_dagger.GetComponent<Rigidbody2D>();
                rigid.AddForce(force);
            }
            else if (statDown)
            {
                var m_dagger = Instantiate(dagger, transform.position, Quaternion.Euler(0, 0, 180));
                m_dagger.GetComponent<PickUp>().enabled = false;
                var force = new Vector2(0, throwDaggerStrength);
                var rigid = m_dagger.GetComponent<Rigidbody2D>();
                rigid.AddForce(-force);
            }
            else if (statLeft)
            {
                var m_dagger = Instantiate(dagger, transform.position, Quaternion.Euler(0, 0, 90));
                m_dagger.GetComponent<PickUp>().enabled = false;
                var force = new Vector2(throwDaggerStrength, 0);
                var rigid = m_dagger.GetComponent<Rigidbody2D>();
                rigid.AddForce(-force);
            }
            else if (statRight)
            {
                var m_dagger = Instantiate(dagger, transform.position, Quaternion.Euler(0, 0, 270));
                m_dagger.GetComponent<PickUp>().enabled = false;
                var force = new Vector2(10, 0);
                var rigid = m_dagger.GetComponent<Rigidbody2D>();
                rigid.AddForce(force);
            }

        }
        public virtual void RemoveThomas()
        {
            m_hasThomas = false;
            PickThomas?.Invoke(false);
        }
        public virtual void AddThomas()
        {
            m_hasThomas = true;
            PickThomas?.Invoke(true);
        }

        public void AddDagger()
        {
            PickDagger.Invoke(true);
        }
        public async virtual void StartFade()
        {

           await Fade.instance.StartFade(true);
           await Fade.instance.StartFade(false);
           
        }
        public bool beingCaught { get; set; } = false;
        public void  ResetPosition()
        {
            disableMovement = false;
            m_rigidBody2D.position = startPosition;
            m_animator.SetBool("isMove", false);
            m_animator.SetFloat("MoveX", 0);
            m_animator.SetFloat("MoveY", 0);
            m_animator.SetFloat("IdleX", 0);
            m_animator.SetFloat("IdleY", -1);
            beingCaught = false;
            onGame = true;
            MabelStartMove();
        }
        private async void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Exit"))
            {
                if (hasThomas)
                {
                    GuardController.instance.GuardAction(false);
                    _miniCanvas.alpha = 1;
                    var fadeIn = await Fade.instance.StartFade(true);
                    if (fadeIn.IsCompleted)
                    {
                        var text = Fade.instance.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                        text.enabled = true;
                        text.text = "You Just Save Thomas and Leave Dungeon";

                    }
                }
            }
        }

    }

}
