using UnityEngine;
using System.Collections;

public class testScript : MonoBehaviour {

	// Use this for initialization
	void onGUI () {
		if (GUI.Button(new Rect(100, 100, 100, 100), "Accueil"))		// Au clic sr le bouton d'accueil
		{
			Application.LoadLevel("HomePage");						// On charge la page d'accueil
		}
	}
}
