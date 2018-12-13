using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelCompleteCtrl : MonoBehaviour {

	public Button btNext;
	public Sprite goldenStar;
	public Image Star1;
	public Image Star2;
	public Image Star3;
	public Text txtScore;
	public int score;
	public int ScoreForOneStar;
	public int ScoreForTwoStars;
	public int ScoreForThreeStars;
	public int ScoreForNextLevel;
	public float animStartDelay;
	public float animDelay;

	// Use this for initialization
	void Start () {
		txtScore.text = score.ToString();
		Invoke("IniciarMostrarEstrelas", animStartDelay);
	}
	void IniciarMostrarEstrelas(){
		StartCoroutine("MostrarEstrelas");
	}
	IEnumerator MostrarEstrelas(){
		if(score >= ScoreForOneStar){
			ExecutarAnimacao(Star1);
			yield return new WaitForSeconds(animDelay);
			if(score >= ScoreForTwoStars){
				ExecutarAnimacao(Star2);
				yield return new WaitForSeconds(animDelay);
				if(score >= ScoreForThreeStars){
					ExecutarAnimacao(Star3);
					yield return new WaitForSeconds(animDelay);
				}
			}
		}
		if(score >= ScoreForNextLevel){

		}
	}
	void ExecutarAnimacao(Image starImg){
		starImg.rectTransform.sizeDelta = new Vector2(150f, 150f);
		starImg.sprite = goldenStar;
		RectTransform t = starImg.rectTransform;
		t.DOSizeDelta(new Vector2(100f, 100f), 0.5f);
	}
	
	// Update is called once per frame

}
