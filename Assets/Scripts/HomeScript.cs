using UnityEngine;
using System.Collections;

public class HomeScript : MonoBehaviour {
	
	void OnGUI () {
		string username = ApplicationModel.username; // récupère la variable static
		GUI.Label(new Rect (90, 10, 100, 20), "Hello connard " + username); // et l'affiche

		if ( GUI.Button(new Rect (10, 50, 80, 20) , "Mes cartes" ) ){ // bouton de connexion
			Application.LoadLevel("MyCardsPage"); // Chargement de la page "Mes cartes"
		}
	}
}
