using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class NewMarketModel
{
	
	public string[] cardTypeList;
	public IList<string> skillsList;
	public IList<int> cardsSold;
	public IList<Card> cards;
	public IList<Card> newCards;
	public User player;
	private string URLGetMarketData = ApplicationModel.host + "get_market_data.php";
	private string URLRefreshMarket = ApplicationModel.host + "refresh_market.php";
	private DateTime newCardsTimeLimit;
	private DateTime oldCardsTimeLimit;
	
	public NewMarketModel()
	{
		this.newCardsTimeLimit = new DateTime (1900,1,1,1,1,1);
		this.oldCardsTimeLimit = new DateTime (1900,1,1,1,1,1);
	}
	public IEnumerator initializeMarket (int totalNbResultLimit) 
	{
		
		this.skillsList = new List<string> ();
		this.cards = new List<Card>();
		this.newCards = new List<Card> ();
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField ("myform_totalnbresultlimit", totalNbResultLimit.ToString());
		
		WWW w = new WWW(URLGetMarketData, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			this.cardTypeList = data[0].Split(new string[] { "\\" }, System.StringSplitOptions.None);
			this.skillsList=parseSkills(data[1].Split(new string[] { "#SK#" }, System.StringSplitOptions.None));
			this.cards=parseCards(data[2].Split(new string[] { "#C#" }, System.StringSplitOptions.None));
			this.player=parsePlayer(data[3].Split(new string[] { "\\" }, System.StringSplitOptions.None));
			if(cards.Count>0)
			{
				this.newCardsTimeLimit = cards[0].OnSaleDate;
				this.oldCardsTimeLimit = cards[cards.Count-1].OnSaleDate;
			}
		}
	}
	public User parsePlayer(string[] array)
	{
		User player = new User ();
		player.Id = System.Convert.ToInt32 (array [0]);
		player.MarketTutorial = System.Convert.ToBoolean (System.Convert.ToInt32 (array [1]));
		return player;
	}
	public IEnumerator refreshMarket (int totalNbResultLimit)
	{	
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField ("myform_totalnbresultlimit", totalNbResultLimit.ToString());
		form.AddField("myform_newcardlimit",  this.newCardsTimeLimit.ToString("yyyy-MM-dd HH:mm:ss"));
		form.AddField("myform_oldcardlimit",  this.oldCardsTimeLimit.ToString("yyyy-MM-dd HH:mm:ss"));
		
		WWW w = new WWW(URLRefreshMarket, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			string[] data=w.text.Split(new string[] { "END" }, System.StringSplitOptions.None);
			IList<Card> newCards = parseCards(data[1].Split(new string[] { "#C#" }, System.StringSplitOptions.None));

			String[] cardsInfos=data[0].Split(new string[] { "#C#" }, System.StringSplitOptions.None);
			List<int> cardsIds = new List<int>();
			List<int> cardsPrices = new List<int>();

			for(int i = 0 ; i < cardsInfos.Length-1 ; i++)
			{
				string[] cardInfo = cardsInfos[i].Split(new string[] { "\\" }, System.StringSplitOptions.None);
				cardsIds.Add(System.Convert.ToInt32(cardInfo[0]));
				cardsPrices.Add(System.Convert.ToInt32(cardInfo[1]));
			}
			
			cardsSold=new List<int>();
			for(int i=0;i<this.cards.Count;i++)
			{
				if(!cardsIds.Contains(this.cards[i].Id)&&this.cards[i].IdOWner!=-1)
				{
					cardsSold.Add (this.cards[i].Id);
					this.cards[i].onSale=0;
					this.cards[i].IdOWner=-1;
				}
				else if(cardsIds.Contains(this.cards[i].Id))
				{
					int idx = cardsIds.IndexOf(this.cards[i].Id);
					this.cards[i].Price=cardsPrices[idx];
				}
			}
			for(int i=0;i<newCards.Count;i++)
			{
				this.newCards.Insert(i,newCards[i]);
			}
			if(this.newCards.Count>0)
			{
				this.newCardsTimeLimit = this.newCards[0].OnSaleDate;
			}
		}
	}
	private List<string> parseSkills(string[] skillsIds)
	{
		List<string> skillsList = new List<string>();
		for(int i = 0 ; i < skillsIds.Length-1 ; i++)
		{
			string [] tempString = skillsIds[i].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
			skillsList.Add (tempString[0]);
		}
		return skillsList;
	}
	private List<Card> parseCards(string[] cardsIDS) 
	{
		string[] cardData = null;
		string[] cardInfo = null;
		
		List<Card> cards = new List<Card>();
		
		for(int i = 0 ; i < cardsIDS.Length-1 ; i++)
		{
			cardData = cardsIDS[i].Split(new string[] { "#S#" }, System.StringSplitOptions.None);
			for(int j = 0 ; j < cardData.Length-1 ; j++)
			{
				cardInfo = cardData[j].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
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
					cards[i].Price=System.Convert.ToInt32(cardInfo[12]);
					cards[i].OnSaleDate=DateTime.ParseExact(cardInfo[13], "yyyy-MM-dd HH:mm:ss", null);
					cards[i].IdOWner=System.Convert.ToInt32(cardInfo[14]);
					cards[i].Experience=System.Convert.ToInt32(cardInfo[15]);
					cards[i].nbWin=System.Convert.ToInt32(cardInfo[16]);
					cards[i].nbLoose=System.Convert.ToInt32(cardInfo[17]);
					cards[i].ExperienceLevel=System.Convert.ToInt32(cardInfo[18]);
					cards[i].PercentageToNextLevel=System.Convert.ToInt32(cardInfo[19]);
					cards[i].Power=System.Convert.ToInt32(cardInfo[20]);
					cards[i].PowerLevel=System.Convert.ToInt32(cardInfo[21]);
					cards[i].onSale=1;
					
					cards[i].Skills = new List<Skill>();
				}
				else
				{
					cards[i].Skills.Add(new Skill ());
					cards[i].Skills[j-1].Id=System.Convert.ToInt32(cardInfo[0]);
					cards[i].Skills[j-1].Name=cardInfo[1];
					cards[i].Skills[j-1].IsActivated=System.Convert.ToInt32(cardInfo[2]);
					cards[i].Skills[j-1].Level=System.Convert.ToInt32(cardInfo[3]);
					cards[i].Skills[j-1].Power=System.Convert.ToInt32(cardInfo[4]);
					cards[i].Skills[j-1].ManaCost=System.Convert.ToInt32(cardInfo[5]);
					cards[i].Skills[j-1].Description=cardInfo[6];
				}
			}
		}
		return cards;
	}
}

