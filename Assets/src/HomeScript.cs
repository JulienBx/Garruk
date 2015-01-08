using UnityEngine;
using System.Collections;

public class HomeScript : MonoBehaviour {
	
	void OnGUI () {
		string username = ApplicationModel.username; // récupère la variable static
		GUI.Label(new Rect (90, 10, 100, 20), "Hello " + username); // et l'affiche
	}
}
