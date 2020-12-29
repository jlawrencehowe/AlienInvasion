using UnityEngine;
using System.Collections;

public class Cloud : MonoBehaviour {

	private Vector3 startPos, endPos, originalPos;
	private float movementTimer;
	private Camera cam;
	public float speed;

	// Use this for initialization
	void Start () {
		cam = GameObject.Find("Main Camera").GetComponent<Camera>();
		originalPos = this.transform.position;
		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;
		startPos = this.transform.position;
		endPos = new Vector3(width + 0.1f, startPos.y, startPos.z);
		speed = 0.01f;

	}
	
	// Update is called once per frame
	void Update () {

		movementTimer += Time.deltaTime * speed;
		this.transform.position = new Vector3(this.transform.position.x + (Time.deltaTime * 0.1f), this.transform.position.y, this.transform.position.z);
		if(this.transform.position.x > endPos.x){
			Reset();
		}
	
	}

	private void Reset(){
		float height = 2f * cam.orthographicSize;
		float width = height * cam.aspect;
		movementTimer = 0;
		this.transform.position = new Vector3(-width - 1, Random.Range(originalPos.y - 1, originalPos.y + 1), startPos.z);
		startPos = this.transform.position;
		endPos = new Vector3(width + 0.1f, startPos.y, startPos.z);
	}
}
