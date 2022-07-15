using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class PlayerManager : MonoBehaviour
    {
        public static PlayerManager Instance { get; private set; }
        public int EarnedGold = 0;
        public int DiscLeft = 5;

        public bool hasUsedSpecialDisk = false;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }


        private void LateUpdate()
        {
            
        }
    }
}

