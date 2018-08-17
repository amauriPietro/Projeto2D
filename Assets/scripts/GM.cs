using System.Collections;
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
}
