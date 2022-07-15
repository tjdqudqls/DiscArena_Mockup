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
            StartGame();
        }

        public void StartGame()
        {
            PopupController.CreatePopup("Disc Arena", "Start", ()=>{});
        }
        public void PlacePuck(PuckType type)
        {
            Destroy(currentPuck);
            currentPuck = Instantiate(Resources.Load("PlayerPuck") as GameObject, SpawnLocation.position, Quaternion.Euler(-90,0,0));
            switch (type)
            {
                case PuckType.NORMAL:
                    PuckData normalData = new PuckData(25f, 10f, NormalPuckMat);
                    currentPuck.GetComponent<PuckController>().Init(normalData, PlayerManager.Instance.DiscLeft);
                    break;
                case PuckType.SPECIAL:
                    PuckData specialData = new PuckData(50f, 15f, SpecialPuckMat);
                    currentPuck.GetComponent<PuckController>().Init(specialData, PlayerManager.Instance.DiscLeft);
                    break;
            }
        }

        public void GameOver()
        {
            Debug.Log("GAME OVER");
            PopupController.CreatePopup("GAME OVER", "Retry", () =>
            {
                Debug.Log("RETRY");
            });
        }


    }
}