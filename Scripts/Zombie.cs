using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public Func m_fun;
    private Rigidbody2D m_Rigidbody2D;
    private Animator animator;
    public PlayerMov player;
    [SerializeField] private Transform playerTrans;
    [SerializeField] float mov_Speed = 30.0f;
    int damage = -20;
    public SpriteRenderer CharacterSprite;
    public int ZombieHealth = 20;
    RaycastHit2D _reWalk;
    public LayerMask detetorLayermask;
    bool canSeePlayer;
    public delegate void OnRecoil();
    public event OnRecoil ZombieDeath;

    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        m_fun = gameObject.GetComponent<Func>();
        player = gameObject.GetComponent<PlayerMov>();
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log($"ZombieHealth----->{ZombieHealth}");

        _reWalk = Physics2D.Linecast(transform.position, playerTrans.position, detetorLayermask);

        if (_reWalk.collider != null)
        {
            if (_reWalk.collider.name == "Player" && _reWalk.distance < 6.0f)
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
            animator.SetBool("IsDead", true);
            animator.SetFloat("Distance", Mathf.Abs(_reWalk.distance));
        }

        if (CharacterSprite && canSeePlayer)
        {
            if (DistancePlayer() > 0)
            {
                CharacterSprite.flipX = false;
            }
            else if (DistancePlayer() < 0)
            {
                CharacterSprite.flipX = true;
            }
        }
        if (ZombieHealth == 0 || ZombieHealth < 0)
        {
            ZombieDeath();
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (canSeePlayer)
            m_Rigidbody2D.MovePosition(Vector2.MoveTowards(transform.position, playerTrans.position, mov_Speed * Time.deltaTime));
    }

    float DistancePlayer()
    {
        return (playerTrans.transform.position.x - transform.position.x);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMov>().HealthManaager(damage);
            PlayerMov.Instance.ShakeCam(.5f, 0.1f);

        }
    }

    public void ManageZombieHealth(int damage)
    {
        ZombieHealth -= damage;
    }


}

