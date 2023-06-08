using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxCam : MonoBehaviour
{
    public delegate void Movement(Vector3 dx);
    public event Movement OnMovement;
    private Vector3 _oldPosition;

    void Start()
    {
        _oldPosition = transform.position;
    }

    void Update()
    {
        Vector3 movement = transform.position - _oldPosition;
        _oldPosition = transform.position;
        if (OnMovement != null && movement.magnitude > 0.0f)
        {
            OnMovement(movement);
        }
    }
}
