using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teletra : MonoBehaviour
{
    public GameObject Teleport;
    bool isJiterring = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.name);
        if (!isJiterring && (other.name == "Player"))
        {
            Teleport.GetComponent<Teletra>().isJiterring = true;
            other.transform.position = Teleport.transform.position;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        isJiterring = false;
    }
}
