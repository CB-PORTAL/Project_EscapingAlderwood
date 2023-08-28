using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuRound
{

    public class MeleeAttack : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Guard"))
            {
                Debug.Log("Melee attack");
                //collision.GetComponent<Guard>().StopMoving();
            }
        }
    }
}
