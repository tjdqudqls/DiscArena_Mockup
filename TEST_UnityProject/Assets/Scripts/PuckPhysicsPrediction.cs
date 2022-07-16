using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class PuckPhysicsPrediction : MonoBehaviour
    {
        public LineRenderer Line;
        public int MaxIteration = 100;

        public PuckController Puck;
        
        public void Predict()
        {
            var transform = Puck.transform;
            var ghost = Instantiate(Puck, transform.position, transform.rotation);
            ghost.Init(Puck,true);
            SceneManager.MoveGameObjectToScene(ghost.gameObject, PhysicsSceneManager.Instance.SimulationScene);
            
            ghost.ShootPuck((Puck.dragStartPos - Puck.draggingPos).normalized * Puck.Speed);
            
            Line.positionCount = MaxIteration;

            for (int i = 0; i < MaxIteration; i++)
            {
                PhysicsSceneManager.Instance.PhysicsScene.Simulate(Time.fixedDeltaTime);
                Line.SetPosition(i, ghost.transform.position);
            }
            
            Destroy(ghost.gameObject);
        }

        public void ClearPrediction()
        {
            Line.positionCount = 0;
        }
        
    }
}