using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{

	public GameObject Asteroid, Cargo, Orb, QuadThreat, LaserDrone;
	public PlayerHealth playerHealth;
	private float waveDiffcultyTimer = 0;
	public Text startText;
	public Image backgroundStartText;
	private float Timer = 0;
	private bool firstPhase = true;
	private bool secondPhase = false;
	private bool gamePhase = false;
	private float enemyTimer, enemyTimer2 = 12.5f;
	public Transform enemyTargetLocation, leftCargoSpawnLocation, rightCargoSpawnLocation;
	public Camera cam;
	public List<GameObject> enemies;
	private float cargoSpawnChance = 1;
	public MusicPlayer musicPlayer;
	public PersistingGameData persistingGD;
	public List<float> highScores;
	public List<string> names;
	public List<Text> highScoreText, namesText;
	public ScoreManager scoreManager;
	public Canvas highScoreCanvas;
	public Button pauseButton;
	private bool awaitingPlayerInput = false;
	public CameraShake cameraShake;
	// Use this for initialization
	void Start ()
	{
		enemyTimer2 = 12.5f;
		highScoreCanvas.enabled = false;
		scoreManager = GameObject.Find ("ScoreManager").GetComponent<ScoreManager> ();
		persistingGD = GameObject.Find ("PersistingGameData").GetComponent<PersistingGameData> ();
		musicPlayer = GameObject.Find ("MusicPlayer").GetComponent<MusicPlayer> ();

		musicPlayer.StartMusic ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		waveDiffcultyTimer += Time.deltaTime;
		Timer += Time.deltaTime;
		Vector3 tempScale = backgroundStartText.rectTransform.localScale;
		if (tempScale.x < 1) {
			backgroundStartText.rectTransform.localScale = new Vector3 (tempScale.x + Time.deltaTime * 2, tempScale.y, tempScale.z);
		} else {
			backgroundStartText.rectTransform.localScale = new Vector3 (1, 1, 1);
		}
		if (Timer > 3 && firstPhase) {

			firstPhase = false;
			Timer = 0;
			secondPhase = true;
			startText.text = "Prepare your defenses";
		}
		if (Timer > 2.5f && secondPhase) {
			secondPhase = false;
			Timer = 0;
			gamePhase = true;
			startText.enabled = false;
			backgroundStartText.enabled = false;
		}
		if(!gamePhase){
			pauseButton.enabled = false;
		}
		else{
			pauseButton.enabled = true;
		}
		if (gamePhase && !playerHealth.gameOver) {
			SpawnEnemy ();
		}
		if (playerHealth.gameOver && ! awaitingPlayerInput) {
			Time.timeScale = 0;
			pauseButton.enabled = false;
			PointlessKillAllEnemies ();
			UpdateHighScore ();
			persistingGD.saveGameInfoData ();
			highScoreCanvas.enabled = true;
			awaitingPlayerInput = true;

		}

	}

	private void SpawnEnemy ()
	{

		enemyTimer -= Time.deltaTime;
		enemyTimer2 -= Time.deltaTime * 0.5f;
		if (enemyTimer <= 0) {
			enemyTimer = Spawn ();
			if (waveDiffcultyTimer > 120) {
				enemyTimer = Spawn ();
			}

		}

		if(enemyTimer2 <= 0){
			enemyTimer2 = Spawn ();
			if (waveDiffcultyTimer > 60) {
				enemyTimer2 = Spawn ();
			}

		}



	}

	private float Spawn ()
	{
		float randomEnemy = Random.Range (1, 101);
		float enemyTimerReturn;
		//cargodrone
		if (randomEnemy <= cargoSpawnChance) {
			enemyTimerReturn = SpawnCargoDrone ();
			cargoSpawnChance = 1;
		}
			//orb
			else if (randomEnemy > 1 && randomEnemy <= 10) {
			GameObject tempGameObject = Instantiate (Orb, RandomPosition (), this.transform.rotation) as GameObject;
			enemyTimerReturn = SetEnemySpawnTimer ();
			enemies.Add (tempGameObject);
				
		}
			
			//quad threat
			else if (randomEnemy > 10 && randomEnemy <= 20) {
			GameObject tempGameObject = Instantiate (QuadThreat, RandomPosition (), this.transform.rotation) as GameObject;
			enemyTimerReturn = SetEnemySpawnTimer ();
			enemies.Add (tempGameObject);
		} else if (randomEnemy > 20 && randomEnemy <= 30) {
			GameObject tempGameObject = Instantiate (LaserDrone, RandomPosition (), this.transform.rotation) as GameObject;
			enemyTimerReturn = SetEnemySpawnTimer ();
			enemies.Add (tempGameObject);
		}
			
			//asteroid
			else {
			GameObject tempGameObject = Instantiate (Asteroid, RandomPosition (), this.transform.rotation) as GameObject;
			tempGameObject.GetComponent<Asteroid> ().targetValue = new Vector3 (RandomPosition ().x, enemyTargetLocation.position.y, 0);
			enemyTimerReturn = SetEnemySpawnTimer ();
			enemies.Add (tempGameObject);
		}
		cargoSpawnChance += 0.2f;
			
		return enemyTimerReturn;
	}

	private Vector3 RandomPosition ()
	{
		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;
		Vector3 randomPos = new Vector3 (this.transform.position.x + Random.Range (cam.transform.position.x - width / 2 + 0.1f, cam.transform.position.y + width / 2 - 0.1f), this.transform.position.y, 0);

		return randomPos;

	}

	private float SpawnCargoDrone ()
	{
		float randomLocation = Random.Range (0, 2);




		if (randomLocation == 0) {
			GameObject tempGameObject = Instantiate (Cargo, leftCargoSpawnLocation.position, this.transform.rotation) as GameObject;
			tempGameObject.GetComponent<CargoDrone> ().targetValue = rightCargoSpawnLocation.position;
			enemies.Add (tempGameObject);
		} else {
			GameObject tempGameObject = Instantiate (Cargo, rightCargoSpawnLocation.position, this.transform.rotation) as GameObject;
			tempGameObject.GetComponent<CargoDrone> ().targetValue = leftCargoSpawnLocation.position;
			enemies.Add (tempGameObject);
		}
		return 0;
	}

	public void RemoveEnemy (GameObject deadEnemy)
	{
		cameraShake.ShakeCamera();
		enemies.Remove (deadEnemy);
			
		
	}

	public void AddEnemy (GameObject newEnemy)
	{
		enemies.Add (newEnemy);
	}

	private void PointlessKillAllEnemies ()
	{
		while (enemies.Count > 0) {
			for (int i = 0; i < enemies.Count; i++) {
				if (enemies [i] != null) {
					enemies [i].GetComponent<EnemyBase> ().PointlessDeath ();
				} else {
					enemies.RemoveAt (i);
				}
			}
		}
	}

	public void KillAllEnemies ()
	{

		while (enemies.Count > 0) {
			for (int i = 0; i < enemies.Count; i++) {
				if (enemies [i] != null) {
					enemies [i].GetComponent<EnemyBase> ().Death ();
				} else {
					enemies.RemoveAt (i);
				}
			}
		}

	}

	private float SetEnemySpawnTimer ()
	{
		float minRange = 3 - (waveDiffcultyTimer * 0.005f);
		float maxRange = 3 - (waveDiffcultyTimer * 0.004f);
		if (maxRange <= 0.5f) {
			maxRange = 0.4f;
		}
		if (minRange < 0.3f) {
			minRange = 0.1f;
		}
		float enemyTimer = Random.Range (minRange, maxRange);
		return enemyTimer;

	}

	private void UpdateHighScore ()
	{
		for (int i = 0; i < 10; i++) {
			if (scoreManager.currentScore > persistingGD.highScores [i]) {
				persistingGD.highScores.Insert (i, (float)scoreManager.currentScore);
				persistingGD.highScores.RemoveAt (10);
				persistingGD.names.Insert (i, persistingGD.currentUserName);
				persistingGD.names.RemoveAt (10);
				break;
			}
		}

		for (int i = 0; i < 10; i++) {
			highScoreText [i].text = "" + persistingGD.highScores [i];
			namesText [i].text = persistingGD.names [i] + ":";
		}

	}

	public void MainMenuButton ()
	{
		Time.timeScale = 1;
		musicPlayer.updateMusicClip (1);
		Application.LoadLevel (0);
	}

	public void TryAgainButton ()
	{
		Time.timeScale = 1;
		Application.LoadLevel (1);
	}


}
