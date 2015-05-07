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
	public IList<PlayerResult> results;
	public string[] cardTypeList;
	public string[] skillsList;
	public Division currentDivision;
	public Cup currentCup;
	public User player;

	private string URLGetLobbyData = ApplicationModel.host + "get_lobby_data.php";

	public LobbyModel ()
	{
		this.decks = new List<Deck> ();
		this.results = new List<PlayerResult> ();
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
		
		WWW w = new WWW(URLGetLobbyData, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.cardTypeList = data[0].Split(new string[] { "//" }, System.StringSplitOptions.None);
			this.skillsList = data[1].Split(new string[] { "//" }, System.StringSplitOptions.None);
			this.decks = this.parseDecks(data[2].Split(new string[] { "#DECK#" }, System.StringSplitOptions.None));
			this.results = this.parseResults(data[3].Split(new string[] { "#RESULT#" }, System.StringSplitOptions.None));
			this.currentDivision = this.parseDivision(data[4].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.currentCup = this.parseCup(data[5].Split(new string[] { "//" }, System.StringSplitOptions.None));
			this.player = this.parsePlayer(data[6].Split(new string[] { "//" }, System.StringSplitOptions.None));
		}
	}
	private List<Deck> parseDecks(string[] decksData)
	{
		List<Deck> decks = new List<Deck> ();
		for(int i=0;i<decksData.Length-1;i++)
		{
			string[] deckInformation = decksData[i].Split(new string[] { "##" }, System.StringSplitOptions.None);
			decks.Add (new Deck());
			decks[i].Name=deckInformation[0];
			decks[i].Cards=new List<Card>();
			decks[i].Cards=this.parseCards(deckInformation[1].Split(new string[] { "#CARD#" }, System.StringSplitOptions.None));
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
			cardData = cardsData[i].Split(new string[] { "#S#" }, System.StringSplitOptions.None);
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
					cards[i].TitleClass=this.cardTypeList[System.Convert.ToInt32(cardInfo[7])];
					cards[i].LifeLevel=System.Convert.ToInt32(cardInfo[8]);
					cards[i].MoveLevel=System.Convert.ToInt32(cardInfo[9]);
					cards[i].SpeedLevel=System.Convert.ToInt32(cardInfo[10]);
					cards[i].AttackLevel=System.Convert.ToInt32(cardInfo[11]);
					cards[i].Experience=System.Convert.ToInt32(cardInfo[12]);
					cards[i].nbWin=System.Convert.ToInt32(cardInfo[13]);
					cards[i].nbLoose=System.Convert.ToInt32(cardInfo[14]);
					cards[i].Skills = new List<Skill>();
				}
				else
				{
					cards[i].Skills.Add(new Skill ());
					cards[i].Skills[j-1].Id=System.Convert.ToInt32(cardInfo[0]);
					cards[i].Skills[j-1].Name=this.skillsList[System.Convert.ToInt32(cardInfo[0])];
					cards[i].Skills[j-1].IsActivated=System.Convert.ToInt32(cardInfo[1]);
					cards[i].Skills[j-1].Level=System.Convert.ToInt32(cardInfo[2]);
					cards[i].Skills[j-1].Power=System.Convert.ToInt32(cardInfo[3]);
					cards[i].Skills[j-1].ManaCost=System.Convert.ToInt32(cardInfo[4]);
					cards[i].Skills[j-1].Description=cardInfo[5];
				}
			}
		}
		return cards;
	}
	private List<PlayerResult> parseResults(string[] resultsData)
	{
		List<PlayerResult> results = new List<PlayerResult> ();
		for(int i=0;i<resultsData.Length-1;i++)
		{
			string[] resultInformation = resultsData[i].Split(new string[] { "//" }, System.StringSplitOptions.None);
			results.Add (new PlayerResult());
			results[i].HasWon=System.Convert.ToBoolean(System.Convert.ToInt32(resultInformation[0]));
			results[i].Date=System.DateTime.ParseExact(resultInformation[1], "yyyy-MM-dd HH:mm:ss", null);
			results[i].GameType=System.Convert.ToInt32(resultInformation[2]);
			results[i].Opponent=new User();
			results[i].Opponent.Username=resultInformation[3];
			results[i].Opponent.Picture=resultInformation[4];
			results[i].Opponent.Division=System.Convert.ToInt32(resultInformation[4]);
			results[i].Opponent.RankingPoints=System.Convert.ToInt32(resultInformation[4]);
			results[i].Opponent.Ranking=System.Convert.ToInt32(resultInformation[4]);
			results[i].Opponent.TotalNbWins=System.Convert.ToInt32(resultInformation[4]);
			results[i].Opponent.TotalNbLooses=System.Convert.ToInt32(resultInformation[4]);
		}
		return results;
	}
	private Division parseDivision(string[] divisionData)
	{
		Division division = new Division ();
		division.Name = divisionData [0];
		division.Picture = divisionData [1];
		division.TitlePrize = System.Convert.ToInt32 (divisionData [2]);
		return division;
	}
	private Cup parseCup(string[] cupData)
	{
		Cup cup=new Cup();
		cup.Name = cupData [0];
		cup.Picture = cupData [1];
		cup.CupPrize = System.Convert.ToInt32 (cupData [2]);
		return cup;
	}
	private User parsePlayer(string[] userData)
	{
		User user = new User ();
		user.NbGamesDivision = System.Convert.ToInt32 (userData [0]);
		user.NbGamesCup = System.Convert.ToInt32 (userData [1]);
		return user;
	}
}

