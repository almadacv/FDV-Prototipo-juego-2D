using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;
public class PlayerMov : MonoBehaviour
{
    public static PlayerMov Instance { get; private set; }
    private Rigidbody2D m_Rigidbody2D;
    private Animator animator;
    private Vector3 m_Velocity = Vector3.zero;
    [SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private float m_JumpForce = 400.0f;
    bool isGrounded;
    float horizontalMove = 0f;
    public SpriteRenderer CharacterSprite;
    public float mov_Speed = 40f;
    public int PlayerHealth = 100;
    public int Score = 0;
    [SerializeField] private Transform cano;
    bool needRotate = true;
    public TMP_Text Txt_Health;
    public TMP_Text Txt_Score;
    [SerializeField] private CinemachineVirtualCamera Camara;
    float shakerTimer;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        m_Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        /*         Debug.Log($"PlayerHealth ---> {PlayerHealth}");
                Debug.Log($"Score ---> {Score}"); 
        Debug.Log($"CharacterSprite.flipX--->{CharacterSprite.flipX}");
        Debug.Log($"needRotate--->{needRotate}");
                */

        horizontalMove = Input.GetAxisRaw("Horizontal") * mov_Speed;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            isGrounded = false;
            m_Rigidbody2D.AddForce(m_JumpForce * Vector2.up);
        }

        if (animator)
        {
            animator.SetBool("IsJumping", !isGrounded);
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        }

        if (CharacterSprite)
        {
            if (horizontalMove > 0)
            {
                CharacterSprite.flipX = false;
            }
            else if (horizontalMove < 0)
            {
                CharacterSprite.flipX = true;
            }

            if (PlayerHealth <= 30)
            {
                Debug.Log("aaaaaaaaaaaaaaaaa");
                CharacterSprite.color = Color.red;
            }
            else if (PlayerHealth > 50)
            {
                CharacterSprite.color = Color.white;
            }
            else
            {
                CharacterSprite.color = Color.yellow;
            }
        }

        needRotate = CharacterSprite.flipX;

        if (needRotate)
        {
            cano.Rotate(0f, 180f, 0f);
        }

        if (PlayerHealth <= 0)
        {
            animator.SetBool("IsDead",true);
            Destroy(gameObject);
            Application.Quit();
            ///observer ali
        }

        Txt_Health.text = $"Health: {PlayerHealth}";
        Txt_Score.text = $"Points: {Score}";

        shakerTimer -= Time.deltaTime;
        if (shakerTimer <= 0f)
        {
            //timer
            CinemachineBasicMultiChannelPerlin camaraNoise = Camara.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            camaraNoise.m_AmplitudeGain = 0;

        }
    }

    void FixedUpdate()
    {
        if (isGrounded)
            MovePlayer(horizontalMove * Time.fixedDeltaTime);
    }

    public void MovePlayer(float move)
    {
        Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Floor"))
            isGrounded = true;
        if (other.gameObject.CompareTag("CannonBall"))
            HealthManaager(-30);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //        Debug.Log(other.name);
        if (other.gameObject.CompareTag("Coin"))
        {
            Score += 1;
            Destroy(other.gameObject);
        }
    }

    public void HealthManaager(int value)
    {
        PlayerHealth += value;
        if (PlayerHealth < 100)
        {
            PlayerHealth = 200;
        }
    }

    public void ShakeCam(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin camaraNoise = Camara.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (Camara != null)
        {
            camaraNoise.m_AmplitudeGain = intensity;
            camaraNoise.m_FrequencyGain = intensity;
            shakerTimer = time;
        }
    }


}

