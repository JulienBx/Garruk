using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class PlayPopUpModel {
	
	public User player;
	public Cup currentCup;
	public Division currentDivision;
	public FriendlyGame currentFriendlyGame;
	public IList<Deck> decks;
	
	private string URLGetUserPlayingData = ApplicationModel.host+"get_user_playing_data.php";
	
	public PlayPopUpModel()
	{
		this.player = new User ();
	}
	
	public IEnumerator loadUserData(){
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username); 	// Pseudo de l'utilisateur connecté
		form.AddField ("myform_nbcardsbydeck", ApplicationModel.nbCardsByDeck.ToString ());
		
		WWW w = new WWW(URLGetUserPlayingData, form); 								// On envoie le formulaire à l'url sur le serveur 
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
			this.decks = this.parseDecks(data[4].Split(new string[] { "#DECK#" }, System.StringSplitOptions.None));
		}
	}
	private User parseUser(string[] array)
	{
		User player = new User ();
		player.SelectedDeckId = System.Convert.ToInt32(array [0]);
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
	private List<Deck> parseDecks(string[] decksData)
	{
		List<Deck> decks = new List<Deck> ();
		for(int i=0;i<decksData.Length-1;i++)
		{
			string[] deckInformation = decksData[i].Split(new string[] { "#DECKINFO#" }, System.StringSplitOptions.None);
			decks.Add (new Deck());
			decks[i].Id=System.Convert.ToInt32(deckInformation[0]);
			decks[i].Name=deckInformation[1];
			decks[i].NbCards=System.Convert.ToInt32(deckInformation[2]);
		}
		return decks;
	}
}

