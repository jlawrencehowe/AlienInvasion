using UnityEngine;
using System.Collections;

public class CargoDrone : EnemyBase {

	private float damage;
	private float maxDamage = 1;
	private ScoreManager sM;
	public Vector3 targetValue;
	public Vector3 startPos;
	public float timer;
	public float speed;
	private GameController GC;
	public SpriteRenderer powerUpSprite;
	public Sprite doubleScoreSprite, doubleDamageSprite, infiniteEnergySprite, nukeSprite, healthSprite;

	//0 - double score, 1 - double damage, 2 - kill all enemies, 3 - health
	public int powerUp;

	private PlayerController playerController;
	private PlayerHealth playerHealth;


	// Use this for initialization
	void Start () {
		playerController = GameObject.Find("Barrel").GetComponent<PlayerController>();
		playerHealth = GameObject.Find("City").GetComponent<PlayerHealth>();
		GC = GameObject.Find("GameController").GetComponent<GameController>();
		sM = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
		startPos = this.transform.position;
		speed = Random.Range(0.1f, 0.3f);
		powerUp = -1;
		if(targetValue.x < startPos.x){
			this.transform.localScale = new Vector3(-1, 1, 1);
			powerUpSprite.transform.localScale = new Vector3(-1, 1, 1);
		}
		while(powerUp == -1){
			
			powerUp = Random.Range(0, 4);
			if(playerHealth.health == 100 && powerUp == 3){
				powerUp = -1;
			}
			Debug.Log(powerUp + " Cargo");
		}
		if(powerUp == 0){
			powerUpSprite.sprite = doubleScoreSprite;
		}
		else if(powerUp == 1){
			powerUpSprite.sprite = doubleDamageSprite;
		}

		else if(powerUp == 2){
			powerUpSprite.sprite = nukeSprite;
		}
		else if(powerUp == 3){
			powerUpSprite.sprite = healthSprite;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(timer >= 1){
			timer = 1;
			Destroy(gameObject);
		}
		else
			timer += Time.deltaTime * speed;
		
		transform.position = Vector3.Lerp(startPos, targetValue, timer);
	}

	public override void GetHit(bool doubleDamage, float laserDamage){
		
		if(doubleDamage){
			damage += laserDamage * Time.deltaTime;
		}
		else{
			damage += laserDamage * Time.deltaTime * 2;
		}
		
		UpdateColor();
		
		if(damage >= maxDamage){
			Death();
		}
		
	}
	
	public override void Death(){
		//0 - double score, 1 - double damage, 2 - kill all enemies, 3 - health
		if(powerUp == 0){
			playerController.doubleScore = true;
			playerController.scoreTimer = 10;
		}
		else if(powerUp == 1){
			playerController.doubleDamage = true;
			playerController.doubleDamageTimer = 10;
		}
		else if(powerUp == 2){
			powerUp = -1;
			GC.KillAllEnemies();
		}
		else if(powerUp == 3){
			playerHealth.TakeDamage(-20);
		}
		else if(powerUp == -1){

		}
		sM.UpdateScore(100);
		GC.RemoveEnemy(this.gameObject);
		Destroy(powerUpSprite.gameObject);
		this.GetComponent<BoxCollider2D>().enabled = false;
		
	}

	public override void PointlessDeath ()
	{
		GC.RemoveEnemy (this.gameObject);
		Destroy (gameObject);
	}
	
	public void UpdateColor(){
		

		powerUpSprite.color = new Color(255, 1 - damage/maxDamage, 1 - damage/maxDamage);
		
	}
}
