using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Model
{
	public string[] arrayList;
	private WWWForm form;
	private WWW w;
	private string URLServeur = "http://54.77.118.214/GarrukServer/" ;

	public Model() 
	{
	
	}

	public string[] getArrayList(string URL){
		return arrayList;
	}

	private IEnumerator sendBDDRequest(string URL) {

		form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		
		w = new WWW(URLServeur+URL, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		
		if (w.error != null) 
		{
			//print(w); 														// donne l'erreur eventuelle
		} 
		else 
		{
			//print(w); 														// donne le retour
			arrayList = w.text.Split('\n'); 					// Chaque ligne du serveur correspond à une carte												// Fonction d'affichage des cartes	
		}
	}
}