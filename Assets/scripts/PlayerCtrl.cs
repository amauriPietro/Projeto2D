using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

	public float horizontalSpeed = 5f;
	public float jumpSpeed = 600f;

	Rigidbody2D rb;
	SpriteRenderer sr;
	Animator anim;
	bool isJumping = false;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
		float horizontalInput = Input.GetAxisRaw("Horizontal");
		float horizontalPlayerSpeed = horizontalSpeed * horizontalInput;
		if(horizontalPlayerSpeed != 0){
			MoveHorizontal(horizontalPlayerSpeed);
		}
		else{
			StopMoving();
		}
		if(Input.GetButtonDown("Jump")){
			jump();
		}
		ShowFall();
	}

	void MoveHorizontal(float speed){
		rb.velocity = new Vector2(speed, rb.velocity.y);
		if(speed < 0f){
			sr.flipX = true;
		}
		if(speed > 0f){
			sr.flipX = false;
		}
		if(!isJumping){
			anim.SetInteger("State", 2);
		}
	}
	void StopMoving(){
		rb.velocity = new Vector2(0f, rb.velocity.y);
		if(!isJumping){
			anim.SetInteger("State", 0);
		}
	}
	void ShowFall(){
		if(rb.velocity.y < 0f){
			anim.SetInteger("State", 3);
		}
	}
	void jump(){
		isJumping = true;
		rb.AddForce(new Vector2(0f, jumpSpeed));
		anim.SetInteger("State", 1);
	}
	void OnCollisionEnter2D(Collision2D other){
		if(other.gameObject.layer == LayerMask.NameToLayer("Ground")){
			isJumping = false;
		}
	}
}
