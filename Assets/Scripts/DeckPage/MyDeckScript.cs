using UnityEngine; 
using System.Collections;
using System.Collections.Generic;

public class MyDeckScript : MonoBehaviour
{
	void Start() {
		
	}
	
	void Awake() {
	}

	
	void OnGUI()
	{
		if (GUI.Button(new Rect(10, 10, 100, 20), "Accueil"))		// Au clic sr le bouton d'accueil
		{
			Application.LoadLevel("HomePage");						// On charge la page d'accueil
		}
		if(Application.loadedLevelName.Equals("MyDeckPage")) 
		{
			if (GUI.Button(new Rect(120, 10, 100, 20), "Mes decks"))	// bouton pour aller sur la page "Mes decks"
			{ 
				Application.LoadLevel("MyDecksPage"); 				// Chargement de la page "Mes decks"
			}
		}

		GUI.Label(new Rect(230, 10, 200, 20), ApplicationModel.selectedDeck.Name);
	}
}