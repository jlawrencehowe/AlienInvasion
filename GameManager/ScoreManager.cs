using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	public int currentScore = 0;
	public PlayerController playerController;
	public Text scoreText;

	// Use this for initialization
	void Start () {
		scoreText.text = "Score: " + string.Format("{0:000000}", 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateScore(int score){
		if(playerController.doubleScore){
			score *= 2;
		}
		currentScore += score;
		scoreText.text = "Score: " + string.Format("{0:000000}", currentScore);
	}


}
