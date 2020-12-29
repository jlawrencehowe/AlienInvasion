using UnityEngine;
using System.Collections;

public class QTMissle : EnemyBase
{

	private float damage;
	private float maxDamage = 0.1f;
	public Vector3 targetLocation;
	private Transform enemyTargetLocation;
	private float turnSpeed = 360;
	private float forwardSpeed = 2;
	private bool cityAttack = false;
	private float rotateAmount = 0;
	private ScoreManager sM;
	private GameController GC;
	public Animator explosion;
	public Camera cam;
	public int choiceLock = 0;
	private bool lockOut = false;
	private bool oneTimeReset = false;
	private AudioSource audioSource;
	public AudioClip explosionSFX;
	public GameObject shrapnel, shrapnel2, shrapnel3;


	// Use this for initialization
	void Start ()
	{
		attack = 20;
		cam = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		GC = GameObject.Find ("GameController").GetComponent<GameController> ();
		sM = GameObject.Find ("ScoreManager").GetComponent<ScoreManager> ();
		enemyTargetLocation = GameObject.Find ("EnemyTargetLocation").GetComponent<Transform> ();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
	{

		/*
		rotateAmount += (Time.deltaTime * turnSpeed);
		transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.FromToRotation (transform.position, targetLocation - transform.position), rotateAmount);
		transform.Translate(Vector3.up * Time.deltaTime * forwardSpeed);
		if(!cityAttack && transform.position.y > targetLocation.y){
			targetLocation = new Vector3(targetLocation.x, enemyTargetLocation.position.y, targetLocation.z);
			cityAttack = true;
			rotateAmount = 0;
		}*/



		if(!isDead)
			CalculateAngle ();
		transform.Translate (Vector3.up * Time.deltaTime * forwardSpeed);

	}

	private void CalculateAngle ()
	{

		Vector3 offset = new Vector2 (targetLocation.x - transform.position.x, targetLocation.y - transform.position.y);
		float angle = Mathf.Atan2 (offset.y, offset.x) * Mathf.Rad2Deg - 90;
		Quaternion test = Quaternion.Euler (0, 0, angle);
		Debug.DrawRay(this.transform.position, offset);
		//Debug.Log(transform.rotation.eulerAngles.z + " transform");


		if (transform.position.y > targetLocation.y) {
			cityAttack = true;
			targetLocation = new Vector3 (enemyTargetLocation.position.x, enemyTargetLocation.position.y, targetLocation.z);
		} 


		/*
		if (transform.rotation.eulerAngles.z > test.eulerAngles.z) {
			float test2 = transform.rotation.eulerAngles.z - test.eulerAngles.z;
			float test3 = 360 - transform.rotation.eulerAngles.z + test.eulerAngles.z;
			//Debug.Log("test2 = " + test2);
			//Debug.Log("test3 = " + test3);
			if (test2 < test3) {
				transform.rotation = Quaternion.Euler (0, 0, transform.rotation.eulerAngles.z - (Time.deltaTime * turnSpeed));
			} else {
				transform.rotation = Quaternion.Euler (0, 0, transform.rotation.eulerAngles.z + (Time.deltaTime * turnSpeed));
			}
		} else {
			float test2 = test.eulerAngles.z - transform.rotation.eulerAngles.z;
			float test3 = 360 - test.eulerAngles.z + transform.rotation.eulerAngles.z;
			if (test2 < test3) {
				transform.rotation = Quaternion.Euler (0, 0, transform.rotation.eulerAngles.z + (Time.deltaTime * turnSpeed));
			} else {
				transform.rotation = Quaternion.Euler (0, 0, transform.rotation.eulerAngles.z - (Time.deltaTime * turnSpeed));
			}
		}*/
		/*
		if(transform.position.x > targetLocation.x && transform.rotation.eulerAngles.z < test.eulerAngles.z){
			transform.rotation = Quaternion.Euler (0, 0, transform.rotation.eulerAngles.z + (Time.deltaTime * turnSpeed));
		}
		else if(transform.position.x <= targetLocation.x && transform.rotation.eulerAngles.z < test.eulerAngles.z){
			transform.rotation = Quaternion.Euler (0, 0, transform.rotation.eulerAngles.z - (Time.deltaTime * turnSpeed));
		}*/
		/*
		if (transform.rotation.eulerAngles.z > (test.eulerAngles.z + 5) || transform.rotation.eulerAngles.z < (test.eulerAngles.z - 5)) {
			if (targetLocation.x > 0) {
				transform.rotation = Quaternion.Euler (0, 0, transform.rotation.eulerAngles.z + (Time.deltaTime * turnSpeed));
			} else if (targetLocation.x <= 0) {
				transform.rotation = Quaternion.Euler (0, 0, transform.rotation.eulerAngles.z - (Time.deltaTime * turnSpeed));
			}
		}
*/

		float targetX;
		if(cityAttack){
			targetX = 0;
			if(!oneTimeReset)
				lockOut = false;
			oneTimeReset = true;
		}
		else{
			targetX = targetLocation.x;
			choiceLock = 0;
		}

		if (transform.rotation.eulerAngles.z > (test.eulerAngles.z + 5) || transform.rotation.eulerAngles.z < (test.eulerAngles.z - 5) && !lockOut) {

		
			if (transform.position.x > targetX || choiceLock == 1) {
				choiceLock = 1;
				transform.rotation = Quaternion.Euler (0, 0, transform.rotation.eulerAngles.z + (Time.deltaTime * turnSpeed));
			} else if (transform.position.x <= targetX || choiceLock == 2) {
				choiceLock = 2;
				transform.rotation = Quaternion.Euler (0, 0, transform.rotation.eulerAngles.z - (Time.deltaTime * turnSpeed));
			}
		}
		else{
			lockOut = true;
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
			this.GetComponent<Collider2D> ().enabled = false;
			isDead = true;
			sM.UpdateScore (25);
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
		this.GetComponent<Collider2D> ().enabled = false;
		isDead = true;
		GC.RemoveEnemy (this.gameObject);
		explosion.SetTrigger ("Explode");
	}
	
	private void UpdateColor ()
	{
		
		SpriteRenderer sR = GetComponent<SpriteRenderer> ();
		sR.color = new Color (255, 1 - damage / maxDamage, 1 - damage / maxDamage);
		
	}
	
	private Vector3 GenerateMissleLocation (int missleNumber)
	{
		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;
		Vector3 targetLocation = new Vector3 (Random.Range ((cam.transform.position.x - width / 2) + (width / 4 * missleNumber), 
		                                                    (cam.transform.position.x - width / 2) + (width / 4 * missleNumber + 1)),
		                                      cam.transform.position.y + height / 4, this.transform.position.z);
		
		return targetLocation;
		
		
	}
}
