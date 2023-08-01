using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuRound.MiniGame
{
    public class GuardWalk : MonoBehaviour
    {
        protected Rigidbody2D m_rigidBody2D;
        protected Vector2 startPos;
        public float moveSpeed = 2f;
        private PlayerWalk m_playerWalk;
        private bool startMove { get; set; } = false;

        protected virtual void Awake()
        {
            startPos = transform.position;
        }
        // Start is called before the first frame update
        void Start()
        {
            m_rigidBody2D = GetComponent<Rigidbody2D>();    
            m_playerWalk = transform.parent.GetChild(2).GetComponent<PlayerWalk>();
            startMove = false;
        }

        // Update is called once per frame
        void Update()
        {
            var newPosition = m_rigidBody2D.position.x + moveSpeed * Time.deltaTime;
            m_rigidBody2D.transform.position = new Vector2(newPosition, m_rigidBody2D.position.y);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Exit"))
            {
                //TODO player lost the guard
                var cg = transform.parent.parent.GetComponent<CanvasGroup>();
                cg.alpha = 0;
                cg.interactable = false;
                cg.blocksRaycasts = false;
                ResetPosition();
                m_playerWalk.ResetPosition();
            }
        }

        public void ResetPosition()
        {
            startMove = false;
            transform.position = startPos;

        }
        public void StartMove()
        {
            startMove = true;
        }
    }
}
