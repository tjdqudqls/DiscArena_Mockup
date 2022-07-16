using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance { get; private set; }
        public AnimationCurve shakeCurve;
        public float shakeDuration = 1f; 
        // Start is called before the first frame update
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            Instance = this;
        }


        public void OnDamage()
        {
            StartCoroutine(Shake());
        }

        /// <summary>
        /// Coroutine to Shake Camera.
        /// generates random vectors to move position during shake duration.
        /// </summary>
        /// <returns></returns>
        IEnumerator Shake()
        {
            Vector3 start = transform.position;
            float elapsedTime = 0f;

            while (elapsedTime < shakeDuration)
            {
                elapsedTime += Time.deltaTime;
                float strength = shakeCurve.Evaluate(elapsedTime / shakeDuration);
                transform.position = start + Random.insideUnitSphere*strength;
                yield return null;
            }

            transform.position = start;
        }
    }
}
