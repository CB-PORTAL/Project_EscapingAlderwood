using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuRound
{
    public class PrisonGate : MonoBehaviour
    {
        private SpriteRenderer m_sprite;
        private void Awake()
        {
            m_sprite = GetComponent<SpriteRenderer>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Mabel"))
            {
                m_sprite.enabled = false;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.CompareTag("Mabel"))
            {
                m_sprite.enabled = true;
            }
        }
    }
}
