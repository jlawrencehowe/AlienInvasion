using UnityEngine;
using System.Collections;

public class OrbEnemy : EnemyBase
{

	public float damage;
	private float maxDamage = 1;
	bool floatDown = true;
	private ScoreManager sM;
	public Camera cam;
	private GameController GC;
	private Vector3 previousLoc, nextLoc;
	private float nextLocDist = 0;
	private bool leftSide = false;
	public float speed;
	private float attackChance = 0;
	private bool isAttacking = false;
	private float attackTimer = 2;
	private Animator anim;
	public Animator explosion;
	public GameObject orbShot;
	private AudioSource audioSource;
	public AudioClip explosionSFX;
	public GameObject shrapnel, shrapnel2, shrapnel3;


	// Use this for initialization
	void Start ()
	{
		attack = 10;
		cam = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		GC = GameObject.Find ("GameController").GetComponent<GameController> ();
		sM = GameObject.Find ("ScoreManager").GetComponent<ScoreManager> ();
		previousLoc = this.transform.position;
		nextLoc = this.transform.position;
		nextLoc = NextLocation ();
		anim = this.GetComponent<Animator> ();
		
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (isAttacking) {
			anim.SetBool ("isCharging", true);
			attackTimer -= Time.deltaTime;
			if (attackTimer <= 0 && !isDead) {
				Instantiate (orbShot, new Vector3 (this.transform.position.x, 
				                                 this.transform.position.y - 0.32f, this.transform.position.z), this.transform.rotation);
				isAttacking = false;
				anim.SetBool ("isCharging", false);
			}
		} else if (nextLocDist < 1) {
			anim.SetBool ("isCharging", false);
			nextLocDist += Time.deltaTime * speed;
			this.transform.position = Vector3.Lerp (previousLoc, nextLoc, nextLocDist);
		} else {
			if (attackChance >= Random.Range (1, 7)) {
				attackChance = 0;
				isAttacking = true;
				attackTimer = 1;
			} else {
				nextLoc = NextLocation ();
				nextLocDist = 0;
				attackChance++;
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

	private void UpdateColor ()
	{
		
		SpriteRenderer sR = GetComponent<SpriteRenderer> ();
		sR.color = new Color (255, 1 - damage / maxDamage, 1 - damage / maxDamage);
		
	}

	private Vector3 NextLocation ()
	{
		previousLoc = nextLoc;
		Vector3 loc;
		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;
		Vector3 center = cam.transform.position;
		float xLoc;
		if (leftSide) {
			xLoc = Random.Range (center.x - (width / 2), center.x);
		} else {
			xLoc = Random.Range (center.x, center.x + (width / 2));
		}
		leftSide = !leftSide;
		float yLoc = Random.Range (center.y + (height / 2 * 0.2f), center.y + (height / 2 * 0.4f));
		loc = new Vector3 (xLoc, yLoc, this.transform.position.z);
		speed = 1.25f - Vector3.Distance (previousLoc, loc) / width;
		return loc;

	}
}
