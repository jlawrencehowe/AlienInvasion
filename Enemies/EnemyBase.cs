using UnityEngine;
using System.Collections;

public class EnemyBase : MonoBehaviour
{

	public float attack;
	public bool isDead = false;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public virtual void GetHit (bool doubleDamage, float laserDamage)
	{
	}

	public virtual void Death ()
	{
	}

	public virtual void PointlessDeath ()
	{
	}

	public virtual void ShutSpriteDown ()
	{
		this.GetComponent<SpriteRenderer> ().enabled = false;
	}
}
