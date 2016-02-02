using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
/**
 ** Classe permettant de stocker certaines informations lors des chargements de niveaux
*/
public class ApplicationModel : MonoBehaviour
{
	static public Player player;
	static public string host;
	static public string hash;
	static public string photonSettings;                    // identifiant utilisé par Photon A mettre à jour à chaque nouvelle version
	static public int nbCardsByDeck;

	static public string myPlayerName;
	static public string hisPlayerName;

	static ApplicationModel()
	{
		player=new User();
		host = "http://54.77.118.214/GarrukServer/"; // local http://localhost/GarrukServer/ 
		hash = "J8xy9Uz4";
		photonSettings = "0.2";	 
		nbCardsByDeck=4;
		myPlayerName="";
		hisPlayerName="";
	}
}