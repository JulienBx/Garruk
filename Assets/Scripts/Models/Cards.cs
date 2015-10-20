using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Cards
{
	private string URLAddXpLevel = ApplicationModel.host + "add_xplevel_to_card.php"; 

	public List<Card> cards ;
	public string error ;

	public Cards()
	{
		this.cards = new List<Card>();
	}
	
	public IEnumerator getCards()
	{
		Debug.Log("blabla");
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_parameters", "3");
		WWW w = new WWW(ApplicationModel.host + "get_cards_deck.php", form);             				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		
		if (w.error != null)
		{
			this.error = w.error;
			Debug.Log("Erreur : "+w.error);
		} 
		else
		{
			Debug.Log("Succès : "+w.text);
			this.parseCards(w.text);
		}
	}
	
	public void parseCards(string s)
	{
		string[] cardsData;
		string[] cardData = null;
		string[] cardInfo = null;
		
		cardsData=s.Split(new string[] { "#CARD#" }, System.StringSplitOptions.None);
		for(int i=0;i<cardsData.Length-1;i++)
		{
			Debug.Log("CardsData "+i+" : "+cardsData[i]);
			cardData = cardsData[i].Split(new string[] { "#SKILL#" }, System.StringSplitOptions.None);
			for(int j = 0 ; j < cardData.Length-1 ; j++)
			{
				Debug.Log("CardData "+j+" : "+cardData[j]);
				
				cardInfo = cardData[j].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
				if (j==0)
				{
					this.cards.Add(new Card());
					this.cards[i-1].Id=System.Convert.ToInt32(cardInfo[0]);
					this.cards[i-1].Title=cardInfo[1];
					this.cards[i-1].Life=System.Convert.ToInt32(cardInfo[2]);
					this.cards[i-1].Attack=System.Convert.ToInt32(cardInfo[3]);
					this.cards[i-1].Speed=System.Convert.ToInt32(cardInfo[4]);
					this.cards[i-1].Move=System.Convert.ToInt32(cardInfo[5]);
					this.cards[i-1].IdOWner=System.Convert.ToInt32(cardInfo[6]);
					this.cards[i-1].UsernameOwner=cardInfo[7];
					this.cards[i-1].IdClass=System.Convert.ToInt32(cardInfo[8]);
					this.cards[i-1].PowerLevel=System.Convert.ToInt32(cardInfo[9]);
					this.cards[i-1].LifeLevel=System.Convert.ToInt32(cardInfo[10]);
					this.cards[i-1].AttackLevel=System.Convert.ToInt32(cardInfo[11]);
					this.cards[i-1].MoveLevel=System.Convert.ToInt32(cardInfo[12]);
					this.cards[i-1].SpeedLevel=System.Convert.ToInt32(cardInfo[13]);
					this.cards[i-1].Experience=System.Convert.ToInt32(cardInfo[14]);
					this.cards[i-1].ExperienceLevel=System.Convert.ToInt32(cardInfo[15]);
					this.cards[i-1].PercentageToNextLevel=System.Convert.ToInt32(cardInfo[16]);
					this.cards[i-1].NextLevelPrice=System.Convert.ToInt32(cardInfo[17]);
					this.cards[i-1].onSale=System.Convert.ToInt32(cardInfo[18]);
					this.cards[i-1].Price=System.Convert.ToInt32(cardInfo[19]);
					//this.cards[i].OnSaleDate=new DateTime(cardInfo[20]);
					this.cards[i-1].nbWin=System.Convert.ToInt32(cardInfo[21]);
					this.cards[i-1].nbLoose=System.Convert.ToInt32(cardInfo[22]);
					this.cards[i-1].destructionPrice=System.Convert.ToInt32(cardInfo[23]);
					this.cards[i-1].Power=System.Convert.ToInt32(cardInfo[24]);
				}
				else
				{
					this.cards[i-1].Skills.Add(new Skill ());
					this.cards[i-1].Skills[j-1].Name=cardInfo[1];
					this.cards[i-1].Skills[j-1].Id=System.Convert.ToInt32(cardInfo[0]);
					this.cards[i-1].Skills[j-1].cible=System.Convert.ToInt32(cardInfo[2]);
					this.cards[i-1].Skills[j-1].IsActivated=System.Convert.ToInt32(cardInfo[3]);
					this.cards[i-1].Skills[j-1].Level=System.Convert.ToInt32(cardInfo[4]);
					this.cards[i-1].Skills[j-1].Power=System.Convert.ToInt32(cardInfo[5]);
					this.cards[i-1].Skills[j-1].Description=cardInfo[6];
					this.cards[i-1].Skills[j-1].proba=System.Convert.ToInt32(cardInfo[7]);
					this.cards[i-1].Skills[j-1].nextDescription=cardInfo[8];
					this.cards[i-1].Skills[j-1].proba=System.Convert.ToInt32(cardInfo[9]);
					
					if (this.cards[i-1].Skills[j-1].Id==9){
						this.cards[i-1].Skills[j-1].nbLeft = 1 ;
					}
				}
			}
		}
	}
}