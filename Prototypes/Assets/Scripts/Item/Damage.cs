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
            
        }
        private void Update()
        {
            
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Guard"))
            {
                collision.collider.GetComponent<Guard>().StopMoving();
                GetComponent<PickUp>().enabled = true;
            }
        }
    }
}
