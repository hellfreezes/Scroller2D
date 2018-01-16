using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour {
    [SerializeField]
    private float movementSpeed = 1.0f;


    private Rigidbody2D heroRigidbody;

	// Use this for initialization
	void Start () {
        heroRigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float horizontal = Input.GetAxis("Horizontal");

        MoveHandle(horizontal);
	}

    private void MoveHandle(float horizontal)
    {
        heroRigidbody.velocity = new Vector2(horizontal * movementSpeed, heroRigidbody.velocity.y);
    }
}
