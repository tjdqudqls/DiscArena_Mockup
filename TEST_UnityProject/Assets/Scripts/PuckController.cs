using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class PuckController : MonoBehaviour
{
    private const float MINIMUM_VELOCITY = 0.3f; 
    // public PuckPrediction Prediction;
    public PuckPhysicsPrediction PhysicsPrediction;
    public MeshRenderer MeshRenderer;
    public int PuckId;
    public float Damage = 25f;
    public float Speed = 10f;
    public Rigidbody rb;
    public bool isShot = false;
    public Vector3 dragStartPos;
    public Vector3 draggingPos;
    public bool isGhost = false;

    private Touch touch;
    private  bool dragging;
    public bool isStopped;
    public void Init(PuckData data, int id, bool ghost = false)
    {
        Damage = data.Damage;
        Speed = data.Speed;
        MeshRenderer.material = data.Look;
        PuckId = id;
        isGhost = ghost;
    }

    public void Init(PuckController puck, bool ghost = false)
    {
        Damage = puck.Damage;
        Speed = puck.Speed;
        MeshRenderer.material = puck.MeshRenderer.material;
        PuckId = puck.PuckId;
        isGhost = ghost;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude > MINIMUM_VELOCITY) //if velocity has reached above threshold then puck has been shot
        {
            isShot = true;
        }
        if (isShot && rb.velocity.magnitude <= MINIMUM_VELOCITY) //to prevent forever sliding, stop puck when velocity is under threshold
        {
            rb.velocity =Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            isStopped = true;
        }

        if (PuckId == 0 && isStopped) // GameOver Condition, Lastpuck and has stopped
        {
            LevelManager.Instance.GameOver();
            Destroy(gameObject);
        }
        
        if (isShot) return; //if shot disable control.
        #if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            DragStart(Input.mousePosition);
            dragging = true;
            return;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            DragRelease(Input.mousePosition);
            dragging = false;
            return;
        }
        if (dragging){
            Dragging(Input.mousePosition);
            return;
        }

        dragging = false;
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
        PhysicsPrediction.Predict();
        
    }

    void DragRelease(Vector3 inputPos)
    {
        Vector3 dragReleasePos = Camera.main.ScreenToViewportPoint(inputPos);
        Vector3 dir = dragStartPos - dragReleasePos;
        Vector3 clampedForce = dir.normalized * Speed;
        
        ShootPuck(clampedForce);
        // Prediction.ResetPrediction();
        PhysicsPrediction.ClearPrediction();
    }

    public void ShootPuck(Vector3 f)
    {
        rb.AddForce(f, ForceMode.Impulse);
    }
}
