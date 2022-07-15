using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class PuckController : MonoBehaviour
{
    private const float MINIMUM_VELOCITY = 0.3f; 
    public PuckPrediction Prediction;

    public float Damage = 25f;
    public float Speed = 10f;
    public Rigidbody rb;
    public bool isShot = false;
    public Vector3 dragStartPos;
    public Vector3 draggingPos;
    
    private Touch touch;
    private  bool dragging;
    
    public void Init(PuckData data)
    {
        Damage = data.Damage;
        Speed = data.Speed;
        gameObject.GetComponent<MeshRenderer>().material = data.Look;
    }

    

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude <= MINIMUM_VELOCITY)
        {
            rb.velocity =Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (isShot) return;
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
        Prediction.ResetPrediction();
        Prediction.Predict(transform.position, dragStartPos - draggingPos, 10);
    }

    void DragRelease(Vector3 inputPos)
    {
        Vector3 dragReleasePos = Camera.main.ScreenToViewportPoint(inputPos);
        Vector3 dir = dragStartPos - dragReleasePos;
        Vector3 clampedForce = dir.normalized * Speed;
        
        rb.AddForce(clampedForce, ForceMode.Impulse);
        Prediction.ResetPrediction();
        isShot = true;
    }
}
