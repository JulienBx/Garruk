using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AuthenticationView : MonoBehaviour {

	Rect windowRect ;
	bool toDisplay ;
	AuthenticationController controller ;

	public float resize = 0 ;

	void Start (){
		controller = GetComponent<AuthenticationController>();
	}

	void OnGUI () 
	{
		if (toDisplay){
			windowRect = GUI.Window(0, new Rect(Screen.width/2-Screen.width/8, Screen.height/2-Screen.height/7f-resize/2, Screen.width/4, Screen.height/3.5f+resize), authentificationWindow, "S'authentifier à Garruk");
		}
	}

	public void toDisplayWindow(){
		this.toDisplay = true ;
	}

	void authentificationWindow(int windowID) {
		
//		Event e = Event.current;
//		if (e.keyCode == KeyCode.Return) {
//			StartCoroutine(Login(formNick, formPassword));
//		}
//		
//		GUILayout.BeginArea(new Rect(windowRect.width/8,windowRect.height/8,windowRect.width, windowRect.height));
//		{
//			GUILayout.Label ("Identifiant",GUILayout.Width(0.75f*windowRect.width));
//			GUI.SetNextControlName("formNick");
//			formNick = GUILayout.TextField(formNick,GUILayout.Width(0.75f*windowRect.width));
//			GUILayout.Label ("Mot de passe",GUILayout.Width(0.75f*windowRect.width));
//			GUI.SetNextControlName("formPassword");
//			formPassword = GUILayout.TextField(formPassword,GUILayout.Width(0.75f*windowRect.width));
//
//			memorizeLogins = GUILayout.Toggle(memorizeLogins, "Mémoriser ma session");
//			GUILayout.Space (windowRect.height*0.05f);
//			GUI.SetNextControlName("Confirmer");
//			if (GUILayout.Button("Confirmer",GUILayout.Width(0.375f*windowRect.width))){
//				StartCoroutine(Login(formNick, formPassword));
//			}
//
//
//			GUILayout.Label (error,GUILayout.Width(0.75f*windowRect.width));
//			if (!isinitialized){
//				GUI.FocusControl("formNick");
//				isinitialized=true;
//			}
//
//
//		}
//		GUILayout.EndArea();
	}

	IEnumerator Login(string formNick, string formPassword) 
	{
//		if (formNick == "" || formPassword == "")
//			yield break;
//
//		string memorize = "0";
//		if (memorizeLogins)
//			memorize = "1";
//
//		WWWForm form = new WWWForm(); 								// Création de la connexion
//		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
//		form.AddField("myform_nick", formNick);
//		form.AddField("myform_pass", formPassword);
//		form.AddField ("myform_memorize", memorize);
//		form.AddField ("myform_macadress", macAdress);
//		
//		WWW w = new WWW(URLCheckAuthentification, form); 								// On envoie le formulaire à l'url sur le serveur 
//		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
//		
//		if (w.error != null) 
//		{
//			//print(w.error); 										// donne l'erreur eventuelle
//		} 
//		else 
//		{
//			//print(w.text);
//			if (w.text.Equals("PASSWORD CORRECT")) 					// On affiche la page d'accueil si l'authentification réussie
//			{ 				
//				ApplicationModel.username = formNick;
//				Application.LoadLevel("HomePage");
//			}
//			else 
//			{
//				resize = Screen.height/10;
//				error = "informations incorrectes, " +			// on affiche les données transmises du serveur
//					"veuillez réessayer: " + w.text;
//			}
//			//w.Dispose(); 											// on supprime la connexion
//		}
//		formPassword = ""; 											// On efface les variables
		yield break;
	}
}
