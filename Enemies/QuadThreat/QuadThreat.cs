using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuadThreat : EnemyBase
{

	private float damage;
	private float maxDamage = 1;
	private Vector3 startPos;
	private Transform targetObject;
	private Vector3 targetLocation;
	private float speed = 0.4f, distanceTraveled = 0;
	private ScoreManager sM;
	private GameController GC;
	public Camera cam;
	public GameObject QTMissle;
	public List<GameObject> missles;
	public Animator explosion;
	private bool fallen = false;
	private AudioSource audioSource;
	public AudioClip explosionSFX;
	public GameObject shrapnel, shrapnel2, shrapnel3;
	// Use this for initialization
	void Start ()
	{
		attack = 10;
		cam = GameObject.Find ("Main Camera").GetComponent<Camera> ();
		startPos = this.transform.position;
		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;
		targetLocation = new Vector3 (this.transform.position.x, cam.transform.position.y, this.transform.position.z);
		GC = GameObject.Find ("GameController").GetComponent<GameController> ();
		sM = GameObject.Find ("ScoreManager").GetComponent<ScoreManager> ();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (!fallen) {
			distanceTraveled += Time.deltaTime * speed;
			this.transform.position = Vector3.Lerp (startPos, targetLocation, distanceTraveled);
			if (distanceTraveled >= 1 && !isDead) {

				CreateMissle ();
			}
		} else {
			transform.Translate (Vector3.down * Time.deltaTime * speed * 2.3f);
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

	public override void ShutSpriteDown ()
	{
		this.GetComponent<SpriteRenderer> ().enabled = false;
		for (int i = 0; i < 4; i++) {
			missles [i].GetComponent<SpriteRenderer> ().enabled = false;

			
		}
	}
	
	private void UpdateColor ()
	{
		
		SpriteRenderer sR = GetComponent<SpriteRenderer> ();
		sR.color = new Color (255, 1 - damage / maxDamage, 1 - damage / maxDamage);
		
	}

	public void CreateMissle ()
	{
		for (int i = 0; i < 4; i++) {
			missles [i].GetComponent<SpriteRenderer> ().enabled = false;
			GameObject tempMissle = Instantiate (QTMissle, missles [i].transform.position, this.transform.rotation) as GameObject;
			tempMissle.GetComponent<QTMissle> ().targetLocation = GenerateMissleLocation (i);
			GC.AddEnemy (tempMissle);

		}
		fallen = true;
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
