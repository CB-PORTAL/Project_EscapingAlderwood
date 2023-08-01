using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;

namespace DuRound
{
    public class GuardController : MonoBehaviour
    {
        public List<Guard> guardList;
        public static GuardController instance;
        private void Awake()
        {
            if(instance == null) { instance = this; }
        }
        // Start is called before the first frame update
        void Start()
        {
            GuardOut();
        }

        // Update is called once per frame
        void Update()
        {

        }
        private async Task<Task> GuardOut()
        {
            for(int g = 0; g < guardList.Count; g++) 
            {
                await guardList [g].Initialize();
                guardList [g].GuardMoveForwards();
                await Task.Delay(3000);
            }
            return Task.CompletedTask;
        }
        public void GuardAction(bool condiion)
        {
            for (int g = 0; g < guardList.Count; g++)
            {
                guardList [g].isMoving = condiion;
            }
        }
    }
}
