using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class authentication : MonoBehaviour {

	string URLCheckAuthentification =  "http://54.77.118.214/GarrukServer/check_authentication.php"; 												// L'url d'authentification du serveur
	string URLCheckPermanentConnexion = "http://54.77.118.214/GarrukServer/check_permanent_connexion.php";
	string formNick = ""; 									// Le champ où l'utilisateur entre son pseudo
	string formPassword = ""; 
	string error ="";
	float resize=0;
	bool isinitialized=false;
	bool isdataloaded=false;
	bool memorizeLogins = false;
	string macAdress;

	Rect windowRect ;


	void Start (){
		macAdress = SystemInfo.deviceUniqueIdentifier;
		StartCoroutine (permanentConnexion());
	}


	void OnGUI () 
	{

		if (isdataloaded)
		windowRect = GUI.Window(0, new Rect(Screen.width/2-Screen.width/8, Screen.height/2-Screen.height/7f-resize/2, Screen.width/4, Screen.height/3.5f+resize), authentificationWindow, "S'authentifier à Garruk");

	}

	void authentificationWindow(int windowID) {
		
		Event e = Event.current;
		if (e.keyCode == KeyCode.Return) {
		StartCoroutine(Login(formNick, formPassword));
		}


		GUILayout.BeginArea(new Rect(windowRect.width/8,windowRect.height/8,windowRect.width, windowRect.height));
		{
			GUILayout.Label ("Identifiant",GUILayout.Width(0.75f*windowRect.width));
			GUI.SetNextControlName("formNick");
			formNick = GUILayout.TextField(formNick,GUILayout.Width(0.75f*windowRect.width));
			GUILayout.Label ("Mot de passe",GUILayout.Width(0.75f*windowRect.width));
			GUI.SetNextControlName("formPassword");
			formPassword = GUILayout.TextField(formPassword,GUILayout.Width(0.75f*windowRect.width));

			memorizeLogins = GUILayout.Toggle(memorizeLogins, "Mémoriser ma session");
			GUILayout.Space (windowRect.height*0.05f);
			GUI.SetNextControlName("Confirmer");
			if (GUILayout.Button("Confirmer",GUILayout.Width(0.375f*windowRect.width))){
				StartCoroutine(Login(formNick, formPassword));
			}


			GUILayout.Label (error,GUILayout.Width(0.75f*windowRect.width));
			if (!isinitialized){
				GUI.FocusControl("formNick");
				isinitialized=true;
			}


		}
		GUILayout.EndArea();
	}
	

	IEnumerator permanentConnexion()
	{
		WWWForm form = new WWWForm(); 
		form.AddField("myform_hash", ApplicationModel.hash); 	
		form.AddField ("myform_macadress", macAdress);
		WWW w = new WWW(URLCheckPermanentConnexion, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w;

		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			//print(w.text);
			if (!w.text.Equals("")) 					// On affiche la page d'accueil si l'authentification réussie
			{ 				
				if(ApplicationModel.toDeconnect!=true){
				
				ApplicationModel.username = w.text;
				Application.LoadLevel("HomePage");
				
				}
				else{
				
				ApplicationModel.toDeconnect=false;
				isdataloaded=true;
				formNick=w.text;
				memorizeLogins=true;
				}
			}
			else 
				isdataloaded=true;
		}

	}
	


	IEnumerator Login(string formNick, string formPassword) 
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
		form.AddField ("myform_macadress", macAdress);
		
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
				resize = Screen.height/10;
				error = "informations incorrectes, " +			// on affiche les données transmises du serveur
					"veuillez réessayer: " + w.text;
			}
			//w.Dispose(); 											// on supprime la connexion
		}
		formPassword = ""; 											// On efface les variables
	}
}
