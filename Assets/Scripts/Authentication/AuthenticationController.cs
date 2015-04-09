using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AuthenticationController : MonoBehaviour {

	private string URLCheckAuthentification =  "http://54.77.118.214/GarrukServer/check_authentication.php"; 												// L'url d'authentification du serveur
	private string URLCheckPermanentConnexion = "http://54.77.118.214/GarrukServer/check_permanent_connexion.php";
	
	private string userMacAdress;
	AuthenticationView view ;
	

	void Start (){
		userMacAdress = SystemInfo.deviceUniqueIdentifier;
		StartCoroutine (permanentConnexion());
		view = GetComponent<AuthenticationView>();
	}

	IEnumerator permanentConnexion()
	{
		WWWForm form = new WWWForm(); 
		form.AddField("myform_hash", ApplicationModel.hash); 	
		form.AddField ("myform_macadress", userMacAdress);
		WWW w = new WWW(URLCheckPermanentConnexion, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w;

		if (w.error != null){
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else{
			if (!w.text.Equals("")) 					// On affiche la page d'accueil si l'authentification réussie
			{ 				
				if(ApplicationModel.toDeconnect==false){
					ApplicationModel.username = w.text;
					Application.LoadLevel("HomePage");
				}
				else{
					ApplicationModel.toDeconnect=false;
					view.toDisplayWindow();
					view.setNick(w.text);
					view.toMemorizeLogins();
				}
			}
			else{
				view.toDisplayWindow();
			}
		}
	}

	public IEnumerator Login(string formNick, string formPassword, bool memorizeLogins) 
	{
		if (formNick == "" || formPassword == "")
			yield break;

		string memorize = "0";
		if (memorizeLogins)
			memorize = "1";

		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", formNick);
		form.AddField("myform_pass", formPassword);
		form.AddField ("myform_memorize", memorize);
		form.AddField ("myform_macadress", userMacAdress);
		
		WWW w = new WWW(URLCheckAuthentification, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		
		if (w.error != null) 
		{
			//print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			//print(w.text);
			if (w.text.Equals("PASSWORD CORRECT")) 					// On affiche la page d'accueil si l'authentification réussie
			{ 				
				ApplicationModel.username = formNick;
				Application.LoadLevel("HomePage");
			}
			else 
			{
				view.setError("informations incorrectes, " +			// on affiche les données transmises du serveur
					"veuillez réessayer: " + w.text);
			}											// on supprime la connexion
		}
		formPassword = ""; 											// On efface les variables
	}
}
