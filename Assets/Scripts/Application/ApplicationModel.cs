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
	static public string host = "http://54.77.118.214/GarrukServer/";	// Adresse du serveur
	static public string dev = "http://localhost/GarrukServer/";		// Adresse du serveur
	static public string username = "guillaume"; 
	static public string hash = "J8xy9Uz4"; 						// Clé secrète
	static public Deck selectedDeck;								// Deck à afficher
	static public string photonSettings = "0.2";                    // identifiant utilisé par Photon A mettre à jour à chaque nouvelle version
	static public string profileChosen = "";
	static public bool toDeconnect = false;
	static public int credits = 0;
	static public int nbNotificationsNonRead = 0;
	static public int gameType = 0;
	static public FriendlyGame currentFriendlyGame;
	static public Cup currentCup;
	static public Division currentDivision;
	static public string error = "";
	static public string skillChosen = "";
	static public int cardTypeChosen = -1;
	static public int tutorialStep;
	static public bool isAdmin = true;
	static public int nbCardsByDeck = 4;
	static public int packToBuy = -1;
	static public bool launchGameTutorial = false;
	static public bool launchEndGameSequence = false;
	static public bool hasWonLastGame = false;
	static public bool isFirstPlayer=false;
	static public string hisPlayerName = "";
	static public string myPlayerName="";

	static private string URLCheckPassword = host + "check_password.php";
	static private string URLEditPassword = host + "edit_password.php";
	static private string URLCheckAuthentification = host + "check_authentication.php"; 
	static private string URLCheckPermanentConnexion = host + "check_permanent_connexion.php";
	static private string URLCreateAccount = host + "create_account.php";

	static public IEnumerator checkPassword(string password)
	{
		error = "";
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", username); 
		form.AddField("myform_pass", password);
		
		WWW w = new WWW(URLCheckPassword, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		
		if (w.error != null)
		{
			print(w.error); 										// donne l'erreur eventuelle
		} else
		{
			if (w.text != "") 					// On affiche la page d'accueil si l'authentification réussie
			{ 				
				error = w.text;
			}
		}
	}
	static public IEnumerator editPassword(string password)
	{
		WWWForm form = new WWWForm(); 								 //Création de la connexion
		form.AddField("myform_hash", hash); 		 				//hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", username); 
		form.AddField("myform_pass", password);
		
		WWW w = new WWW(URLEditPassword, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		
		if (w.error != null)
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
	}
	static public IEnumerator permanentConnexion()
	{
		WWWForm form = new WWWForm(); 
		form.AddField("myform_hash", ApplicationModel.hash); 	
		form.AddField("myform_macadress", SystemInfo.deviceUniqueIdentifier); 	
		
		WWW w = new WWW(URLCheckPermanentConnexion, form);
		yield return w;
		
		if (w.error != null)
		{
			error = w.error;
		} else
		{
			if (w.text.Contains("#ERROR#"))
			{
				string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				error = errors [1];
			} else
			{
				error = "";
				string[] data = w.text.Split(new string[] { "//" }, System.StringSplitOptions.None);
				username = data [0];
				tutorialStep = System.Convert.ToInt32(data [1]);
				isAdmin = System.Convert.ToBoolean(System.Convert.ToInt32(data [2]));
			}		
		}
	}
	static public IEnumerator Login(string nick, string password, bool toMemorize)
	{	
		string toMemorizeString;
		if (toMemorize)
		{
			toMemorizeString = "1";
		} else
		{
			toMemorizeString = "0";
		}
		WWWForm form = new WWWForm();
		form.AddField("myform_hash", ApplicationModel.hash);
		form.AddField("myform_nick", nick);
		form.AddField("myform_pass", password);
		form.AddField("myform_memorize", toMemorizeString);
		form.AddField("myform_macadress", SystemInfo.deviceUniqueIdentifier);
		
		WWW w = new WWW(URLCheckAuthentification, form);
		yield return w;
		
		if (w.error != null)
		{
			error = w.error;
		} else
		{
			if (w.text.Contains("#ERROR#"))
			{
				string[] errors = w.text.Split(new string[] { "#ERROR#" }, System.StringSplitOptions.None);
				error = errors [1];
			} else
			{
				error = "";
				string[] data = w.text.Split(new string[] { "//" }, System.StringSplitOptions.None);
				toDeconnect = false;
				username = data [0];
				tutorialStep = System.Convert.ToInt32(data [1]);
				isAdmin = System.Convert.ToBoolean(System.Convert.ToInt32(data [2]));
			}											
		}
	}
	static public IEnumerator createAccount(string nick, string email, string password)
	{	
		WWWForm form = new WWWForm();
		form.AddField("myform_hash", ApplicationModel.hash);
		form.AddField("myform_nick", nick);
		form.AddField("myform_email", email);
		form.AddField("myform_pass", password);
		form.AddField("myform_macadress", SystemInfo.deviceUniqueIdentifier);
		
		WWW w = new WWW(URLCreateAccount, form);
		yield return w;
		
		if (w.error != null)
		{
			error = w.error;
		} else
		{
			if (w.text == "")
			{	 				
				username = nick;
				toDeconnect = false;
			} else
			{
				error = w.text;
			}										
		}
	}
}