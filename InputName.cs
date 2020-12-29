using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputName : MonoBehaviour {

	public bool keyIsActive = false, firstTime = false;
	private PersistingGameData persistingGD;
	private Canvas inputNameCanvas;
	public OptionsMenu optionsMenu;
	private string tempString = "";
	private TouchScreenKeyboard keyboard;
	private bool iskBoardOpen = false;
	// Use this for initialization
	void Start () {
	
		inputNameCanvas = this.GetComponent<Canvas>();
		persistingGD = GameObject.Find("PersistingGameData").GetComponent<PersistingGameData>();

	}
	
	// Update is called once per frame
	void Update () {

		

			kBoard0();
		}

	public void kBoard0()
	{
		// keyboard = iPhoneKeyboard.Open( tempName : String, iPhoneKeyboardType.Default, Auto-Correct, Multi-Line, Secure, Alert, "Text Placeholder" );

		if(firstTime && keyIsActive){
			inputNameCanvas.enabled = true;
			firstTime = false;
		}

		if ( keyIsActive ) 
		{

			keyboard = TouchScreenKeyboard.Open(tempString);
			keyIsActive = false;
		}
		
		if ( keyboard.done ) 
		{
			optionsMenu.userName.text = "Current User: " + keyboard.text;
			inputNameCanvas.enabled = false;
			persistingGD.currentUserName = keyboard.text;
			keyIsActive = false;
		}

	}
		
		
}
