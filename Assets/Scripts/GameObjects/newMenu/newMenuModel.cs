using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class newMenuModel {

	public User player;
	public Cup currentCup;
	public Division currentDivision;
	public FriendlyGame currentFriendlyGame;
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
			this.currentDivision=parseDivision(data[1].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.currentCup=parseCup(data[2].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.currentFriendlyGame = this.parseFriendlyGame(data[3].Split(new string[] { "//" }, System.StringSplitOptions.None));
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
	private FriendlyGame parseFriendlyGame(string[] friendlyGameData)
	{
		FriendlyGame friendlyGame =new FriendlyGame();
		friendlyGame.EarnXp_W = System.Convert.ToInt32 (friendlyGameData [0]);
		friendlyGame.EarnXp_L = System.Convert.ToInt32 (friendlyGameData [1]);
		friendlyGame.EarnCredits_W = System.Convert.ToInt32 (friendlyGameData [2]);
		friendlyGame.EarnCredits_L = System.Convert.ToInt32 (friendlyGameData [3]);
		return friendlyGame;
	}
	private Cup parseCup(string[] array)
	{
		Cup cup = new Cup ();
		cup.Name= array[0];
		cup.Picture= array[1];
		cup.NbRounds = System.Convert.ToInt32(array [2]);
		cup.CupPrize = System.Convert.ToInt32(array [3]);
		return cup;
	}
	private Division parseDivision(string[] array)
	{
		Division division = new Division ();
		division.Name= array[0];
		division.Picture= array[1];
		division.NbGames = System.Convert.ToInt32(array [2]);
		division.TitlePrize = System.Convert.ToInt32(array [3]);
		return division;
	}
}

