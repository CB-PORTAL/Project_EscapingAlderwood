using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


namespace DuRound
{
    public class Mabel : MonoBehaviour
    {
        protected Animator m_animator { get; set; }
        protected Rigidbody2D m_rigidBody2D { get; set; }
        private bool isPickUp { get; set; } = false;
        protected bool isCollide { get; set; } = false;
        public bool hasThomas { get { return m_hasThomas; } }
        protected bool m_hasThomas { get; set; } = false;

        public float moveSpeed = 2f;
        private Vector2 m_movement { get; set; }
        private Vector2 startPosition { get; set; }
        public Action<bool> PickThomas,PickDagger;
        public static  Mabel instance { get; set; }

        protected virtual void Awake()
        {
            m_animator = GetComponent<Animator>();
            m_rigidBody2D = GetComponent<Rigidbody2D>();
            if (instance == null)
                instance = this;
        
        }
        // Start is called before the first frame update
        void Start()
        {
            startPosition = this.transform.position;
            m_animator.SetFloat("IdleY", -1);
            PickThomas += UpdateMabelUI.instance.UpdateThomas;
            PickDagger += UpdateMabelUI.instance.UpdateDagger;
        }

        // Update is called once per frame
        void Update()
        {
        }
        private void FixedUpdate()
        {
            MabelStartMove();
        }
        private void MabelStartMove()
        {
            if (m_movement.x > 0)
            {
                m_animator.SetBool("isMove", true);
                m_animator.SetFloat("IdleX", 1);
                m_animator.SetFloat("IdleY", 0);
                m_animator.SetFloat("MoveX", 1);
                m_animator.SetFloat("MoveY", 0);
                var currentPosition = m_rigidBody2D.transform.position.x + moveSpeed * Time.fixedDeltaTime;
                var newPosition = new Vector2(currentPosition, m_rigidBody2D.transform.position.y);
                m_rigidBody2D.MovePosition(newPosition);
            }
            else if (m_movement.x < 0)
            {
                m_animator.SetBool("isMove", true);
                m_animator.SetFloat("IdleX", -1);
                m_animator.SetFloat("IdleY", 0);
                m_animator.SetFloat("MoveX", -1);
                m_animator.SetFloat("MoveY", 0);
                var currentPosition = m_rigidBody2D.transform.position.x + -moveSpeed * Time.fixedDeltaTime;
                var newPosition = new Vector2(currentPosition, m_rigidBody2D.transform.position.y);
                m_rigidBody2D.MovePosition(newPosition);
            }
            else if (m_movement.y > 0)
            {
                m_animator.SetBool("isMove", true);
                m_animator.SetFloat("IdleX", 0);
                m_animator.SetFloat("IdleY", 1);
                m_animator.SetFloat("MoveY", 1);
                m_animator.SetFloat("MoveX", 0);
                var currentPosition = m_rigidBody2D.transform.position.y + moveSpeed * Time.fixedDeltaTime;
                var newPosition = new Vector2(m_rigidBody2D.transform.position.x, currentPosition);
                m_rigidBody2D.MovePosition(newPosition);
            }
            else if (m_movement.y < 0)
            {
                m_animator.SetBool("isMove", true);
                m_animator.SetFloat("IdleX", 0);
                m_animator.SetFloat("IdleY", -1);
                m_animator.SetFloat("MoveY", -1);
                m_animator.SetFloat("MoveX", 0);
                var currentPosition = m_rigidBody2D.transform.position.y + -moveSpeed * Time.fixedDeltaTime;
                var newPosition = new Vector2(m_rigidBody2D.transform.position.x, currentPosition);
                m_rigidBody2D.MovePosition(newPosition);
            }
            else
            {
                m_animator.SetBool("isMove", false);
            }

        }
        public void MabelMovement(InputAction.CallbackContext context)
        {
            m_movement = context.ReadValue<Vector2>();

        }
       // public void MabelPickUp(InputAction.CallbackContext context)
       // {
       //     if (context.performed)
       //     {
       //         if (isCollide)
       //         {
       //             //TODO Mabel pick thomas or something
       //
       //         }
       //     }
       // }
       // public void MabelPickUp()
       // {
       //     if (isCollide)
       //     {
       //         //TODO
       //     }
       // }
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
        public void RemoveThomas()
        {
            m_hasThomas = false;
            PickThomas.Invoke(false);
        }
        public void AddThomas()
        {
            m_hasThomas = true;
            PickThomas.Invoke(true);
        }
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Thomas"))
            {
                collision.gameObject.SetActive(false);
                PickThomas.Invoke(true);
            }
            else if (collision.CompareTag("Dagger"))
            {
                collision.gameObject.SetActive(false);
                PickDagger.Invoke(true);
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Guard"))
            {
                if (!m_hasThomas)
                {
                    UpdateMabelUI.instance.UpdateHealthMabel();
                    GameManager.Instance.EnableThomas();
                    StartFade();
                }

            }
        }
        private async void StartFade()
        {
            await Fade.instance.StartFade(true);
            transform.position = startPosition;
            await Fade.instance.StartFade(false);

        }

    }

}
