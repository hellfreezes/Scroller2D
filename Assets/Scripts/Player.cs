using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField]
    private float movementSpeed = 1.0f;
    [SerializeField]
    private Transform[] groundPoints;
    [SerializeField]
    private float groundRadius;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private bool airControl;

    private bool facingRight;
    private Animator heroAnimator;
    private static Player instance;

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }


    public Rigidbody2D HeroRigidbody { get; set; }
    public bool Attack { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }

    

    // Use this for initialization
    private void Start() {
        facingRight = true;
        HeroRigidbody = GetComponent<Rigidbody2D>();
        heroAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate() {
        float horizontal = Input.GetAxis("Horizontal");
        OnGround = IsGrounded();

        HandleMove(horizontal);
        Flip(horizontal);
        HandleLayers();
    }

    private void HandleMove(float horizontal)
    {
        if (HeroRigidbody.velocity.y < 0) //герой падает
        {
            heroAnimator.SetBool("land", true);
        }
        if (!Attack && (OnGround || airControl))
        {
            HeroRigidbody.velocity = new Vector2(horizontal * movementSpeed, HeroRigidbody.velocity.y);
        }

        if (HeroRigidbody.velocity.y == 0 && Jump) //Были на земле и выполнен запрос на прыжок
        {
            HeroRigidbody.AddForce(new Vector2(0, jumpForce));
        }

        heroAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            heroAnimator.SetTrigger("jump");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            heroAnimator.SetTrigger("attack");
        }
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private bool IsGrounded()
    {
        if (HeroRigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    //Смена слоев анимации
    private void HandleLayers()
    {
        if (!OnGround) //Если мы не на земле
        {
            heroAnimator.SetLayerWeight(1, 1); //1 = AirLayer
        } else
        {
            heroAnimator.SetLayerWeight(1, 0);
        }
    }
}
