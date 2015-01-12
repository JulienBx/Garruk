using UnityEngine;
using System.Collections;

public class AuthenticationScript : MonoBehaviour {

	private string formNick = ""; // Le champ où l'utilisateur entre son pseudo
	private string formPassword = ""; // Celui pour le mot de passe
	private string formText = ""; // La réponse du serveur PHP
	
	public string URL = "http://localhost/GarrukServer/check_authentication.php"; // L'url d'authentification du serveur
	
	private Rect textrect = new Rect (10, 160, 800, 23); // créé un rectangle visuel
	
	void OnGUI() {
		GUI.Label(new Rect (10, 5, 140, 50), "Page d'authentification" ); // label texte
		GUI.Label(new Rect (10, 55, 100, 30), "votre identifiant:" ); // label texte
		GUI.Label(new Rect (10, 85, 160, 30), "votre mot de passe:" );
		
		formNick = GUI.TextField (new Rect (150, 55, 100, 20), formNick ); // Input pour le pseudo
		formPassword = GUI.TextField (new Rect (150, 85, 100, 20), formPassword ); // Celui pour le mot de passe
		
		if ( GUI.Button(new Rect (10, 130, 100, 20) , "s'authentifier" ) ){ // bouton de connexion
			StartCoroutine(Login());
		}
		GUI.TextArea( textrect, formText );
	}

	IEnumerator Login() {
		WWWForm form = new WWWForm(); // Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash ); // hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", formNick );
		form.AddField("myform_pass", formPassword );

		WWW w = new WWW(URL, form); // On envoie le formulaire à l'url sur le serveur 
		yield return w; // On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) {
			print(w.error); // donne l'erreur eventuelle
		} else {
			print(w.text);
			if (w.text.Equals("PASSWORD CORRECT")) { // On affiche la page d'accueil si l'authentification réussie
				ApplicationModel.username = formNick;
				Application.LoadLevel("HomePage");
			}
			else {
				formText = "informations incorrectes, veuillez réessayer: " + w.text; // on affiche les données transmises du serveur
			}
			w.Dispose(); // on efface le formulaire
		}
		
		formPassword = ""; // On efface les variables
	}
}
