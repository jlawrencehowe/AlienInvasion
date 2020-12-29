using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

	public AudioSource musicPlayer;
	
	public AudioClip mainMenu, tutorial, level, gameOver;
	
	public PersistingGameData persistingGameData;
	
	public static MusicPlayer musicPlayerObject;
	
	// Use this for initialization
	void Start () {
		if(musicPlayerObject == null){
			musicPlayerObject = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (musicPlayerObject != this){
			
			Destroy(gameObject);
			
		}

		persistingGameData = GameObject.Find("PersistingGameData").GetComponent<PersistingGameData>();
		musicPlayer.clip = mainMenu;
		musicPlayer.loop = true;
		musicPlayer.Play();
		
		volumeUpdate();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void volumeUpdate(){
		musicPlayer.volume = (persistingGameData.musicVolumeControl/100);

		
	}
	
	public void optionsMenuUpdateMusicVolume(float musicVolume){
		
		musicPlayer.volume = (musicVolume/100);

	}
	
	public void updateMusicClip(int musicClip){
		
		if(musicClip == 1){
			
			musicPlayer.clip = mainMenu;
			musicPlayer.Play();
			
		}

		else if (musicClip == 2){
			
			musicPlayer.clip = level;
			musicPlayer.Play();
			
		}
		else if(musicClip == 3){
			musicPlayer.clip = gameOver;
			musicPlayer.Play();
		}
		
		
	}

	public void StopMusic(){
		this.GetComponent<AudioSource>().Pause();
	}

	public void StartMusic(){
		this.GetComponent<AudioSource>().UnPause();
	}
}
