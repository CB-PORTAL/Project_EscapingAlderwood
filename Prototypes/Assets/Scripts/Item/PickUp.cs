using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuRound
{
    public class PickUp : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            if (GameManager.Instance.isBegin)
            {
                tempIsPickWeapon = false;
            }
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
                    var sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                    if (!tempIsPickWeapon)
                    {
                        if (GameManager.Instance.isBegin)
                        {
                            tempIsPickWeapon = true;
                            Instruction.instance.SetPickWeapon();
                        }
                    }
                    gameObject.SetActive(false);
                    WeaponArsenal.instance.InsetSlot1(sprite);
                    //collision.collider.GetComponent<Mabel>().AddDagger();
                }
            }
        }
    }
}
