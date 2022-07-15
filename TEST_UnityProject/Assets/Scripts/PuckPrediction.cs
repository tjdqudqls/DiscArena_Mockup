using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class PuckPrediction : MonoBehaviour
    {
        public LineRenderer Line;
        private List<Vector3> lineIndices = new List<Vector3>();
        public void Predict(Vector3 startPos, Vector3 dir, float predictionLength)
        {
            lineIndices.Add(startPos);

            Ray ray = new Ray(startPos, dir);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, predictionLength))
            {
                Bounce(hit, dir, predictionLength);
            }
            else
            {
                lineIndices.Add(ray.GetPoint(predictionLength));
            }


            Line.positionCount = lineIndices.Count;
            for(int i = 0 ; i< lineIndices.Count; i++)
            {
                Line.SetPosition(i, lineIndices[i]);
            }
            
        }

        void Bounce(RaycastHit hit, Vector3 inDirection, float predictionLength)
        {
            Vector3 pos = hit.point;
            Vector3 dir = Vector3.Reflect(inDirection, hit.normal);
            Predict(pos,dir, predictionLength-hit.distance);
        }

        public void ResetPrediction()
        {
            lineIndices.Clear();
        }

    }
}