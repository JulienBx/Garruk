using UnityEngine;
using System.Collections;

public class HomeScript : MonoBehaviour 
{
	
	void OnGUI () 
	{
		string username = ApplicationModel.username; 				// récupère la variable static
		GUI.Label(new Rect (90, 10, 150, 20), "Hello " + username); // et l'affiche

		if (GUI.Button(new Rect(10, 50, 80, 20), "Mes cartes"))		// bouton pour aller sur la page "Mes cartes"
		{ 
			Application.LoadLevel("MyCardsPage"); 					// Chargement de la page "Mes cartes"
		}

		if (GUI.Button(new Rect(100, 50, 80, 20), "Mes decks"))		// bouton pour aller sur la page "Mes decks"
		{ 
			Application.LoadLevel("MyDecksPage"); 						// Chargement de la page "Mes decks"
		}
		
		if (GUI.Button(new Rect(10, 80, 170, 20), "Créer des cartes"))// bouton pour aller sur la page "Création des cartes"
		{ 
			Application.LoadLevel("buyCards"); 						// Chargement de la page "Création des cartes"
		}
		if (GUI.Button(new Rect(200, 50, 100, 20), "Déconnexion"))	// bouton pour aller sur la page "Création des cartes"
		{ 
			ApplicationModel.username = "";
			Application.LoadLevel("ConnectionPage"); 				// Chargement de la page "dauthentification"
		}
	}

	void Update() {
		//Instantiate
	}
}
