using UnityEngine;
using System.Collections;

public class CharacterMakerScript : MonoBehaviour {

	GameObject character;
	CharacterScript characterModelScript;

	// Use this for initialization
	void Start () {

		characterModelScript = GameObject.Find("Vampire").GetComponent<CharacterScript> ();

		characterModelScript.showInformations();
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.Return)){
			characterModelScript.toWalk();
		}
		if(Input.GetKeyUp(KeyCode.Return)){
			characterModelScript.stopWalking();
		}
	
	}
}
