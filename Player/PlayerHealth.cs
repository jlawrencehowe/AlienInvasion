using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {

	public float health, maxHealth = 100;
	public GameObject healthBar;
	public Text healthNumber;
	public bool gameOver = false;

	// Use this for initialization
	void Start () {
		health = maxHealth;
		UpdateHealthBar();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TakeDamage(float damage){

		health -= damage;
			//game over
		if(health > 100){
			health = 100;
		}
		if(health <= 0){
			health = 0;
			gameOver = true;
		}
		UpdateHealthBar();
	}

	public void OnTriggerEnter2D(Collider2D coll){
		Debug.Log("Test");
		if(coll.gameObject.tag == "Enemy"){
			TakeDamage(coll.GetComponent<EnemyBase>().attack);
			coll.GetComponent<EnemyBase>().PointlessDeath();
		}

	}

	private void UpdateHealthBar(){
		float percentHealth = health/maxHealth;
		healthNumber.text = health + "/" + maxHealth;
		// Set the health bar's colour to proportion of the way between green and red based on the player's health.
	
		//healthBarSprite.color = Color.Lerp(Color.green, Color.red, 1f - percentHealth);
		// Set the scale of the health bar to be proportional to the player's health.
		healthBar.transform.localScale = new Vector3(1f * (percentHealth), 1, 1);
	}
}
