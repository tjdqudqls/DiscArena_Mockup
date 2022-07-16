using System.Collections.Generic;
using UnityEngine;

namespace Predictions
{
    public class PuckPrediction : MonoBehaviour
    {
        public LineRenderer line;
        private readonly List<Vector3> _lineIndices = new List<Vector3>();
        /// <summary>
        /// Recursive method to predict puck shoot path.
        /// used Raycasting to objects.
        /// </summary>
        /// <param name="startPos"></param>
        /// <param name="dir"></param>
        /// <param name="predictionLength"></param>
        public void Predict(Vector3 startPos, Vector3 dir, float predictionLength)
        {
            _lineIndices.Add(startPos);

            Ray ray = new Ray(startPos, dir);
            if (Physics.Raycast(ray, out var hit, predictionLength))
            {
                Bounce(hit, dir, predictionLength);
            }
            else
            {
                _lineIndices.Add(ray.GetPoint(predictionLength));
            }


            line.positionCount = _lineIndices.Count;
            for(int i = 0 ; i< _lineIndices.Count; i++)
            {
                line.SetPosition(i, _lineIndices[i]);
            }
            
        }

        /// <summary>
        /// Calculate the reflected raycast.
        /// </summary>
        /// <param name="hit"></param>
        /// <param name="inDirection"></param>
        /// <param name="predictionLength"></param>
        void Bounce(RaycastHit hit, Vector3 inDirection, float predictionLength)
        {
            Vector3 pos = hit.point;
            Vector3 dir = Vector3.Reflect(inDirection, hit.normal);
            Predict(pos,dir, predictionLength-hit.distance);
        }

        public void ResetPrediction()
        {
            _lineIndices.Clear();
            line.positionCount = 0;
            
        }

    }
}