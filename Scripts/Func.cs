using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Func : MonoBehaviour
{
    public float mov_Speed = 40f;
    public float horizontalMove = 0f;

    [SerializeField] private Transform playerTrans;

    void Update()
    {
        //horizontalMove = Input.GetAxisRaw("Horizontal") * mov_Speed;

    }

    float DistancePlayer(Transform enemyTrans)
    {
        //if (playerTrans.transform != null)
            return (playerTrans.transform.position.x - enemyTrans.position.x);
        
    }
}
