using UnityEngine;
using System.Collections;

public class AuthenticationScript : MonoBehaviour 
{
	private string formNick = ""; 									// Le champ où l'utilisateur entre son pseudo
	private string formPassword = ""; 								// Celui pour le mot de passe
//	private string formText = ""; 									// La réponse du serveur PHP
//	private Rect textrect = new Rect(10, 130, 800, 23); 			// créé un rectangle visuel
	
	public string URL; 												// L'url d'authentification du serveur
		
	void OnGUI() 
	{	
		
	}

	IEnumerator Login() 
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", formNick);
		form.AddField("myform_pass", formPassword);

		WWW w = new WWW(URL, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente

		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			print(w.text);
			if (w.text.Equals("PASSWORD CORRECT")) 					// On affiche la page d'accueil si l'authentification réussie
			{ 				
				ApplicationModel.username = formNick;
				Application.LoadLevel("HomePage");
			}
			else 
			{
//				formText = "informations incorrectes, " +			// on affiche les données transmises du serveur
//					"veuillez réessayer: " + w.text;
			}
			w.Dispose(); 											// on supprime la connexion
		}
		
		formPassword = ""; 											// On efface les variables
	}
}
