using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MenuModel {
	
	public User player;
	public string[] buttonsLabels;
	public bool isInvited;
	public string invitationError;
	
	private string URLGetUserData = ApplicationModel.host+"get_user_data.php";
	private string URLRefreshUserData = ApplicationModel.host+"refresh_user_data.php";
	
	public MenuModel()
	{
		this.player = new User ();
		this.buttonsLabels = new string[6];
		this.buttonsLabels [0] = "Accueil";
		this.buttonsLabels [1] = "Mes unités";
		this.buttonsLabels [2] = "Recruter";
		this.buttonsLabels [3] = "Le marché";
		this.buttonsLabels [4] = "Cristalopedia";
		this.buttonsLabels [5] = "Jouer";
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
			//Debug.Log(this.player.nonReadNotifications);
		}
	}
	public IEnumerator refreshUserData(int totalNbResultLimit, bool isInviting)
	{
		
		string isInvitingString = "0";
		if(isInviting)
		{
			isInvitingString="1";
		}
		
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_limit", totalNbResultLimit); 
		form.AddField("myform_isinviting", isInvitingString);
		
		WWW w = new WWW(URLRefreshUserData, form); 								// On envoie le formulaire à l'url sur le serveur 
		yield return w; 											// On attend la réponse du serveur, le jeu est donc en attente
		if (w.error != null) 
		{
			Debug.Log(w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.player = parseUser(data[0].Split(new string[] { "//" }, System.StringSplitOptions.None));
			if(isInviting)
			{
				this.invitationError=data[1];
			}
			if(data[2]!="-1")
			{
				this.isInvited=true;
			}
			else
			{
				this.isInvited=false;
			}
		}
	}
	private User parseUser(string[] array)
	{
		User player = new User ();
		player.Money=System.Convert.ToInt32(array[0]);
		player.nonReadNotifications= System.Convert.ToInt32(array[1]);
		player.idProfilePicture = System.Convert.ToInt32(array [2]);
		return player;
	}
}

