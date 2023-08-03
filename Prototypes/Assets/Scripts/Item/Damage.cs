using DuRound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuRound
{
    public class Damage : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Guard"))
            {
                collision.GetComponent<Guard>().StopMoving();
                GetComponent<PickUp>().enabled = true;
            }
        }
    }
}
