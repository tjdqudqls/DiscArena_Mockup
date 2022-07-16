using System.IO;
using Data;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class MapDataGenerator : MonoBehaviour
    {
        [MenuItem("DiscArena/Generate Level Json")]
        static void GenerateLevelJson()
        {
            string path = Path.Combine(Application.streamingAssetsPath, "gameData.json");

            string json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<GameData>(json);

            var newLevel = new LevelData();
            data.levels.Add(newLevel);
            
            var objs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var o in objs)
            {
                var prefab = PrefabUtility.GetCorrespondingObjectFromSource(o);
                if (prefab != null)
                {
                    newLevel.enemies.Add(new EnemyMapData(prefab.name, o.transform.position, o.transform.rotation));
                }
            }

            File.WriteAllText(path,JsonUtility.ToJson(data,true));

        }
    }

   
}