using DuRound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuRound
{
    public class Damage : MonoBehaviour
    {
        private void Start()
        {
            Invoke("DestroySelf", 5f);
        }
        private void Update()
        {
            
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Guard"))
            {
               // collision.collider.GetComponent<Guard>().StopMoving();
                collision.gameObject.SetActive(false);
                gameObject.SetActive(false);
                GetComponent<PickUp>().enabled = true;
            }
        }
        private void DestroySelf()
        {
            gameObject.SetActive(false);
        }
    }
}
