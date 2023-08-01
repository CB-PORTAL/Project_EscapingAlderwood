using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace DuRound
{
    public class GuardController : MonoBehaviour
    {
        public List<Guard> guardList;
        private CancellationToken ct;
        public static GuardController instance;
        private void Awake()
        {
            if(instance == null) { instance = this; }
        }
        // Start is called before the first frame update
        async void Start()
        {
            await GuardOut();
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnDestroy()
        {
        }
        private async Task<Task> GuardOut()
        {
            await Task.Delay(3000);
            for(int g = 0; g < guardList.Count; g++) 
            {
                await guardList [g].Initialize();
#pragma warning disable CS4014
                 guardList [g].GuardMoveForwards();

#pragma warning restore CS4014

                await Task.Delay(3000);
            }
            return Task.CompletedTask;
        }
        public void GuardAction(bool condition)
        {
            for (int g = 0; g < guardList.Count; g++)
            {
                guardList [g].isMoving = condition;
            }
        }
        public async void ResetAllGuard()
        {
            for (int g = 0; g < guardList.Count; g++)
            {
                guardList [g].isMoving = false;
                guardList [g].ResetPosition();
            }
            await GuardOut();
        }
    }
}
