using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Func m_fun;
    private Rigidbody2D m_Rigidbody2D;
    private Animator animator;
    public PlayerMov player;
    [SerializeField] private Transform playerTrans;
    [SerializeField] float mov_Speed = 1.0f;
    int damage = -20;
    public SpriteRenderer CharacterSprite;
    public int GhostHealth = 5;
    public LayerMask detetorLayermask;
    bool canSeePlayer;
    RaycastHit2D _reWalk;




    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        m_fun = gameObject.GetComponent<Func>();
        player = gameObject.GetComponent<PlayerMov>();
    }

    // Update is called once per frame
    void Update()
    {
        _reWalk = Physics2D.Linecast(transform.position, playerTrans.position, detetorLayermask);

        if (_reWalk.collider != null)
        {
            if (_reWalk.collider.name == "Player" && _reWalk.distance < 5.0f)
            {
                canSeePlayer = true;
            }
            else
            {
                canSeePlayer = false;
            }
        }

        if (animator)
        {
            animator.SetFloat("Distance", Mathf.Abs(_reWalk.distance));
        }

        if (CharacterSprite && canSeePlayer)
        {
            if (DistancePlayer() > 0)
            {
                CharacterSprite.flipX = true;
            }
            else if (DistancePlayer() < 0)
            {
                CharacterSprite.flipX = false;
            }
        }
        if (GhostHealth == 0 || GhostHealth < 0)
        {
            Destroy(gameObject);
        }


    }

    private void FixedUpdate()
    {
        if (canSeePlayer)
        {
            m_Rigidbody2D.MovePosition(Vector2.MoveTowards(transform.position, playerTrans.position, mov_Speed * Time.deltaTime));
        }
    }

    float DistancePlayer()
    {
        return (playerTrans.transform.position.x - transform.position.x);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMov>().HealthManaager(damage);
            PlayerMov.Instance.ShakeCam(.7f, 0.1f);
        }
        if (other.gameObject.CompareTag("Bala"))
        {
            GhostHealth -= 20;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMov>().HealthManaager(damage / 10);
            PlayerMov.Instance.ShakeCam(.5f, 0.1f);

        }
    }

}

