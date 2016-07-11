using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class NewMarketModel
{
	public IList<int> cardsSold;
	public Cards cardsOnSale;
	public Cards newCards;
	private string URLGetCardsOnSale = ApplicationModel.host + "market_getCardsOnSale.php";
	//private string URLGetMarketData = ApplicationModel.host + "get_market_data.php";
	private string URLRefreshMarket = ApplicationModel.host + "refresh_market.php";
	private string URLRefreshMyGame = ApplicationModel.host + "refresh_mygame.php";
	private DateTime newCardsTimeLimit;
	private DateTime oldCardsTimeLimit;
	
	public NewMarketModel()
	{
		this.newCardsTimeLimit = new DateTime (1900,1,1,1,1,1);
		this.oldCardsTimeLimit = new DateTime (1900,1,1,1,1,1);
		this.newCards=new Cards();
	}
	public IEnumerator initializeMarket (int totalNbResultLimit) 
	{
		this.cardsOnSale=new Cards();
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.player.Username);
		form.AddField ("myform_totalnbresultlimit", totalNbResultLimit.ToString());

		ServerController.instance.setRequest(URLGetCardsOnSale, form);
		yield return ServerController.instance.StartCoroutine("executeRequest");

		if(ServerController.instance.getError()=="")
		{
			string result = ServerController.instance.getResult();
			if(result!="")
			{
				this.cardsOnSale.parseCards(result);
				this.newCardsTimeLimit = cardsOnSale.getCard(0).OnSaleDate;
				this.oldCardsTimeLimit = cardsOnSale.getCard(cardsOnSale.getCount()-1).OnSaleDate;
			}
		}
		else
		{
			Debug.Log(ServerController.instance.getError());
			ServerController.instance.lostConnection();
		}
	}
	public IEnumerator refreshMarket (int totalNbResultLimit)
	{	
		Cards newCards = new Cards ();
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.player.Username);
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
			for(int i=0;i<this.cardsOnSale.getCount();i++)
			{
				if(!cardsIds.Contains(this.cardsOnSale.getCard(i).Id)&&this.cardsOnSale.getCard(i).IdOWner!=-1)
				{
					cardsSold.Add (this.cardsOnSale.getCard(i).Id);
					this.cardsOnSale.getCard(i).onSale=0;
					this.cardsOnSale.getCard(i).IdOWner=-1;
				}
				else if(cardsIds.Contains(this.cardsOnSale.getCard(i).Id))
				{
					int idx = cardsIds.IndexOf(this.cardsOnSale.getCard(i).Id);
					this.cardsOnSale.getCard(i).Price=cardsPrices[idx];
				}
			}
			for(int i=0;i<newCards.getCount();i++)
			{
				bool existing=false;
				for(int j=0;j<this.cardsOnSale.getCount();j++)
				{
					if(this.cardsOnSale.getCard(j).Id==newCards.getCard(i).Id)
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
	public IEnumerator refreshMyGame()
	{
		this.cardsSold = new List<int> ();

		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.player.Username);
		
		WWW w = new WWW(URLRefreshMyGame, form); 				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		if (w.error != null) 
		{
			Debug.Log (w.error); 										// donne l'erreur eventuelle
		} 
		else 
		{
			int id;
			bool stillExists;
			string[] data=w.text.Split(new string[] { "#ID#" }, System.StringSplitOptions.None);
			for(int i=0;i<ApplicationModel.player.MyCardsOnMarket.getCount();i++)
			{
				id = ApplicationModel.player.MyCardsOnMarket.getCard(i).Id;
				stillExists=false;
				for(int j =0;j<data.Length-1;j++)
				{
					if(id==System.Convert.ToInt32(data[j]))
					{
						stillExists=true;
						break;
					}
				}
				if(!stillExists)
				{
					this.cardsSold.Add (id);
					ApplicationModel.player.MyCardsOnMarket.remove(i);
				}
			}
		}
	}
}