using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DuRound
{
    public class WeaponArsenal : MonoBehaviour
    {
        private bool slot1Select, slot2Select, slot3Select, slot4Select = false;
        public bool p_slot1Select { get { return slot1Select; } }
        public bool p_slot2Select { get { return slot2Select; } }
        public bool p_slot3Select { get { return slot3Select; } }
        public bool p_slot4Select { get { return slot4Select; } }
        private Image slot1, slot2, slot3, slot4;
        private Image slot1Parent, slot2Parent, slot3Parent, slot4Parent;
        private void Awake()
        {
            slot1 = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            slot2 = transform.GetChild(1).GetChild(0).GetComponent<Image>();
            slot3 = transform.GetChild(2).GetChild(0).GetComponent<Image>();
            slot4 = transform.GetChild(3).GetChild(0).GetComponent<Image>();

            slot1Parent = transform.GetChild(0).GetComponent<Image>();
            slot2Parent = transform.GetChild(0).GetComponent<Image>();
            slot3Parent = transform.GetChild(0).GetComponent<Image>();
            slot4Parent = transform.GetChild(0).GetComponent<Image>();
        }
        //call from button
        public void Slot1Select() { if (!slot1.enabled) return; slot1Parent.color = Color.red; slot2Parent.color = Color.black; slot3Parent.color = Color.black; slot4Parent.color = Color.black; slot1Select = true; slot2Select = false; slot3Select = false; slot4Select = false; }
        public void Slot2Select() { if (!slot2.enabled) return; slot2Parent.color = Color.red;  slot1Parent.color = Color.black; slot3Parent.color = Color.black; slot4Parent.color = Color.black; slot1Select = false; slot2Select = true; slot3Select = false; slot4Select = false; }
        public void Slot3Select() { if (!slot3.enabled) return; slot2Parent.color = Color.black; slot1Parent.color = Color.black; slot3Parent.color = Color.red; slot4Parent.color = Color.black; slot1Select = false; slot2Select = false; slot3Select = true; slot4Select = false; }
        public void Slot4Select() { if (!slot4.enabled) return; slot2Parent.color = Color.black; slot1Parent.color = Color.black; slot3Parent.color = Color.black; slot4Parent.color = Color.red; slot1Select = false; slot2Select = false; slot3Select = false; slot4Select = true; }

        public void InsetSlot1(Sprite sprite) { slot1.sprite = sprite; slot1.enabled = true; }
        public void InsetSlot2(Sprite sprite) { slot2.sprite = sprite; slot2.enabled = true; }
        public void InsetSlot3(Sprite sprite) { slot3.sprite = sprite; slot3.enabled = true; }
        public void InsetSlot4(Sprite sprite) { slot4.sprite = sprite; slot4.enabled = true; }
    }
}
