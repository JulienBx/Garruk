using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AuthenticationManager : MonoBehaviour {

	private string URLCheckAuthentification =  "http://54.77.118.214/GarrukServer/check_authentication.php"; 												// L'url d'authentification du serveur
	private string URLCheckPermanentConnexion = "http://54.77.118.214/GarrukServer/check_permanent_connexion.php";

	public static AuthenticationManager instance;

	void Start ()
	{
		print ("start Manager");
		instance = this;
	}
	
	public AuthenticationViewModel createAuthenticationViewModel()
	{
		AuthenticationViewModel result = new AuthenticationViewModel();
		return result;
	}
	
	public IEnumerator permanentConnexion(AuthenticationViewModel authenticationViewModel)
	{
		WWWForm form = new WWWForm(); 
		form.AddField("myform_hash", ApplicationModel.hash); 	
		form.AddField("myform_macadress", authenticationViewModel.userMacAdress); 	

		WWW w = new WWW(URLCheckPermanentConnexion, form);
		yield return w;
		
		if (w.error != null)
		{
			print(w.error);
		} 
		else
		{
			authenticationViewModel.userName = w.text;
			authenticationViewModel.isRemembered = !w.text.Equals("");
		}
	}

	public IEnumerator Login(AuthenticationViewModel authenticationViewModel)
	{
		if (authenticationViewModel.userName == "" || authenticationViewModel.password == "")
		{
			yield break;
		}
				
		string memorize = "0";
		if (authenticationViewModel.isMemorizingLogin)
		{
			memorize = "1";
		}
				
		WWWForm form = new WWWForm();
		form.AddField("myform_hash", ApplicationModel.hash);
		form.AddField("myform_nick", authenticationViewModel.userName);
		form.AddField("myform_pass", authenticationViewModel.password);
		form.AddField ("myform_memorize", memorize);
		form.AddField ("myform_macadress", authenticationViewModel.userMacAdress);
				
		WWW w = new WWW(URLCheckAuthentification, form);
		yield return w;
				
		if (w.error != null) 
		{
			//print(w.error);
		} 
		else 
		{
			//print(w.text);
			if (w.text.Equals("PASSWORD CORRECT")) 					// On affiche la page d'accueil si l'authentification réussie
			{	 				
				ApplicationModel.username = authenticationViewModel.userName;
				Application.LoadLevel("HomePage");
			}
			else 
			{
				authenticationViewModel.connexionError = "informations incorrectes, " + "veuillez réessayer: " + w.text;
				authenticationViewModel.password = "";
			}											
		}
		yield break;
	}
}
