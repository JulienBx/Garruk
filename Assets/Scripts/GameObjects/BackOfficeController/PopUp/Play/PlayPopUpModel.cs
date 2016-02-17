using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class PlayPopUpModel {
	
	public IList<Deck> decks;
	
	private string URLGetUserPlayingData = ApplicationModel.host+"get_user_playing_data.php";
	
	public PlayPopUpModel()
	{
	}
	public IEnumerator loadUserData()
	{
		WWWForm form = new WWWForm(); 								// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 		// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.player.Username); 	// Pseudo de l'utilisateur connecté
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
			this.parseUser(data[0].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.parseDivision(data[1].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.parseCup(data[2].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.parseFriendlyGame(data[3].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.decks = this.parseDecks(data[4].Split(new string[] { "#DECK#" }, System.StringSplitOptions.None));
		}
	}
	private void parseUser(string[] array)
	{
		ApplicationModel.player.SelectedDeckId = System.Convert.ToInt32(array [0]);
	}
	private void parseFriendlyGame(string[] friendlyGameData)
	{
		ApplicationModel.player.CurrentFriendlyGame = new FriendlyGame();
		ApplicationModel.player.CurrentFriendlyGame.EarnXp_W = System.Convert.ToInt32 (friendlyGameData [0]);
		ApplicationModel.player.CurrentFriendlyGame.EarnXp_L = System.Convert.ToInt32 (friendlyGameData [1]);
		ApplicationModel.player.CurrentFriendlyGame.EarnCredits_W = System.Convert.ToInt32 (friendlyGameData [2]);
		ApplicationModel.player.CurrentFriendlyGame.EarnCredits_L = System.Convert.ToInt32 (friendlyGameData [3]);
	}
	private void parseCup(string[] array)
	{
		ApplicationModel.player.CurrentCup=new Cup();
		ApplicationModel.player.CurrentCup.Name= "";
		ApplicationModel.player.CurrentCup.IdPicture= System.Convert.ToInt32(array[1]);
		ApplicationModel.player.CurrentCup.NbRounds = System.Convert.ToInt32(array [2]);
		ApplicationModel.player.CurrentCup.CupPrize = System.Convert.ToInt32(array [3]);
		ApplicationModel.player.CurrentCup.EarnXp_W = System.Convert.ToInt32 (array [4]);
		ApplicationModel.player.CurrentCup.EarnXp_L = System.Convert.ToInt32 (array [5]);
		ApplicationModel.player.CurrentCup.EarnCredits_W = System.Convert.ToInt32 (array [6]);
		ApplicationModel.player.CurrentCup.EarnCredits_L = System.Convert.ToInt32 (array [7]);
	}
	private void parseDivision(string[] array)
	{
		ApplicationModel.player.CurrentDivision = new Division();
		ApplicationModel.player.CurrentDivision.Name= WordingGameModes.getName(0,System.Convert.ToInt32(array[0])-1);
		ApplicationModel.player.CurrentDivision.IdPicture= System.Convert.ToInt32(array[1]);
		ApplicationModel.player.CurrentDivision.NbGames = System.Convert.ToInt32(array [2]);
		ApplicationModel.player.CurrentDivision.TitlePrize = System.Convert.ToInt32(array [3]);
		ApplicationModel.player.CurrentDivision.EarnXp_W = System.Convert.ToInt32 (array [4]);
		ApplicationModel.player.CurrentDivision.EarnXp_L = System.Convert.ToInt32 (array [5]);
		ApplicationModel.player.CurrentDivision.EarnCredits_W = System.Convert.ToInt32 (array [6]);
		ApplicationModel.player.CurrentDivision.EarnCredits_L = System.Convert.ToInt32 (array [7]);
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

