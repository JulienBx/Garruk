using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class newMenuModel {

	public User player;
	public string[] buttonsLabels;

	private string URLGetUserData = ApplicationModel.host+"get_user_data.php";
	private string URLRefreshUserData = ApplicationModel.host+"refresh_user_data.php";
	
	public newMenuModel()
	{
		this.player = new User ();
		this.buttonsLabels = new string[5];
		this.buttonsLabels [0] = "Accueil";
		this.buttonsLabels [1] = "Mes cartes";
		this.buttonsLabels [2] = "La boutique";
		this.buttonsLabels [3] = "Le bazar";
		this.buttonsLabels [4] = "Jouer";
	}
	
	public IEnumerator loadUserData(int totalNbResultLimit){
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_limit", totalNbResultLimit); 
		
		WWW w = new WWW(URLGetUserData, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.player = parseUser(data[0].Split(new string[] { "//" }, System.StringSplitOptions.None));
		}
	}
	public IEnumerator refreshUserData(int totalNbResultLimit)
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_limit", totalNbResultLimit); 
		
		WWW w = new WWW(URLGetUserData, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			string[] data=w.text.Split(new string[] { "//" }, System.StringSplitOptions.None);
			this.player.Money = System.Convert.ToInt32(data[0]);
			this.player.nonReadNotifications = System.Convert.ToInt32(data[1]);
		}
	}
	private User parseUser(string[] array)
	{
		User player = new User ();
		player.Money=System.Convert.ToInt32(array[0]);
		player.nonReadNotifications= System.Convert.ToInt32(array[1]);
		player.ThumbPicture = array [2];
		return player;
	}
}

