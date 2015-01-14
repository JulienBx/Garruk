using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WelcomeScript : MonoBehaviour 
{
	void Start(){ 
		var titre = GetComponent<Text>();
		titre.text = "Bienvenue "+ApplicationModel.username;
	}
}
