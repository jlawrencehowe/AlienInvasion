using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PersistingGameData : MonoBehaviour {

	public float soundVolumeControl = 50f, musicVolumeControl = 50f;
	public List <float> highScores;
	public List <string> names;
	public MusicPlayer musicPlayer;
	public string currentUserName = " ";
	public InputName inputName;
	public static PersistingGameData gameData;
	
	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.Portrait;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		Debug.Log(Application.persistentDataPath);
		currentUserName = " ";
		for(int i = 0; i < 10; i++){

			highScores.Add(5000);
			names.Add("Morb");

		}
		if(gameData == null){
			gameData = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (gameData != this){
			
			Destroy(gameObject);
			
		}

		musicPlayer = GameObject.Find("MusicPlayer").GetComponent<MusicPlayer>();
		
		loadGameInfoData();

		if(currentUserName == " " && inputName != null){
			inputName.firstTime = true;
			inputName.keyIsActive = true;
		}

		
	}


	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void saveGameInfoData(){
		
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/gameInfo.gd");
		
		GameSystemData gameSystemData = new GameSystemData();
		gameSystemData.soundVolumeControl = soundVolumeControl;
		gameSystemData.musicVolumeControl = musicVolumeControl;
		gameSystemData.highScores = highScores;
		gameSystemData.names = names;
		gameSystemData.currentUserName = currentUserName;
		bf.Serialize(file, gameSystemData);
		file.Close();
		
	}
	
	public void loadGameInfoData(){
		
		if(File.Exists(Application.persistentDataPath + "/gameInfo.gd")){
			
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.gd", FileMode.Open);
			GameSystemData gameSystemData = (GameSystemData)bf.Deserialize(file);
			updateGameSystemData(gameSystemData);
			
			file.Close();
		}
	}


	private void updateGameSystemData(GameSystemData gameSystemData){

		highScores = gameSystemData.highScores;
		names = gameSystemData.names;
		soundVolumeControl = gameSystemData.soundVolumeControl;
		musicVolumeControl = gameSystemData.musicVolumeControl;
		currentUserName = gameSystemData.currentUserName;
		
	}
	
	
}

[Serializable]
class GameSystemData
{
	public float soundVolumeControl, musicVolumeControl;
	public List <float> highScores;
	public List <string> names;
	public string currentUserName;
	
	
}