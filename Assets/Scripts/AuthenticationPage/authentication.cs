using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class authentication : MonoBehaviour {

	public string URL; 												// L'url d'authentification du serveur
	
	public void loginServeur() 
	{	
		string formNick = ""; 									// Le champ où l'utilisateur entre son pseudo
		string formPassword = ""; 

		GameObject obj = GameObject.Find("InputLogin");
		InputField inp = obj.GetComponent<InputField>();
		formNick =  inp.text;

		GameObject obj2 = GameObject.Find("InputMdp");
		InputField inp2 = obj2.GetComponent<InputField>();
		formPassword =  inp2.text;
		
		StartCoroutine(Login(formNick, formPassword));

		//GUI.TextArea(textrect, formText);
	}
	
	IEnumerator Login(string formNick, string formPassword) 
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", formNick);
		form.AddField("myform_pass", formPassword);
		
		WWW w = new WWW(URL, form); 								// On envoie le formulaire à l'url sur le serveur 
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
				GameObject obj = GameObject.Find("ErrorText");
				var texte = obj.GetComponent<Text>();
				texte.text = "informations incorrectes, " +			// on affiche les données transmises du serveur
					"veuillez réessayer: " + w.text;
			}
			//w.Dispose(); 											// on supprime la connexion
		}
		formPassword = ""; 											// On efface les variables
	}
}
