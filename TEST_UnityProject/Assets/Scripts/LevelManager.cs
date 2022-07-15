using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }
        public GameObject PuckObj;
        public Material NormalPuckMat;
        public Material SpecialPuckMat;
        public Transform SpawnLocation;
        private GameObject currentPuck;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }


        public void PlacePuck(PuckType type)
        {
            Destroy(currentPuck);
            switch (type)
            {
                case PuckType.NORMAL:
                    PuckData normalData = new PuckData(25f, 10f, NormalPuckMat);
                    currentPuck = Instantiate(PuckObj, SpawnLocation.position, Quaternion.Euler(-90,0,0));
                    currentPuck.GetComponent<PuckController>().Init(normalData);
                    break;
                case PuckType.SPECIAL:
                    PuckData specialData = new PuckData(50f, 15f, SpecialPuckMat);
                    currentPuck = Instantiate(PuckObj, SpawnLocation.position, Quaternion.Euler(-90,0,0));
                    currentPuck.GetComponent<PuckController>().Init(specialData);
                    break;
            }
        }
        
        
    }
}