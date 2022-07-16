using System;
using System.IO;
using UnityEngine;

namespace DefaultNamespace
{
    public class LevelManager : MonoBehaviour
    {
        public int CurrentLevel = 1;
        public static LevelManager Instance { get; private set; }
        public GameObject PuckObj;
        public Material NormalPuckMat;
        public Material SpecialPuckMat;
        public Transform SpawnLocation;
        private GameObject currentPuck;

        private GameData gameData;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            LoadGameData();
            StartNewGame();
        }

        public void StartNewGame()
        {
            CurrentLevel = 1;
            ClearObjects();
            PopupController.CreatePopup("Disc Arena", "Start", () =>
            {
                PlayerManager.Instance.ResetPlayerState();
                InstantiateEnemies();
            });
        }

        public void ClearedLevel()
        {
            PopupController.CreatePopup(String.Format("LEVEL{0} CLEARED",CurrentLevel), "Next Level", LoadNextLevel);
        }
        public void LoadNextLevel()
        {
            CurrentLevel++;
            if (CurrentLevel == 4)
            {
                PopupController.CreatePopup("FINISHED!", "Retry", StartNewGame);
                return;
            }
            
            ClearObjects();
            InstantiateEnemies();
            PlayerManager.Instance.ResetPlayerState();
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

        private void LoadGameData()
        {
            #if UNITY_EDITOR
            string path = Path.Combine(Application.streamingAssetsPath, "gameData.json");
            string json = File.ReadAllText(path);

            #elif UNITY_ANDROID
            string path = Path.Combine(Application.streamingAssetsPath, "gameData.json");
            WWW reader = new WWW(path);
            while (!reader.isDone) { }
            string json = reader.text;
            #endif
            gameData = JsonUtility.FromJson<GameData>(json);
        }
        public void InstantiateEnemies()
        {
            var levelData = gameData.levels[CurrentLevel - 1];
            foreach (var e  in levelData.enemies)
            {
                Instantiate(Resources.Load(e.enemyPrefabName), e.position, e.rotation);
            }
        }

        public void ClearObjects()
        {
            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var e in enemies)
            {
                Destroy(e);
            }

            var pucks = GameObject.FindGameObjectsWithTag("Player");
            foreach (var p in pucks)
            {
                Destroy(p);
            }
        }

        

    }
}