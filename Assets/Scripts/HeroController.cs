using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {
    [SerializeField]
    private float movementSpeed = 1.0f;

    private bool facingRight;

    private Rigidbody2D heroRigidbody;
    private Animator heroAnimator;

	// Use this for initialization
	void Start () {
        facingRight = true;
        heroRigidbody = GetComponent<Rigidbody2D>();
        heroAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float horizontal = Input.GetAxis("Horizontal");

        MoveHandle(horizontal);
        Flip(horizontal);
	}

    private void MoveHandle(float horizontal)
    {
        heroRigidbody.velocity = new Vector2(horizontal * movementSpeed, heroRigidbody.velocity.y);
        heroAnimator.SetFloat("speed", Mathf.Abs(horizontal));
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
}
