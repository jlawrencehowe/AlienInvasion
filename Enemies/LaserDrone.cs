using UnityEngine;
using System.Collections;

public class LaserDrone : EnemyBase
{



	private float damage;
	private float maxDamage = 1;
	public Vector3 targetValue;
	public Vector3 startPos;
	public float timer;
	public float speed;
	private ScoreManager sM;
	private GameController GC;
	private float laserCharge = 0;
	public GameObject laser;
	private PlayerHealth playerHealth;
	private Camera cam;
	private float damageToPlayer = 0;
	private Animator laserAnimator;
	public Animator explosion;
	private AudioSource audioSource;
	public AudioClip explosionSFX;
	public GameObject shrapnel, shrapnel2, shrapnel3;
	
	// Use this for initialization
	void Start ()
	{
		attack = 1;
		cam = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		targetValue = new Vector3 (this.transform.position.x, cam.transform.position.y, this.transform.position.z);
		GC = GameObject.Find ("GameController").GetComponent<GameController> ();
		sM = GameObject.Find ("ScoreManager").GetComponent<ScoreManager> ();
		startPos = this.transform.position;
		speed = Random.Range (0.2f, 0.4f);
		playerHealth = GameObject.Find ("City").GetComponent<PlayerHealth> ();
		laserAnimator = this.GetComponent<Animator> ();
		
		audioSource = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (timer >= 1) {
			timer = 1;
		} else
			timer += Time.deltaTime * speed;
		
		transform.position = Vector3.Lerp (startPos, targetValue, timer);

		if (timer == 1) {
			laserCharge += Time.deltaTime;
			laserAnimator.SetBool ("isCharging", true);

		}
		if (laserCharge >= 1 && !isDead) {
			laserAnimator.SetBool ("isCharging", false);
			laser.SetActive (true);
			damageToPlayer += Time.deltaTime * 2;
			while (damageToPlayer > 1) {
				playerHealth.TakeDamage (1);
				damageToPlayer--;
			}
		}
		
	}
	
	public override void GetHit (bool doubleDamage, float laserDamage)
	{
		
		if (doubleDamage) {
			damage += laserDamage * Time.deltaTime;
		} else {
			damage += laserDamage * Time.deltaTime * 2;
		}
		
		UpdateColor ();
		
		if (damage >= maxDamage) {
			Death ();
		}
		
	}
	
	public override void Death ()
	{
		if (!isDead) {
			int shrapnelAmount = Random.Range(1, 3);
			for(int i = 0; i < shrapnelAmount; i++){
				int shrapnelChoice = Random.Range(1, 3);
				if(shrapnelChoice == 1){
					Instantiate(shrapnel, this.transform.position, this.transform.rotation);
				}
				else if(shrapnelChoice == 2){
					Instantiate(shrapnel2, this.transform.position, this.transform.rotation);
				}
				else if(shrapnelChoice == 3){
					Instantiate(shrapnel3, this.transform.position, this.transform.rotation);
				}
			}
			audioSource.PlayOneShot(explosionSFX);
			this.GetComponent<Collider2D>().enabled = false;
			isDead = true;
			sM.UpdateScore (100);
			GC.RemoveEnemy (this.gameObject);
			explosion.SetTrigger ("Explode");
		}
	}
	
	public override void PointlessDeath ()
	{
		int shrapnelAmount = Random.Range(1, 3);
		for(int i = 0; i < shrapnelAmount; i++){
			int shrapnelChoice = Random.Range(1, 3);
			if(shrapnelChoice == 1){
				Instantiate(shrapnel, this.transform.position, this.transform.rotation);
			}
			else if(shrapnelChoice == 2){
				Instantiate(shrapnel2, this.transform.position, this.transform.rotation);
			}
			else if(shrapnelChoice == 3){
				Instantiate(shrapnel3, this.transform.position, this.transform.rotation);
			}
		}
		audioSource.PlayOneShot(explosionSFX);
		this.GetComponent<Collider2D>().enabled = false;
		isDead = true;
		GC.RemoveEnemy (this.gameObject);
		explosion.SetTrigger ("Explode");
	}
	
	private void UpdateColor ()
	{
		
		SpriteRenderer sR = GetComponent<SpriteRenderer> ();
		sR.color = new Color (255, 1 - damage / maxDamage, 1 - damage / maxDamage);
		
	}
}
