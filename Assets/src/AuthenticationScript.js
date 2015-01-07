﻿private var formNick = ""; // Le champ où l'utilisateur entre son pseudo
private var formPassword = ""; // Celui pour le mot de passe
var formText = ""; // La réponse du serveur PHP
 
var URL = "http://localhost/garruk/check_authentication.php"; // L'url d'authentification du serveur
var hash = "J8xy9Uz4"; // clé secrète
 
private var textrect = Rect (10, 130, 800, 23); // créé un rectangle visuel
 
function OnGUI() {
	GUI.Label( Rect (10, 5, 140, 20), "Page d'authentification" ); // label texte
    GUI.Label( Rect (10, 30, 100, 20), "votre identifiant:" ); // label texte
    GUI.Label( Rect (10, 60, 160, 20), "votre mot de passe:" );
 
    formNick = GUI.TextField ( Rect (150, 30, 100, 20), formNick ); // Input pour le pseudo
    formPassword = GUI.TextField ( Rect (150, 60, 100, 20), formPassword ); // Celui pour le mot de passe
 
    if ( GUI.Button ( Rect (10, 90, 100, 20) , "s'authentifier" ) ){ // bouton de connexion
        Login();
    }
    GUI.TextArea( textrect, formText );
}
 
function Login() {
    var form = new WWWForm(); // Création de la connexion
    form.AddField( "myform_hash", hash ); // hashcode de sécurité, doit etre identique à celui sur le serveur
    form.AddField( "myform_nick", formNick );
    form.AddField( "myform_pass", formPassword );
    var w = WWW(URL, form); // On envoie le formulaire à l'url sur le serveur 
    yield w; // On attend la réponse du serveur, le jeu est donc en attente
    if (w.error != null) {
        print(w.error); // donne l'erreur eventuelle
    } else {
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