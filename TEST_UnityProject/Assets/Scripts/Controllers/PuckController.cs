using Data;
using Managers;
using Predictions;
using UnityEngine;

namespace Controllers
{
    public class PuckController : MonoBehaviour
    {
        private const float MinimumVelocity = 0.3f; 
        // public PuckPrediction Prediction;
        public PuckPhysicsPrediction physicsPrediction;
        public MeshRenderer meshRenderer;
        public int puckId;
        public float damage = 25f;
        public float speed = 10f;
        public Rigidbody rb;
        public bool isShot;
        public Vector3 dragStartPos;
        public Vector3 draggingPos;
        public bool isGhost;

        private Touch _touch;
        private  bool _dragging;
        public bool isStopped;

        public PuckController(Touch touch)
        {
            _touch = touch;
        }

        public void Init(PuckData data, int id, bool ghost = false)
        {
            damage = data.Damage;
            speed = data.Speed;
            meshRenderer.material = data.Look;
            puckId = id;
            isGhost = ghost;
        }

        public void Init(PuckController puck, bool ghost = false)
        {
            damage = puck.damage;
            speed = puck.speed;
            meshRenderer.material = puck.meshRenderer.material;
            puckId = puck.puckId;
            isGhost = ghost;
        }
    

        // Update is called once per frame
        void Update()
        {
            if (rb.velocity.magnitude > MinimumVelocity) //if velocity has reached above threshold then puck has been shot
            {
                isShot = true;
            }
            if (isShot && rb.velocity.magnitude <= MinimumVelocity) //to prevent forever sliding, stop puck when velocity is under threshold
            {
                rb.velocity =Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                isStopped = true;
            }

            if (puckId == 0 && isStopped) // GameOver Condition, Lastpuck and has stopped
            {
                LevelManager.Instance.GameOver();
                Destroy(gameObject);
            }
        
            if (isShot) return; //if shot disable control.
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                DragStart(Input.mousePosition);
                _dragging = true;
                return;
            }
        
            if (Input.GetMouseButtonUp(0))
            {
                DragRelease(Input.mousePosition);
                _dragging = false;
                return;
            }
            if (_dragging){
                Dragging(Input.mousePosition);
                return;
            }

            _dragging = false;
#else
        if (Input.touchCount > 0)
        {
            Debug.Log("TOUCHED");
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                DragStart(touch.position);
            }

            if (touch.phase == TouchPhase.Moved)
            {
                Dragging(touch.position);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                DragRelease(touch.position);
            }
        }
#endif
        }

        void DragStart(Vector3 inputPos)
        {
            dragStartPos = Camera.main.ScreenToViewportPoint(inputPos);
        

        }

        void Dragging(Vector3 inputPos)
        {
        
            draggingPos= Camera.main.ScreenToViewportPoint(inputPos);
            // Debug.LogFormat("DRAGGING DIR {0}", dragStartPos-draggingPos);
            // Prediction.ResetPrediction();
            //Prediction.Predict(transform.position, dragStartPos - draggingPos, 10);
            physicsPrediction.Predict();
        
        }

        void DragRelease(Vector3 inputPos)
        {
            Vector3 dragReleasePos = Camera.main.ScreenToViewportPoint(inputPos);
            Vector3 dir = dragStartPos - dragReleasePos;
            Vector3 clampedForce = dir.normalized * speed;
        
            ShootPuck(clampedForce);
            // Prediction.ResetPrediction();
            physicsPrediction.ClearPrediction();
        }

        public void ShootPuck(Vector3 f)
        {
            rb.AddForce(f, ForceMode.Impulse);
        }
    }
}
