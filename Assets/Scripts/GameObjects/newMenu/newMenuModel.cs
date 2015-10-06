using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class newMenuModel {

	public User player;
	public string[] buttonsLabels;
	public bool isInvited;
	public Invitation invitation;

	private string URLGetUserData = ApplicationModel.host+"get_user_data.php";
	private string URLRefreshUserData = ApplicationModel.host+"refresh_user_data.php";
	
	public newMenuModel()
	{
		this.player = new User ();
		this.buttonsLabels = new string[6];
		this.buttonsLabels [0] = "Accueil";
		this.buttonsLabels [1] = "Mon armée";
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
	public IEnumerator refreshUserData(int totalNbResultLimit)
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField("myform_limit", totalNbResultLimit); 
		
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
			if(data[1]!="-1")
			{
				this.isInvited=true;
				this.invitation = parseInvitation(data[1].Split(new string[] { "//" }, System.StringSplitOptions.None));
				//Debug.Log(this.invitation.SendingUser.Username+" vous a invité");
			}
			else
			{
				this.isInvited=false;
			}
			//Debug.Log(this.player.nonReadNotifications);
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
	private Invitation parseInvitation(string[] array)
	{
		Invitation invitation = new Invitation ();
		invitation.Id = System.Convert.ToInt32 (array [0]);
		invitation.InvitedUser = this.player;
		invitation.SendingUser=new User();
		invitation.SendingUser.Username = array [1];
		invitation.SendingUser.ThumbPicture = array [2];
		invitation.SendingUser.CollectionRanking = System.Convert.ToInt32 (array [3]);
		invitation.SendingUser.RankingPoints = System.Convert.ToInt32 (array [4]);
		invitation.SendingUser.Ranking = System.Convert.ToInt32 (array [5]);
		invitation.SendingUser.TotalNbWins = System.Convert.ToInt32 (array [6]);
		invitation.SendingUser.TotalNbLooses = System.Convert.ToInt32 (array [7]);
		return invitation;
	}
}

