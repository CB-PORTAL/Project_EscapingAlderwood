using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DuRound
{
    public class PickUp : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Mabel"))
            {
                if (tag == "Thomas")
                {
                    gameObject.SetActive(false);
                    collision.GetComponent<Mabel>().AddThomas();
                }
                else if (tag == "Dagger")
                {
                    gameObject.SetActive(false);
                    collision.GetComponent<Mabel>().AddDagger();
                }
            }
        }
    }
}
