﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour {


	public static GM instance = null;

	public float yMinLive = -10f;
	public Transform spawnPoint;
	public GameObject playerPrefab;
	PlayerCtrl player;
	public float timeToResspawn = 2f;
    public float maxTime = 120f;
	bool timerOn = true;
	float timeLeft;
	public UI ui;
    public float timeToKill = 1.5f;
    GameData data = new GameData();

	void Awake(){
		if(instance == null){
			instance = this;
		}
	}
	// Use this for initialization
	void Start () {
		if(player == null){
			RespawnPlayer();
		}
		timeLeft = maxTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(player == null){
			GameObject obj = GameObject.FindGameObjectWithTag("Player");
			if(obj != null){
				player = obj.GetComponent<PlayerCtrl>();
			}
		}
		UpdateTimer();
		DisplayHudData();
	}
	public void RestartLevel(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	public void LoadMainMenu(){
		LoadScene("Main menu");
	}
	public void CloseApp(){
		Application.Quit();
	}
	public void LoadScene(string scene){
		SceneManager.LoadScene(scene);
	}
	void UpdateTimer(){
		if(timerOn){
			timeLeft = timeLeft - Time.deltaTime;
			if(timeLeft <= 0f){
				timeLeft = 0;
				ExpirePlayer();
			}
		}
	}
    void DisplayHudData()
    {
        ui.hud.txtCoinCount.text = "X " + data.coinCount;
		ui.hud.txtHeartCount.text = "X " + data.heartCount;
		ui.hud.txtTimer.text = "Timer: " + timeLeft.ToString("F1");
    }
    public void IncrementCoinCount(){
        data.coinCount++;
    }
	public void DecrementHeartCount(){
		data.heartCount--;
	}
	public void RespawnPlayer(){
		Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);

	}
	public void KillPlayer(){
		if(player != null){
			Destroy(player.gameObject);
			DecrementHeartCount();
			if(data.heartCount > 0 && timeLeft >= 2.1){
				Invoke("RespawnPlayer", timeToResspawn);
			}
			else{
				GameOver();
			}
		}
	}
    public void HurtPlayer()
    {
        if (player != null)
        {
            StartCoroutine(MuteMusic(true, 0f));
            AudioManager.instance.PlayFailSound(player.gameObject);
            DisableAndPushPlayer();
            Destroy(player.gameObject, timeToKill);
            DecrementHeartCount();
            if (data.heartCount > 0 && timeLeft >= 2.1)
            {
                StartCoroutine(MuteMusic(false, timeToKill + timeToResspawn));
                Invoke("RespawnPlayer", timeToKill + timeToResspawn);
            }
            else
            {
                StartCoroutine(MuteMusic(false, timeToKill + timeToResspawn));
                GameOver();
            }
        }
    }

    public int Score()
    {
        return data.coinCount;
    }
    public void ExpirePlayer(){
		if(player != null){
			Destroy(player.gameObject);
			GameOver();
		}
	}
	void GameOver(){
		timerOn = false;
		ui.gameOver.txtCoinCount.text = "X " + data.coinCount;
		ui.gameOver.txtTimer.text = "Timer: " + timeLeft.ToString("F1");
		ui.gameOver.gameOverPanel.SetActive(true);
	}
	public void LevelComplete(){
		Destroy(player.gameObject);
		timerOn = false;
	    StartCoroutine(MuteMusic(true, 0f));
        ui.levelComplete.levelCompleteStarPanel.SetActive(true);
		//ui.levelComplete.txtCoinCount.text = "X " + data.coinCount;
		//ui.levelComplete.txtTimer.text = "Timer: " + timeLeft.ToString("F1");
		//ui.levelComplete.levelCompletePanel.SetActive(true);
	}
    IEnumerator MuteMusic(bool value, float delay)
    {
        yield return new WaitForSeconds(delay);
        Camera.main.GetComponentInChildren<AudioSource>().mute = value;
    }
    void DisableAndPushPlayer()
    {
        player.transform.GetComponent<PlayerCtrl>().enabled = false;
        foreach(Collider2D c2d in player.transform.GetComponents<Collider2D>())
        {
            c2d.enabled = false;
        }
        foreach(Transform child in player.transform)
        {
            child.gameObject.SetActive(false);
        }
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.AddForce(new Vector2(-150f, 400f));
    }
}
