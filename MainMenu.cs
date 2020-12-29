using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MainMenu : MonoBehaviour {

	public Texture2D emptyProgressBar; // Set this in inspector.
	public Texture2D fullProgressBar; // Set this in inspector.
	private AsyncOperation async = null; // When assigned, load is in progress.
	public Canvas loadingCanvas, HTPCanvas, mainMenuCanvas, creditsCanvas, quitCanvas;
	public Button playButton, optionsButton, HTPButton, quitButton, creditsButton;
	public List<GameObject> instructionList;
	private int currentInstructionPage = 0;
	public Text nextButtonText;
	public GameObject previousButton;

	
	public MusicPlayer musicPlayer;

	// Use this for initialization
	void Start () {
		musicPlayer = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayButton(){
		loadingCanvas.enabled = true;
		this.GetComponent<Canvas>().enabled = false;
		async = Application.LoadLevelAsync(1);
		load ();
		musicPlayer.updateMusicClip(2);
		musicPlayer.StopMusic();
	}

	IEnumerator load(){
		
		yield return async;
		
	}
	
	void OnGUI() {
		if (async != null) {
			
			GUI.DrawTexture(new Rect(350, 200, 100 * async.progress, 50), emptyProgressBar);
			//GUI.DrawTexture(new Rect(50, 50, 100 * async.progress, 50), fullProgressBar);
		}
	}

	public void HowToPlayButton(){
		//currentInstructionPage = 0;
		//previousButton.SetActive(false);
		//for(int i = 0; i < instructionList.Count; i++){
		//	instructionList[i].SetActive(false);
		//}
		//instructionList[0].SetActive(true);
		//nextButtonText.text = "Next";
		HTPCanvas.enabled = true;
		DisableMenuButtons();
	}

	public void HTPBReturn(){
		HTPCanvas.enabled = false;
		EnableMenuButtons();
	}

	public void CreditsButton(){
		DisableMenuButtons();
		creditsCanvas.enabled = true;
	}

	public void CreditsReturn(){
		EnableMenuButtons();
		creditsCanvas.enabled = false;
	}

	public void QuitButton(){
		DisableMenuButtons();
		quitCanvas.enabled = true;
	}

	public void YesButton(){
		Application.Quit();
	}

	public void NoButton(){
		EnableMenuButtons();
		quitCanvas.enabled = false;
	}

	public void OptionsButton(){
		DisableMenuButtons();
	}

	public void OptionReturnButton(){
		EnableMenuButtons();
	}

	private void DisableMenuButtons(){
		playButton.enabled = false;
		optionsButton.enabled = false;
		HTPButton.enabled = false;
		creditsButton.enabled = false;
		quitButton.enabled = false;
	}

	private void EnableMenuButtons(){
		playButton.enabled = true;
		optionsButton.enabled = true;
		HTPButton.enabled = true;
		creditsButton.enabled = true;
		quitButton.enabled = true;
	}

	public void NextButton(){

		if(currentInstructionPage == (instructionList.Count - 1)){
			HTPBReturn();
			return;
		}
		currentInstructionPage++;
		instructionList[currentInstructionPage].SetActive(true);
		instructionList[currentInstructionPage - 1].SetActive(false);
		if(currentInstructionPage == (instructionList.Count - 1)){
			nextButtonText.text = "Finish";
		}
		previousButton.SetActive(true);
	}

	public void PreviousButton(){

		currentInstructionPage--;
		if(currentInstructionPage == 0){
			previousButton.SetActive(false);
		}
		instructionList[currentInstructionPage].SetActive(true);
		instructionList[currentInstructionPage + 1].SetActive(false);
		if(currentInstructionPage != (instructionList.Count - 1)){
			nextButtonText.text = "Next";
		}

	}


}
