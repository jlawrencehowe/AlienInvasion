using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour {

	public Renderer beamRenderer;
	float texAmount = 0;
	public float textAddRate = 0.5f;
	public LineRenderer beam1, beam2;
	private float lerpTimer;

	// Use this for initialization
	void Awake () {
		lerpTimer = 1;
		beamRenderer = this.gameObject.GetComponent<Renderer>();

	}
	
	// Update is called once per frame
	void Update () {
		texAmount += Time.deltaTime * textAddRate;
		beamRenderer.material.SetFloat("_AddTex",texAmount);

	}

	public void ChangeLength(float newLength){
		beamRenderer.material.SetFloat("_BeamLength", newLength/40f);
	}

	public void ChangeColor(bool doublePower){
		if(doublePower){
			this.GetComponent<LineRenderer>().SetColors(new Color32(180, 153, 0, 255), new Color32(118, 107, 20, 255));
			beam2.SetColors(new Color32(180, 153, 0, 255), new Color32(118, 107, 20, 255));
			beam1.SetColors(new Color32(118, 107, 20, 255), new Color32(180, 153, 0, 255));
			beam1.SetWidth(0.63f, 1.06f);
			beam2.SetWidth(1.06f, 0.36f);
			beam2.SetPosition(1, new Vector3(0, 0.53f, 0));
			lerpTimer = 0;
		}
		else{
			lerpTimer += Time.deltaTime;
			Color tempColor = Color.Lerp(new Color32(180, 153, 0, 255), new Color32(40, 17, 165, 255), lerpTimer);
			Color tempColor2 = Color.Lerp(new Color32(118, 107, 20, 255), new Color32(9, 0, 255, 255), lerpTimer);
			this.GetComponent<LineRenderer>().SetColors(tempColor , tempColor2);
			
			tempColor = Color.Lerp(new Color32(180, 153, 0, 255), new Color32(19, 25, 39, 255), lerpTimer);
			tempColor2 = Color.Lerp( new Color32(118, 107, 20, 255), new Color32(9, 0, 255, 255), lerpTimer);
			beam2.SetColors(tempColor, tempColor2);
			tempColor = Color.Lerp(new Color32(118, 107, 20, 255), new Color32(9, 0, 255, 255), lerpTimer);
			tempColor2 = Color.Lerp( new Color32(180, 153, 0, 255),  new Color32(19, 25, 39, 255), lerpTimer);
			beam1.SetColors(new Color32(9, 0, 255, 255), new Color32(19, 25, 39, 255));
			float tempFloat = Mathf.Lerp(0.63f, 0.36f, lerpTimer);
			float tempFloat2 = Mathf.Lerp(1.06f, 0.73f, lerpTimer);
			beam1.SetWidth(tempFloat, tempFloat2);
			tempFloat = Mathf.Lerp(1.06f, 0.73f, lerpTimer);
			tempFloat2 = Mathf.Lerp(0.36f, 0.36f, lerpTimer);
			beam2.SetWidth(tempFloat, tempFloat2);
			Vector3 tempPos = Vector3.Lerp(new Vector3(0, 0.53f, 0), new Vector3(0, 0.29f, 0), lerpTimer);
			beam2.SetPosition(1, tempPos);
		}
	}
}
