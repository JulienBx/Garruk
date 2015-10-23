using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;


public class LobbyModel
{
	public IList<Deck> decks;
	public string[] cardTypeList;
	public string[] skillsList;
	public Division currentDivision;
	public Cup currentCup;
	public User player;
	public FriendlyGame currentFriendlyGame;

	private string URLGetLobbyData = ApplicationModel.host + "get_lobby_data.php";

	public LobbyModel ()
	{
		this.decks = new List<Deck> ();
		this.currentDivision = new Division ();
		this.currentCup = new Cup ();
		this.player = new User ();
	}
	public IEnumerator getLobbyData(int totalNbResultLimit)
	{
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField ("myform_totalnbresultlimit", totalNbResultLimit.ToString());
		form.AddField ("myform_nbcardsbydeck", ApplicationModel.nbCardsByDeck.ToString ());
		
		WWW w = new WWW(URLGetLobbyData, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);

			this.player = this.parsePlayer(data[0].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.cardTypeList = data[1].Split(new string[] { "//" }, System.StringSplitOptions.None);
			this.skillsList = data[2].Split(new string[] { "//" }, System.StringSplitOptions.None);
			this.decks = this.parseDecks(data[3].Split(new string[] { "#DECK#" }, System.StringSplitOptions.None));
			if(this.decks.Count>0)
			{
				this.decks[0].cards=this.parseCards(data[4].Split(new string[] { "#CARD#" }, System.StringSplitOptions.None));
			}
			this.currentDivision = this.parseDivision(data[5].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.currentCup = this.parseCup(data[6].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.currentFriendlyGame = this.parseFriendlyGame(data[7].Split(new string[] { "//" }, System.StringSplitOptions.None));
			ApplicationModel.currentFriendlyGame=this.currentFriendlyGame;
		}
	}
	private User parsePlayer(string[] userData)
	{
		User user = new User ();
		user.NbGamesDivision = System.Convert.ToInt32 (userData [0]);
		user.NbGamesCup = System.Convert.ToInt32 (userData [1]);
		user.SelectedDeckId = System.Convert.ToInt32 (userData [2]);
		user.TutorialStep = System.Convert.ToInt32 (userData [3]);
		return user;
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
	private List<Card> parseCards(string[] cardsData)
	{
		List<Card> cards = new List<Card> ();
		string[] cardData = null;
		string[] cardInfo = null;

		for(int i=0;i<cardsData.Length-1;i++)
		{
			cardData = cardsData[i].Split(new string[] { "#SKILL#" }, System.StringSplitOptions.None);
			for(int j = 0 ; j < cardData.Length-1 ; j++)
			{
				cardInfo = cardData[j].Split(new string[] { "//" }, System.StringSplitOptions.None); 
				if (j==0)
				{
					cards.Add(new Card());
					cards[i].Id=System.Convert.ToInt32(cardInfo[0]);
					cards[i].Title=cardInfo[1];
					cards[i].Life=System.Convert.ToInt32(cardInfo[2]);
					cards[i].Attack=System.Convert.ToInt32(cardInfo[3]);
					cards[i].Speed=System.Convert.ToInt32(cardInfo[4]);
					cards[i].Move=System.Convert.ToInt32(cardInfo[5]);
					cards[i].ArtIndex=System.Convert.ToInt32(cardInfo[6]);
					cards[i].IdClass=System.Convert.ToInt32(cardInfo[7]);
					cards[i].TitleClass=cardInfo[8];
					cards[i].LifeLevel=System.Convert.ToInt32(cardInfo[9]);
					cards[i].MoveLevel=System.Convert.ToInt32(cardInfo[10]);
					cards[i].SpeedLevel=System.Convert.ToInt32(cardInfo[11]);
					cards[i].AttackLevel=System.Convert.ToInt32(cardInfo[12]);
					cards[i].Experience=System.Convert.ToInt32(cardInfo[13]);
					cards[i].nbWin=System.Convert.ToInt32(cardInfo[14]);
					cards[i].nbLoose=System.Convert.ToInt32(cardInfo[15]);
					cards[i].ExperienceLevel=System.Convert.ToInt32(cardInfo[16]);
					cards[i].PercentageToNextLevel=System.Convert.ToInt32(cardInfo[17]);
					cards[i].NextLevelPrice=System.Convert.ToInt32(cardInfo[18]);
					cards[i].deckOrder=System.Convert.ToInt32(cardInfo[19]);
					cards[i].destructionPrice=System.Convert.ToInt32(cardInfo[20]);
					cards[i].Power=System.Convert.ToInt32(cardInfo[21]);
					cards[i].Skills = new List<Skill>();
				}
				else
				{
					cards[i].Skills.Add(new Skill ());
					cards[i].Skills[j-1].Name=cardInfo[0];
					cards[i].Skills[j-1].Id=System.Convert.ToInt32(cardInfo[1]);
					cards[i].Skills[j-1].IsActivated=System.Convert.ToInt32(cardInfo[2]);
					cards[i].Skills[j-1].Level=System.Convert.ToInt32(cardInfo[3]);
					cards[i].Skills[j-1].Power=System.Convert.ToInt32(cardInfo[4]);
					cards[i].Skills[j-1].ManaCost=System.Convert.ToInt32(cardInfo[5]);
					cards[i].Skills[j-1].Description=cardInfo[6];
					cards[i].Skills[j-1].Action=cardInfo[7];
				}
			}
		}
		return cards;
	}
	private Division parseDivision(string[] divisionData)
	{
		Division division = new Division ();
		division.Name = divisionData [0];
		division.Picture = divisionData [1];
		division.TitlePrize = System.Convert.ToInt32 (divisionData [2]);
		division.NbGames = System.Convert.ToInt32 (divisionData [3]);
		return division;
	}
	private Cup parseCup(string[] cupData)
	{
		Cup cup=new Cup();
		cup.Name = cupData [0];
		cup.Picture = cupData [1];
		cup.CupPrize = System.Convert.ToInt32 (cupData [2]);
		cup.NbRounds = System.Convert.ToInt32 (cupData [3]);
		return cup;
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
}

