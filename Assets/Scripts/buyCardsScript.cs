using UnityEngine;
using System.Collections;

public class buyCardsScript : MonoBehaviour {

	// Use this for initialization
	void Start (){
	
	}

	void OnGUI() {
	
		// Deal button
		if (GUI.Button(new Rect(10, 10, 100, 20), "Accueil"))
		{
			Application.LoadLevel("HomePage");
		}

		if (GUI.Button(new Rect(10, 40, 150, 20), "Me créer une carte"))
		{
			generateRandomCard();
		}
	}

	void generateRandomCard(){
		
	}
}
