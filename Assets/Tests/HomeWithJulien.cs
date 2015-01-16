using UnityEngine;
using System.Collections;

public class HomeWithJulien : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ApplicationModel.username = "yoann";
		ApplicationModel.selectedDeck = new Deck(1, "deck 1", 2);
		Application.LoadLevel("MyDeckPage");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
