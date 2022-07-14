using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    public AnimationCurve ShakeCurve;
    public float ShakeDuration = 1f; 
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

    IEnumerator Shake()
    {
        Vector3 start = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < ShakeDuration)
        {
            elapsedTime += Time.deltaTime;
            float strength = ShakeCurve.Evaluate(elapsedTime / ShakeDuration);
            transform.position = start + Random.insideUnitSphere*strength;
            yield return null;
        }

        transform.position = start;
    }
}
