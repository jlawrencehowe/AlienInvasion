using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public EnemyBase enemy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShutEnemySpriteOff(){

		enemy.ShutSpriteDown();
		enemy.GetComponent<Collider2D>().enabled = false;

	}

	public void DestroyGameObject(){

		Destroy(enemy.gameObject);
	}
}
