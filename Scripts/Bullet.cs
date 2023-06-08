using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 40;
    public Rigidbody2D rb;
    public GameObject impactEffect;

    // Use this for initialization
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (!hitInfo.name.Equals("Border_Cam") && !hitInfo.name.Equals("Player"))
        {
            //Debug.Log(hitInfo.name);
            Destroy(gameObject);
        }
        if (hitInfo.name.Equals("Zombie"))
        {
            hitInfo.gameObject.GetComponent<Zombie>().ManageZombieHealth(damage);
        }
    }
}
