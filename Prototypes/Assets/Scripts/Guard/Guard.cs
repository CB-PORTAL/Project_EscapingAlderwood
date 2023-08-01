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

        protected override void Awake()
        {
            base.Awake();
        }
        // Start is called before the first frame update
        public Task Initialize()
        {
            isCollide = false;
            moveIncrement = 0;
            isMoving = true;
            miniGame = GameObject.Find("MiniGame").GetComponent<CanvasGroup>();
            var movePointLength = parentMovePoint.childCount;
            movePoints = new Transform [movePointLength];
            for(int p = 0; p < movePointLength; p++) 
            {
                movePoints [p] = parentMovePoint.GetChild(p);
            }
            return Task.CompletedTask;
        }

        // Update is called once per frame
        void Update()
        {

        }
        public async Task<Task> GuardMoveForwards()
        {
            while (!isCollide)
            {
                if (moveIncrement < movePoints.Length)
                {
                    var movePos = movePoints [moveIncrement];
                    if (m_rigidBody2D.position != (Vector2)movePos.position)
                    {
                        if (isMoving)
                        {
                            Vector2.MoveTowards(m_rigidBody2D.transform.position, movePos.position, moveSpeed * Time.fixedDeltaTime);
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
                }

                await Task.Yield();
            }
            return Task.CompletedTask;
        }
        private async Task<Task> GuardMoveExit()
        {
            while (isCollide) 
            {
                var movePos = movePoints [moveIncrement];
                if (m_rigidBody2D.position != (Vector2)movePos.position)
                {
                    if (isMoving)
                    {
                        Vector2.MoveTowards(m_rigidBody2D.transform.position, movePos.position, moveSpeed * Time.fixedDeltaTime);
                    }
                }
                else
                {
                    moveIncrement--;
                }
                if(m_rigidBody2D.position == (Vector2)movePoints [0].position) 
                {
                    isCollide = false;
                }
                await Task.Yield();
            }
            return Task.CompletedTask;
        }
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Mabel"))
            {
                isCollide = true;
                var m_Mabel = collision.gameObject.GetComponent<Mabel>();
                if (m_Mabel.hasThomas)
                {
                    m_Mabel.RemoveThomas();
                    m_hasThomas = true;
                    moveIncrement--;

                    GuardMoveExit();
                }
                else
                {
                    GuardController.instance.GuardAction(false);
                    miniGame.alpha = 1;
                    miniGame.interactable = true;
                    miniGame.blocksRaycasts = true;
                    miniGame.transform.GetChild(0).GetChild(0).GetComponent<GuardWalk>().StartMove();
                }
            }
        }
    }
}
