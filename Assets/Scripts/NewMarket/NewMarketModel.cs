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
	public Cards cards;
	public Cards newCards;
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
		this.cards = new Cards();
		this.newCards = new Cards ();
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		form.AddField ("myform_totalnbresultlimit", totalNbResultLimit.ToString());
		
		WWW w = new WWW(URLGetMarketData, form);				// On envoie le formulaire à l'url sur le serveur 
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
			if(data[2]!="")
			{
				this.cards.parseCards(data[2]);
			}
			this.player=parsePlayer(data[3].Split(new string[] { "\\" }, System.StringSplitOptions.None));
			if(cards.getCount()>0)
			{
				this.newCardsTimeLimit = cards.getCard(0).OnSaleDate;
				this.oldCardsTimeLimit = cards.getCard(cards.getCount()-1).OnSaleDate;
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
		Cards newCards = new Cards ();
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
			if(data[1]!="")
			{
				newCards.parseCards(data[1]);
			}
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
			for(int i=0;i<this.cards.getCount();i++)
			{
				if(!cardsIds.Contains(this.cards.getCard(i).Id)&&this.cards.getCard(i).IdOWner!=-1)
				{
					cardsSold.Add (this.cards.getCard(i).Id);
					this.cards.getCard(i).onSale=0;
					this.cards.getCard(i).IdOWner=-1;
				}
				else if(cardsIds.Contains(this.cards.getCard(i).Id))
				{
					int idx = cardsIds.IndexOf(this.cards.getCard(i).Id);
					this.cards.getCard(i).Price=cardsPrices[idx];
				}
			}
			for(int i=0;i<newCards.getCount();i++)
			{
				bool existing=false;
				for(int j=0;j<this.cards.getCount();j++)
				{
					if(this.cards.getCard(j).Id==newCards.getCard(i).Id)
					{
						existing=true;
						break;
					}
				}
				if(!existing)
				{
					this.newCards.cards.Insert(i,newCards.getCard(i));
				}
			}
			if(this.newCards.getCount()>0)
			{
				this.newCardsTimeLimit = this.newCards.getCard(0).OnSaleDate;
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
}

