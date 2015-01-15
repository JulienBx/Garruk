using UnityEngine;
using System.Collections;

public class HomeWithJulien : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ApplicationModel.username = "julien";
		ApplicationModel.selectedDeck = "deck 1";
		Application.LoadLevel("MyDeckPage");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
