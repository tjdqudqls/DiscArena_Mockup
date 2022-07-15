using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    [SerializeField]
    public class GameData
    {
        public List<LevelData> levels;
    }
    [Serializable]
    public class LevelData
    {
        public List<EnemyMapData> enemies;

        public LevelData()
        {
            enemies = new List<EnemyMapData>();
        }
    }
    [Serializable]
    public class EnemyMapData
    {
        public string enemyPrefabName;
        public Vector3 position;
        public Quaternion rotation;

        public EnemyMapData(string name, Vector3 pos, Quaternion rot)
        {
            enemyPrefabName = name;
            position = pos;
            rotation = rot;
        }
    }
}