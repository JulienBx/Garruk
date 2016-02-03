using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
/**
 ** Classe permettant de stocker certaines informations lors des chargements de niveaux
*/
public class ApplicationModel
{
	static public Player player;
	static public string host;
	static public string hash;
	static public string photonSettings;                    // identifiant utilisé par Photon A mettre à jour à chaque nouvelle version
	static public int nbCardsByDeck;

	static public string myPlayerName; // A voir si on le met pas dans player ?
	static public string hisPlayerName; // A voir si on le met pas dans un objet user ?


	static public bool isFirstPlayer; // A REMPLACER PAR ApplicationModel.player.IsFirstPlayer
	static public bool launchGameTutorial; // A REMPLACER PAR ApplicationModel.player.ToLaunchGameTutorial
	static public int gameType; // A REMPLACER PAR ApplicationModel.player.ChosenGameType


	static ApplicationModel()
	{
		host = "http://54.77.118.214/GarrukServer/"; // local http://localhost/GarrukServer/ 
		hash = "J8xy9Uz4";
		photonSettings = "0.2";	 
		nbCardsByDeck=4;
		myPlayerName="";
		hisPlayerName="";
		player=new Player();
	}
}