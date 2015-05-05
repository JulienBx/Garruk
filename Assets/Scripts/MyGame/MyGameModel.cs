using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class MyGameModel
{

	public IList<Card> cards;
	public IList<Deck> decks;
	public string[] cardTypeList;
	public IList<string> skillsList;
	public int idSelectedDeck;

	private string URLGetMyGameData = ApplicationModel.host + "get_mygame_data.php"; 

	public MyGameModel()
	{
	}
	public IEnumerator initializeMyGame () 
	{
		
		this.skillsList = new List<string> ();
		this.cards = new List<Card>();
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_nick", ApplicationModel.username);
		
		WWW w = new WWW(URLGetMyGameData, form); 				// On envoie le formulaire à l'url sur le serveur 
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
			this.decks=parseDecks(data[3].Split(new string[] { "#D#" }, System.StringSplitOptions.None));
			this.idSelectedDeck=System.Convert.ToInt32(data[4]);
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
	public List<Deck> parseDecks(string[] decksIds)
	{
		string[] deckData = null;
		string[] deckInfo = null;

		List<Deck> decks = new List<Deck> (); 
		
		for (int i = 0; i < decksIds.Length - 1; i++) 		// On boucle sur les attributs d'un deck
		{
			deckData = decksIds[i].Split(new string[] { "#C#" }, System.StringSplitOptions.None);
			for(int j=0 ; j<deckData.Length-1;j++)
			{

				deckInfo=deckData[j].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
				if(j==0)
				{

					decks.Add(new Deck());
					decks[i].Id=System.Convert.ToInt32(deckInfo [0]);
					decks[i].Name=deckInfo [1];
					decks[i].NbCards=System.Convert.ToInt32(deckInfo [2]);

					decks[i].Cards = new List<Card>();
				}
				else
				{
					decks[i].Cards.Add(new Card ());
					decks[i].Cards[j-1].Id=System.Convert.ToInt32(deckInfo [0]);
				}
			}	                     
		}
		return decks;
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
					cards[i].onSale=System.Convert.ToInt32(cardInfo[12]);
					cards[i].Price=System.Convert.ToInt32(cardInfo[13]);
					cards[i].IdOWner=System.Convert.ToInt32(cardInfo[14]);
					cards[i].UsernameOwner=cardInfo[15];
					cards[i].Experience=System.Convert.ToInt32(cardInfo[16]);
					cards[i].nbWin=System.Convert.ToInt32(cardInfo[17]);
					cards[i].nbLoose=System.Convert.ToInt32(cardInfo[18]);

					
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



}

