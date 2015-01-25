using UnityEngine;
using System.Collections;

public class HomeWithJulien : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ApplicationModel.username = "julien";
		//ApplicationModel.selectedDeck = new Deck(1, "deck 1", 2);
		Application.LoadLevel("LobbyPage");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
