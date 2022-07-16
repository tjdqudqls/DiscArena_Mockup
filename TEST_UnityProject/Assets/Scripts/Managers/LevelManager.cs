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

        /// <summary>
        /// Sets the level for new game.
        /// Reset current level state to 1
        /// Clears all Objects.
        /// And instantiate enemies.
        /// </summary>
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

        /// <summary>
        /// called when level is cleared.
        /// Creates popup to move to next level.
        /// </summary>
        public void ClearedLevel()
        {
            PopupController.CreatePopup(String.Format("LEVEL{0} CLEARED",currentLevel), "Next Level", LoadNextLevel);
        }
        
        /// <summary>
        /// Increase current level
        /// Clear current objects
        /// Instantiate New level data.
        /// Reset Playerdata for new level.
        /// Check if this is last level.
        /// </summary>
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
        /// <summary>
        /// Instantiate a puck according to puck type.
        /// </summary>
        /// <param name="type"></param>
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

        /// <summary>
        /// Called when player failed.
        /// Creates popup to retry.
        /// </summary>
        public void GameOver()
        {
            Debug.Log("GAME OVER");
            PopupController.CreatePopup("GAME OVER", "Retry", () =>
            {
                Debug.Log("RETRY");
            });
        }

        /// <summary>
        /// Loads Game data from json created by map generator.
        /// </summary>
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
        
        /// <summary>
        /// Populates level from GameData.
        /// Instantiate Enemies to according positions
        /// </summary>
        public void InstantiateEnemies()
        {
            var levelData = _gameData.levels[currentLevel - 1];
            foreach (var e  in levelData.enemies)
            {
                var enemy = Instantiate(Resources.Load(e.enemyPrefabName), e.position, e.rotation);
                PhysicsSceneManager.Instance.AddPhysicsObject(enemy);
            }
        }

        /// <summary>
        /// Cleans the scene
        /// </summary>
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