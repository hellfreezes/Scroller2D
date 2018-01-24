using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {
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

    private bool isGrounded;
    private bool facingRight;
    private bool attack;
    private bool jump;

    private Rigidbody2D heroRigidbody;
    private Animator heroAnimator;

    // Use this for initialization
    private void Start() {
        facingRight = true;
        heroRigidbody = GetComponent<Rigidbody2D>();
        heroAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate() {
        float horizontal = Input.GetAxis("Horizontal");
        isGrounded = IsGrounded();

        HandleMove(horizontal);
        Flip(horizontal);
        HandleAttacks();
        HandleLayers();
        ResetValues();
    }

    private void HandleMove(float horizontal)
    {
        if (heroRigidbody.velocity.y < 0) //герой падает
        {
            heroAnimator.SetBool("land", true);
        }
        if (!this.heroAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && (isGrounded || airControl))
        {
            heroRigidbody.velocity = new Vector2(horizontal * movementSpeed, heroRigidbody.velocity.y);
        }

        if (isGrounded && jump) //Были на земле и выполнен запрос на прыжок
        {
            isGrounded = false;
            heroRigidbody.AddForce(new Vector2(0, jumpForce));
            heroAnimator.SetTrigger("jump");
        }

        heroAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleAttacks()
    {
        if (attack && isGrounded && !this.heroAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            heroAnimator.SetTrigger("attack");
            heroRigidbody.velocity = Vector2.zero;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            attack = true;
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

    private void ResetValues()
    {
        attack = false;
        jump = false;
    }

    private bool IsGrounded()
    {
        if (heroRigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        heroAnimator.ResetTrigger("jump");
                        heroAnimator.SetBool("land", false);
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
        if (!isGrounded) //Если мы не на земле
        {
            heroAnimator.SetLayerWeight(1, 1); //1 = AirLayer
        } else
        {
            heroAnimator.SetLayerWeight(1, 0);
        }
    }
}
