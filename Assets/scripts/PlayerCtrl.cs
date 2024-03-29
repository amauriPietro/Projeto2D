﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {

	public float horizontalSpeed = 5f;
	public float jumpSpeed = 600f;
	public float feetWidth = 0.5f;
	public float feetHeight = 0.1f;
	public bool isGrounded;
	public LayerMask whatIsGround;
	public bool canDoubleJump = false;
	public float delayForDoubleJump = 0.2f;
	public float delayForShoot = 0.2f;
	public float ShootTime = 0f;
	public GameObject RightShootPrefab;
	public GameObject LeftShootPrefab;

	Rigidbody2D rb;
	SpriteRenderer sr;
	Animator anim;
	bool isJumping = false;
	public Transform feet;
	public Transform RightShoot;
	public Transform LeftShoot;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
	}
	
	void OnDrawGizmos(){
		Gizmos.DrawWireCube(feet.position, new Vector3(feetWidth, feetHeight,  0f));
	}
	// Update is called once per frame
	void Update () {
		if(ShootTime < delayForShoot){
			ShootTime += Time.deltaTime;
		}
		if(transform.position.y < GM.instance.yMinLive){
			GM.instance.KillPlayer();
		}
		isGrounded = Physics2D.OverlapBox(new Vector2(feet.position.x, feet.position.y), new Vector2(feetWidth, feetHeight), 360f, whatIsGround);
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
		if(Input.GetButtonDown("Fire1")){
			shoot();
		}
		ShowFall();
	}

	void shoot(){
		if(delayForShoot > ShootTime){
			return;
		}
		ShootTime = 0f;
		anim.Play("robot-shoot");
		if(sr.flipX){
			AudioManager.instance.PlayLaserSound(LeftShoot.gameObject);
			Instantiate(LeftShootPrefab, LeftShoot.position, Quaternion.identity);
		}
		else{
			AudioManager.instance.PlayLaserSound(RightShoot.gameObject);
			Instantiate(RightShootPrefab, RightShoot.position, Quaternion.identity);
		}
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
		if(isGrounded){
			isJumping = true;
            AudioManager.instance.PlayJumpSound(gameObject);
			rb.AddForce(new Vector2(0f, jumpSpeed));
			anim.SetInteger("State", 1);

			Invoke("EnableDoubleJump", delayForDoubleJump);
		}
		if(canDoubleJump && !isGrounded){
			rb.velocity = Vector2.zero;
            AudioManager.instance.PlayJumpSound(gameObject);
            rb.AddForce(new Vector2(0f, jumpSpeed));
			anim.SetInteger("State", 1);
			canDoubleJump = false;
		}
	}
	void EnableDoubleJump(){
		canDoubleJump = true;
	}
	void OnCollisionEnter2D(Collision2D other){
		if(other.gameObject.layer == LayerMask.NameToLayer("Ground")){
			isJumping = false;
		}
        else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            anim.SetInteger("State", 5);
            GM.instance.HurtPlayer();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
		switch(other.gameObject.tag){
			case "coin":
			AudioManager.instance.PlayCoinPickupSound(other.gameObject);
            SFXManager.instance.ShowCoinParticle(other.gameObject);
            GM.instance.IncrementCoinCount();
            Destroy(other.gameObject);
			break;
			case "Finish":
			GM.instance.LevelComplete();
			break;
            case "Checkpoint":
                GameObject obj = GameObject.Find("SpawnPoint");
                obj.transform.position = other.transform.position;
                break;
		}
    }
}
