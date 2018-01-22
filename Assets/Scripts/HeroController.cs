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
	private void Start () {
        facingRight = true;
        heroRigidbody = GetComponent<Rigidbody2D>();
        heroAnimator = GetComponent<Animator>();
	}
    
    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate () {
        float horizontal = Input.GetAxis("Horizontal");
        isGrounded = IsGrounded();

        HandleMove(horizontal);
        Flip(horizontal);
        HandleAttacks();
        ResetValues();
	}

    private void HandleMove(float horizontal)
    {
        if (!this.heroAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack") && (isGrounded || airControl))
        {
            heroRigidbody.velocity = new Vector2(horizontal * movementSpeed, heroRigidbody.velocity.y);
        }

        if (isGrounded && jump)
        {
            isGrounded = false;
            heroRigidbody.AddForce(new Vector2(0, jumpForce));
        }
        
        heroAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleAttacks()
    {
        if (attack && !this.heroAnimator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
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
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
