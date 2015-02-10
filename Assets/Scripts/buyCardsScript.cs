using UnityEngine;
using System.Collections;

public class buyCardsScript : MonoBehaviour {

	public string scriptGenerateURL = null ;

	// Use this for initialization
	void Start (){
	
	}

	void OnGUI() {
	
		// Deal button
		if (GUI.Button(new Rect(10, 10, 100, 20), "Accueil"))
		{
			Application.LoadLevel("HomePage");
		}

		if (GUI.Button(new Rect(10, 40, 150, 20), "Me créer une carte"))
		{
			StartCoroutine(generateRandomCard());
		}
	}

	private IEnumerator generateRandomCard(){

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW("localhost/GarrukServer/buyRandomCards.php", form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			print (w.error); 										// donne l'erreur eventuelle
		} else {
			print (w.text);
			

		}
	}
}
