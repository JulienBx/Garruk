using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MenuModel {
	
	public User player;
	public string[] buttonsLabels;
	public string[] mobileButtonsLabels;
	public bool isInvited;
	public string invitationError;
	
	private string URLGetUserData = ApplicationModel.host+"get_user_data.php";
	private string URLRefreshUserData = ApplicationModel.host+"refresh_user_data.php";
	
	public MenuModel()
	{
		this.player = new User ();
		this.buttonsLabels = new string[6];
		this.buttonsLabels [0] = WordingMenu.getReference(0);
		this.buttonsLabels [1] = WordingMenu.getReference(1);
		this.buttonsLabels [2] = WordingMenu.getReference(2);
		this.buttonsLabels [3] = WordingMenu.getReference(3);
		this.buttonsLabels [4] = WordingMenu.getReference(4);
		this.buttonsLabels [5] = WordingMenu.getReference(5);
		this.mobileButtonsLabels = new string[6];
		this.mobileButtonsLabels [0] = WordingMenu.getReference(6);
		this.mobileButtonsLabels [1] = WordingMenu.getReference(7);
		this.mobileButtonsLabels [2] = WordingMenu.getReference(8);
		this.mobileButtonsLabels [3] = WordingMenu.getReference(9);
		this.mobileButtonsLabels [4] = WordingMenu.getReference(10);
		this.mobileButtonsLabels [5] = WordingMenu.getReference(11);
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
		ApplicationModel.credits = player.Money;
		player.nonReadNotifications= System.Convert.ToInt32(array[1]);
		player.idProfilePicture = System.Convert.ToInt32(array [2]);
		return player;
	}
}

