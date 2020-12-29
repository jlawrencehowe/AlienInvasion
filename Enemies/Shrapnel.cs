using UnityEngine;
using System.Collections;

public class Shrapnel : MonoBehaviour {

	private float rotationSpeed;
	public float gravity;
	public float sideForce;
	public GameObject fire;
	int rotationDirection;
	private GameObject camera;
	public float decaySpeed;
	private ParticleSystem particleSystem;

	// Use this for initialization
	void Start () {
		particleSystem = fire.GetComponent<ParticleSystem>();
		camera = GameObject.Find("Main Camera");
		rotationSpeed = Random.Range(350, 400);
		rotationDirection = Random.Range(-1, 1);
		if(rotationDirection == 0){
			rotationDirection = -1;
		}
		sideForce = Random.Range(-2f, 2f);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tempScale = this.transform.localScale;
		tempScale = new Vector3(tempScale.x - Time.deltaTime * decaySpeed, 
		                         tempScale.y - Time.deltaTime * decaySpeed, 
		                         tempScale.z - Time.deltaTime * decaySpeed);
		this.transform.localScale = tempScale;
		
		particleSystem.startSize = 0.2f * tempScale.x/1.0f;
		if(this.transform.localScale.x <= 0){
			Destroy(fire);
		}
		if(this.transform.position.y < camera.transform.position.y - Screen.height){
			Destroy(fire);
		}
		fire.transform.position = new Vector3 (this.transform.position.x + (Time.deltaTime * sideForce), 
		                                       this.transform.position.y - (Time.deltaTime * gravity), 1);
		//Vector3 newRotation = this.transform.rotation.eulerAngles;
		//newRotation.z += Time.deltaTime*rotationSpeed;
		//this.transform.rotation.eulerAngles = newRotation;
		transform.Rotate(new Vector3(0, 0, 1), Time.deltaTime * rotationSpeed);
		if(sideForce > 0.2f)
			sideForce -= Time.deltaTime;
		else if(sideForce < -0.2f){
			sideForce += Time.deltaTime;
		}
	}
}
