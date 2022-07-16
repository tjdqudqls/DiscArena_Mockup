using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Managers
{
    public class PhysicsSceneManager : MonoBehaviour
    {
        public static PhysicsSceneManager Instance { get; private set; }

        public Scene simulationScene;
        public PhysicsScene PhysicsScene;
        private readonly Dictionary<Transform, Transform> _inSceneEnemies = new Dictionary<Transform, Transform>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }

        void Start()
        {
            CreatePhysicsScene();
        }
        
        void Update()
        {
            foreach (var o in _inSceneEnemies)
            {
                o.Value.position = o.Key.position;
                o.Value.rotation = o.Key.rotation;
            }
        }
        void CreatePhysicsScene()
        {
            simulationScene =
                SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));var walls =GameObject.FindGameObjectsWithTag("Wall");
            PhysicsScene = simulationScene.GetPhysicsScene();
            foreach (var w in walls)
            {
                var ghost = Instantiate(w.gameObject, w.transform.position, w.transform.rotation);
                ghost.GetComponent<Renderer>().enabled = false;
                SceneManager.MoveGameObjectToScene(ghost, simulationScene);
            }
        }

        public void AddPhysicsObject(Object obj)
        {
            var go = obj as GameObject;
            var ghost = Instantiate(go, go.transform.position, go.transform.rotation);
            var children = ghost.GetComponentsInChildren<Renderer>();
            foreach (var m in children)
            {
                m.enabled = false;
            }

            var healthbar = ghost.GetComponentInChildren<Canvas>();
            healthbar.enabled = false;
            
            // var fire = ghost.GetComponentInChildren<ParticleSystem>();
            // fire.Stop();
            

            SceneManager.MoveGameObjectToScene(ghost, simulationScene);
            _inSceneEnemies.Add(go.transform, ghost.transform);
        }

        public void ClearEnemies()
        {
            foreach (var v in _inSceneEnemies)
            {
                var g = v.Value;
                Destroy(g.gameObject);
            }
            _inSceneEnemies.Clear();
        }

        public void DestroyEnemy(GameObject e)
        {
            var ghost = _inSceneEnemies[e.transform];
            _inSceneEnemies.Remove(e.transform);
            Destroy(ghost.gameObject);
        }
    }
}