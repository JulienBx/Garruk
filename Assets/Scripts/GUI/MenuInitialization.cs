using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuInitialization : MonoBehaviour 
{
	private string URLGetUserData = "http://54.77.118.214/GarrukServer/get_user_data.php";
	private Text name;
	private Text notifications;
	
	void Start(){ 
		StartCoroutine(loadUserData ());
		name=GameObject.Find("Name").GetComponent<Text>();
		notifications=GameObject.Find("Notifications").GetComponent<Text>();
	}
	
	void OnGUI(){
		notifications.text = ApplicationModel.nbNotificationsNonRead.ToString();
		name.text = "Bienvenue "+ApplicationModel.username+"    "+ApplicationModel.credits+" crédits";
	}
	
	private IEnumerator loadUserData(){
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		
		WWW w = new WWW(URLGetUserData, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			print(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			string[] data=w.text.Split(new string[] { "//" }, System.StringSplitOptions.None);
			ApplicationModel.credits = System.Convert.ToInt32(data[0]);
			ApplicationModel.nbNotificationsNonRead = System.Convert.ToInt32(data[1]);
		}
		
	}
}
