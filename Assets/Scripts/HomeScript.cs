﻿using UnityEngine;
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
	}

	void Update() {
		//Instantiate
	}
}
