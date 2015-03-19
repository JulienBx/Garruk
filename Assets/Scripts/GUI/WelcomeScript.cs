using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WelcomeScript : MonoBehaviour 
{
	private string URLGetMoney = "http://54.77.118.214/GarrukServer/get_money_by_user.php";
	private Text titre;

	void Start(){ 
		StartCoroutine(loadMoney ());
		titre = GetComponent<Text>();
	}

	void OnGUI(){
		titre.text = "Bienvenue "+ApplicationModel.username+"    "+ApplicationModel.credits+" crédits";
	}

	private IEnumerator loadMoney(){
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté

		WWW w = new WWW(URLGetMoney, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
//			print ("crédits "+w.text);
		}
		ApplicationModel.credits = System.Convert.ToInt32(w.text);
	}
}
