using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace DuRound
{
    public class GuardController : MonoBehaviour
    {
        public List<Guard> guardList;
        public static GuardController instance;
        private Guard currentGuardHT;
        private Mabel m_Mabel { get; set; }
        public float resetMovement = 0.5f;
        private void Awake()
        {
            if(instance == null) { instance = this; }
            m_Mabel = GameObject.FindWithTag("Mabel").GetComponent<Mabel>();
        }

        // Start is called before the first frame update
        public async void Start()
        {
            await GuardStart();
        }
        private async Task<Task> GuardStart()
        {
            await GuardOut();
            await Task.Delay(3000);
            SetMovementSpeedAllGuard();
            return Task.CompletedTask;
        }
        // Update is called once per frame
        void Update()
        {

        }
        private void OnDestroy()
        {
            currentGuardHT = null; 
            guardList.Clear();
        }
        private async Task GuardOut()
        {
            await Task.Delay(1000);
            for(int g = 0; g < guardList.Count; g++) 
            {
                await guardList [g].Initialize();
                guardList [g].GuardMoveForwards();


                await Task.Delay(3000);
            }
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
                guardList [g].RemoveThomas();
                guardList [g].isMoving = false;
                guardList [g].ResetPosition();
                
            }
            await GuardOut();
        }
        public void SetMovementSpeedAllGuard()
        {
            for (int g = 0; g < guardList.Count; g++)
            {
                guardList [g].isMoving = true;
                guardList [g].moveSpeed = resetMovement;
            }
        }
        public void CurrentGuardHasThomas(Guard guard)
        {
            currentGuardHT = null;
            currentGuardHT = guard;
        }
        public void ResetCurrentGuard()
        {
            currentGuardHT.ResetPosition();
            currentGuardHT.GuardMoveForwards();
        }
        public void AddThomas()
        {
            currentGuardHT.AddThomas();
        }
    }
}
