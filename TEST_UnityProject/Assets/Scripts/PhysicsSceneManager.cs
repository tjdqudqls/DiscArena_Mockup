using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace DefaultNamespace
{
    public class PhysicsSceneManager : MonoBehaviour
    {
        public static PhysicsSceneManager Instance { get; private set; }

        public Scene SimulationScene;
        public PhysicsScene PhysicsScene;
        [SerializeField]private List<GameObject> walls = new List<GameObject>();
        private List<GameObject> Enemies = new List<GameObject>();
        private Dictionary<Transform, Transform> inSceneEnemies = new Dictionary<Transform, Transform>();

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
            foreach (var o in inSceneEnemies)
            {
                o.Value.position = o.Key.position;
                o.Value.rotation = o.Key.rotation;
            }
        }
        void CreatePhysicsScene()
        {
            SimulationScene =
                SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));var walls =GameObject.FindGameObjectsWithTag("Wall");
            PhysicsScene = SimulationScene.GetPhysicsScene();
            foreach (var w in walls)
            {
                var ghost = Instantiate(w.gameObject, w.transform.position, w.transform.rotation);
                ghost.GetComponent<Renderer>().enabled = false;
                SceneManager.MoveGameObjectToScene(ghost, SimulationScene);
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
            

            SceneManager.MoveGameObjectToScene(ghost, SimulationScene);
            inSceneEnemies.Add(go.transform, ghost.transform);
        }

        public void ClearEnemies()
        {
            foreach (var v in inSceneEnemies)
            {
                var g = v.Value;
                Destroy(g.gameObject);
            }
            inSceneEnemies.Clear();
        }

        public void DestroyEnemy(GameObject e)
        {
            var ghost = inSceneEnemies[e.transform];
            inSceneEnemies.Remove(e.transform);
            Destroy(ghost.gameObject);
        }
    }
}