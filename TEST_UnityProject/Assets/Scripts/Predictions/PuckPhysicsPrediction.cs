using Controllers;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Predictions
{
    public class PuckPhysicsPrediction : MonoBehaviour
    {
        public LineRenderer line;
        public int maxIteration = 100;

        public PuckController puck;
        
        /// <summary>
        /// Predicts Puck shoot path by Physics simulations.
        /// instantiating puck in physics scene
        /// and draws the Line renderer according to path.
        /// Length of prediction is determined by maxIteration
        /// </summary>
        public void Predict()
        {
            var puckTransform = puck.transform;
            var ghost = Instantiate(puck, puckTransform.position, puckTransform.rotation);
            ghost.Init(puck,true);
            SceneManager.MoveGameObjectToScene(ghost.gameObject, PhysicsSceneManager.Instance.simulationScene);
            
            ghost.ShootPuck((puck.dragStartPos - puck.draggingPos).normalized * puck.speed);
            
            line.positionCount = maxIteration;

            for (int i = 0; i < maxIteration; i++)
            {
                PhysicsSceneManager.Instance.PhysicsScene.Simulate(Time.fixedDeltaTime);
                line.SetPosition(i, ghost.transform.position);
            }
            
            Destroy(ghost.gameObject);
        }

        /// <summary>
        /// Clear Line Renderer
        /// </summary>
        public void ClearPrediction()
        {
            line.positionCount = 0;
        }
        
    }
}