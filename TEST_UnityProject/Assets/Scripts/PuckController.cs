using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PuckController : MonoBehaviour
{
    public PuckPrediction Prediction;

    public float Damage = 25f;
    public float power = 10f;
    public Rigidbody rb;


    public Vector3 dragStartPos;
    public Vector3 draggingPos;
    private Touch touch;

    public  bool dragging;



    

    // Update is called once per frame
    void Update()
    {
        
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
        Vector3 clampedForce = dir.normalized * power;
        
        rb.AddForce(clampedForce, ForceMode.Impulse);
    }
}
