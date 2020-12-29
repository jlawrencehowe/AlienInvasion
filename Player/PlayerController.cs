using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public GameObject barrel;
	public GameObject beam;
	public GameObject currentBeam;
	public Transform rayCastStartPos;
	public float firstChargeAmount = 5, secondChargeAmount = 2.5f;
	public bool isFiring = false;
	public bool chargingBeam = false;
	public Image fullBar, halfBar;
	public bool doubleScore = false, doubleDamage = false;
	public float scoreTimer = 0, doubleDamageTimer = 0;
	public AudioClip beamSFX;
	public AudioSource audioSource;
	public CameraShake cameraShake;
	public GameObject beamHit;
	public GameObject currentBeamHit;
	public Image doubleDamagePUDisplay, doubleDamagePUTimer, doubleScorePUDisplay, doubleScorePUTimer;
	public Animator barrelAnimator;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//this section creates the angle for the barrel to follow the mouse
		Vector3 mouse = Input.mousePosition;
		Vector3 screenPoint = Camera.main.WorldToScreenPoint(barrel.transform.position);
		Vector3 offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
		float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, angle - 90);


		//if fire button is down, create new beam if one isn't created
		if(Input.GetButton("Fire1") && !isFiring){
			currentBeam = Instantiate(beam, new Vector3(rayCastStartPos.position.x, rayCastStartPos.position.y, 1), this.transform.rotation) as GameObject;
			currentBeam.transform.parent = this.transform;
			isFiring = true;
			barrelAnimator.SetBool("isFiring", true);

		}
		if(!audioSource.isPlaying && Input.GetButton("Fire1"))
			audioSource.PlayOneShot(beamSFX);
		//turns off beam when fire button is up
		if(!Input.GetButton("Fire1") && isFiring){
			Destroy(currentBeam);
			currentBeam = null;
			isFiring = false;
			audioSource.Stop();
			
			barrelAnimator.SetBool("isFiring", false);
			if(currentBeamHit != null){
				Destroy(currentBeamHit);
			}
		}
		if(Input.GetButton("Fire1") && doubleDamage){
			cameraShake.MiniShake();
		}

		//ray from turret to mouse and beyond
		RaycastHit2D ray = Physics2D.Raycast(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) - 
		                                     rayCastStartPos.position, Mathf.Infinity);
		//draws ray
		Debug.DrawRay(rayCastStartPos.position, Camera.main.ScreenToWorldPoint(Input.mousePosition) - rayCastStartPos.position);
		//if ray is hitting something and it is an enemy, change length to match 
		if(ray.collider != null && ray.collider.tag == "Enemy"){
			Debug.Log("Hot");
			//if there is a beam
			if(currentBeam != null){
				currentBeam.GetComponent<Beam>().ChangeLength(Mathf.Abs(Vector3.Distance
				                                              (ray.collider.gameObject.transform.position ,rayCastStartPos.position) 
				                                                        ));
				if(!doubleDamage){
					ray.collider.GetComponent<EnemyBase>().GetHit(false, 1);
				}
				else{
					ray.collider.GetComponent<EnemyBase>().GetHit(false, 2);
				}
				if(currentBeamHit == null){
					currentBeamHit = Instantiate(beamHit, ray.collider.gameObject.transform.position, barrel.transform.rotation) as GameObject;
				}
				/*
				if(currentBeamHit != null){
					currentBeamHit.GetComponent<ParticleSystem>().enableEmission = true;
				}
				*/
				currentBeamHit.transform.position = ray.collider.gameObject.transform.position;
			}
		}
		else{
			if(currentBeam != null){
				currentBeam.GetComponent<Beam>().ChangeLength(40);

			}
			if(currentBeamHit != null){
				Destroy(currentBeamHit);
			}
			/*
			if(currentBeamHit != null){
				currentBeamHit.GetComponent<ParticleSystem>().enableEmission = false;
			}
			*/
		}
		if(currentBeam != null){
			currentBeam.GetComponent<Beam>().ChangeColor(doubleDamage);
		}
		Debug.Log(Input.touchCount);
		if(Input.touchCount > 0){


			/*
			Vector3 touchPosition = Input.GetTouch(0).position;
			Vector3 objectPos = Camera.main.WorldToScreenPoint(barrel.transform.position);
			touchPosition.x = touchPosition.x - objectPos.x;
			touchPosition.y = touchPosition.y - objectPos.y;
			float angle = Mathf.Atan2(touchPosition.y, touchPosition.x) * Mathf.Rad2Deg;
			barrel.transform.rotation = Quaternion.Euler(0, 0, angle);
			*/
		}

		//CheckBeamGauge();
		if(doubleDamage){
			//doubleDamagePUDisplay.enabled = true;
			doubleDamageTimer -= Time.deltaTime;
			doubleDamagePUTimer.transform.localScale = new Vector3(doubleDamageTimer/10, 1, 1);
			if(doubleDamageTimer <= 0){
				doubleDamage = false;
			//	doubleDamagePUDisplay.enabled = false;
			}
		}
		if(doubleScore){
			//doubleScorePUDisplay.enabled = true;
			scoreTimer -= Time.deltaTime;
			doubleScorePUTimer.transform.localScale = new Vector3(scoreTimer/10, 1, 1);
			if(scoreTimer <= 0){
				doubleScore = false;
			//	doubleScorePUDisplay.enabled = false;
			}
			/*
			if(!doubleDamagePUDisplay.enabled){
				doubleScorePUDisplay.rectTransform.localPosition = new Vector3(-141, 171, 0);
			}
			else{
				doubleScorePUDisplay.rectTransform.localPosition = new Vector3(-141, 184.4f, 0);
			}
			*/
		}



	}

	private void CheckBeamGauge(){


		if(isFiring && !chargingBeam){
			if(firstChargeAmount > 0){
				
				firstChargeAmount -= Time.deltaTime;
				if(firstChargeAmount < 0){
					firstChargeAmount = 0;
				}
				
			}
			else if(secondChargeAmount > 0 && firstChargeAmount == 0){
				
				secondChargeAmount -= Time.deltaTime;
				if(secondChargeAmount < 0){
					secondChargeAmount = 0;
				}
			}
			else if(secondChargeAmount <= 0){
				chargingBeam = true;
				isFiring = false;
				Destroy(currentBeam);
			}
		}
		else{
			
			if(secondChargeAmount != 2.5f){
				secondChargeAmount += Time.deltaTime;
				if(secondChargeAmount > 2.5f){
					secondChargeAmount = 2.5f;
				}
			}
			else if(firstChargeAmount != 5 && secondChargeAmount >= 2.5f){
				chargingBeam = false;
				firstChargeAmount += Time.deltaTime * 1.5f;
				if(firstChargeAmount > 5){
					firstChargeAmount = 5;
				}
			}
			
		}

		UpdateBeamGuageSize();

	}

	private void UpdateBeamGuageSize(){
		fullBar.rectTransform.localScale = new Vector3(firstChargeAmount/5, 1, 1);
		halfBar.rectTransform.localScale = new Vector3(secondChargeAmount/2.5f, 1, 1);
	}


}
