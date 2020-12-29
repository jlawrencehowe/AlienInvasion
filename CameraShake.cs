using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {

	private Vector3 startPos, newLoc, prevLoc;
	public float shakeDecay = 0.05f;
	public float shakeIntensity;
	public float shakeSpeed;
	private float distance;
	private bool shaking = false;

	// Use this for initialization
	void Start () {
		distance = 0;
		startPos = this.transform.position;
		prevLoc = startPos;
		newLoc = startPos;

	}
	
	// Update is called once per frame
	void Update () {
		if(shaking){
			shakeIntensity -= (Time.deltaTime * shakeDecay);
			distance += Time.deltaTime * shakeSpeed;
			this.transform.position = Vector2.Lerp(prevLoc, newLoc, distance);
			if(distance >= 1){
				distance = 0;
				NewLocation();
			}
			if(shakeIntensity <= 0){
				shakeIntensity = 0;
				shaking = false;
				distance = 0;
				prevLoc = this.transform.position;
			}
		}
		else{
			this.transform.position = Vector3.Lerp(prevLoc, startPos, distance);
			distance += Time.deltaTime * shakeSpeed;
		}
		
		this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10);

	}

	public void ShakeCamera(){
		shakeIntensity += 0.3f;
		if(shakeIntensity > 0.4f){
			shakeIntensity = 0.4f;
		}
		shaking = true;
	}

	public void MiniShake(){
		
		if(shakeIntensity < 0.05f){
			shakeIntensity = 0.05f;
		}
		shaking = true;
	}

	private void NewLocation(){
		prevLoc = newLoc;
		newLoc.x = Random.Range(startPos.x - shakeIntensity, startPos.x + shakeIntensity);
		newLoc.y =	Random.Range(startPos.y - shakeIntensity, startPos.y + shakeIntensity);
	}
}
