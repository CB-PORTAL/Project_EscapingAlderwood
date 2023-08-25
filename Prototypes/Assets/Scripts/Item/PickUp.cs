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
        private bool tempIsPickWeapon { get; set; } = false;
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Mabel"))
            {
                if (tag == "Thomas")
                {
                    gameObject.SetActive(false);
                    collision.collider.GetComponent<Mabel>().AddThomas();
                }
                else if (tag == "Dagger")
                {
                    if (!tempIsPickWeapon)
                    {
                        if (GameManager.Instance.isBegin)
                        {
                            tempIsPickWeapon = true;
                            Instruction.instance.SetPickWeapon();
                        }
                    }
                    gameObject.SetActive(false);
                    collision.collider.GetComponent<Mabel>().AddDagger();
                }
            }
        }
    }
}
