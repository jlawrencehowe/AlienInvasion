using UnityEngine;
using System.Collections;

public class OrbShot : EnemyBase
{


	public float damage;
	private float maxDamage = 1;
	private ScoreManager sM;
	private GameController GC;
	public Animator explosion;
	private AudioSource audioSource;
	public AudioClip explosionSFX;
	public GameObject shrapnel, shrapnel2, shrapnel3;

	// Use this for initialization
	void Start ()
	{
		attack = 15;
		GC = GameObject.Find ("GameController").GetComponent<GameController> ();
		sM = GameObject.Find ("ScoreManager").GetComponent<ScoreManager> ();
		GC.AddEnemy (this.gameObject);
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Translate (Vector3.down * Time.deltaTime * 2);
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
