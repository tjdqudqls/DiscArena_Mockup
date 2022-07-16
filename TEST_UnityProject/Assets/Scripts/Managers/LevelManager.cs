using System;
using System.IO;
using Controllers;
using Data;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public int currentLevel = 1;
        public static LevelManager Instance { get; private set; }
        public Material normalPuckMat;
        public Material specialPuckMat;
        public Transform spawnLocation;
        private GameObject _currentPuck;

        private GameData _gameData;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            LoadGameData();
        }

        private void Start()
        {
            StartNewGame();
        }

        public void StartNewGame()
        {
            currentLevel = 1;
            ClearObjects();
            PopupController.CreatePopup("Disc Arena", "Start", () =>
            {
                PlayerManager.Instance.ResetPlayerState();
                InstantiateEnemies();
            });
        }

        public void ClearedLevel()
        {
            PopupController.CreatePopup(String.Format("LEVEL{0} CLEARED",currentLevel), "Next Level", LoadNextLevel);
        }
        public void LoadNextLevel()
        {
            currentLevel++;
            if (currentLevel == 4)
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
            Destroy(_currentPuck);
            _currentPuck = Instantiate(Resources.Load("PlayerPuck") as GameObject, spawnLocation.position, Quaternion.Euler(-90,0,0));
            switch (type)
            {
                case PuckType.Normal:
                    PuckData normalData = new PuckData(25f, 30f, normalPuckMat);
                    _currentPuck.GetComponent<PuckController>().Init(normalData, PlayerManager.Instance.discLeft);
                    break;
                case PuckType.Special:
                    PuckData specialData = new PuckData(50f, 50f, specialPuckMat);
                    _currentPuck.GetComponent<PuckController>().Init(specialData, PlayerManager.Instance.discLeft);
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
            _gameData = JsonUtility.FromJson<GameData>(json);
        }
        public void InstantiateEnemies()
        {
            var levelData = _gameData.levels[currentLevel - 1];
            foreach (var e  in levelData.enemies)
            {
                var enemy = Instantiate(Resources.Load(e.enemyPrefabName), e.position, e.rotation);
                PhysicsSceneManager.Instance.AddPhysicsObject(enemy);
            }
        }

        public void ClearObjects()
        {
            PhysicsSceneManager.Instance.ClearEnemies();
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