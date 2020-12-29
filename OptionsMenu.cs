using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsMenu : MonoBehaviour {

	public Button pauseButton, resumeButton, exitButton;
	public GameObject pauseMenu, confirmationMenu;
	public Slider musicSlider, sfxSlider;
	public Text musicText, sfxText;
	public Text userName;
	public PersistingGameData persistingGD;
	public InputName inputName;
	public MusicPlayer musicPlayer;

	// Use this for initialization
	void Awake () {
		musicPlayer = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();
		persistingGD = GameObject.Find ("PersistingGameData").GetComponent<PersistingGameData>();
		this.GetComponent<Canvas>().enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PauseButton(){
		pauseMenu.SetActive(true);
		pauseButton.enabled = false;
		Time.timeScale = 0;
		musicSlider.value = persistingGD.musicVolumeControl;
		sfxSlider.value = persistingGD.soundVolumeControl;
		musicVolumeChange();
		SFXVolumeChange();
		userName.text = "Current User: " + persistingGD.currentUserName;
		this.GetComponent<Canvas>().enabled = true;


	}

	public void ResumeButton ()
	{
		pauseMenu.SetActive(false);
		pauseButton.enabled = true;
		Time.timeScale = 1;
		this.GetComponent<Canvas>().enabled = false;
		persistingGD.saveGameInfoData();
		
	}
	
	public void musicVolumeChange ()
	{
		
		persistingGD.musicVolumeControl = musicSlider.value;
		musicPlayer.volumeUpdate();
		musicText.text = "Music Volume: " + musicSlider.value;
		
	}
	
	public void SFXVolumeChange ()
	{
		persistingGD.soundVolumeControl = sfxSlider.value;
		sfxText.text = "SFX Volume: " + sfxSlider.value;
		
	}

	public void QuitButton(){
		confirmationMenu.SetActive(true);
		pauseMenu.SetActive(false);
		musicSlider.enabled = false;
		sfxSlider.enabled = false;
		resumeButton.enabled = false;
		exitButton.enabled = false;
	}

	public void YesButton(){
		//return to main menu
		Application.LoadLevel(0);
		musicPlayer.updateMusicClip(1);
	}

	public void NoButton(){
		confirmationMenu.SetActive(false);
		pauseMenu.SetActive(true);
		musicSlider.enabled = true;
		sfxSlider.enabled = true;
		resumeButton.enabled = true;
		exitButton.enabled = true;
	}

	public void ChangeName(){
		inputName.keyIsActive = true;
		userName.text = "Current User: " + persistingGD.currentUserName;
	}


}
