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
	
	public IEnumerator getCards(string parameters)
	{
		string[] cardsData;
		string[] cardData = null;
		string[] cardInfo = null;
		
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_parameters", parameters);
		WWW w = new WWW(ApplicationModel.host + "getCards.php", form);             				// On envoie le formulaire à l'url sur le serveur 
		yield return w;
		
		if (w.error != null)
		{
			this.error = w.error;
		} 
		else
		{
			cardsData=w.text.Split(new string[] { "#CARD#" }, System.StringSplitOptions.None);
			for(int i=0;i<cardsData.Length-1;i++)
			{
				cardData = cardsData[i].Split(new string[] { "#SKILL#" }, System.StringSplitOptions.None);
				for(int j = 0 ; j < cardData.Length-1 ; j++)
				{
					cardInfo = cardData[j].Split(new string[] { "//" }, System.StringSplitOptions.None); 
					if (j==0)
					{
						this.cards.Add(new Card());
						this.cards[i].Id=System.Convert.ToInt32(cardInfo[0]);
						this.cards[i].Title=cardInfo[1];
						this.cards[i].Life=System.Convert.ToInt32(cardInfo[2]);
						this.cards[i].Attack=System.Convert.ToInt32(cardInfo[3]);
						this.cards[i].Speed=System.Convert.ToInt32(cardInfo[4]);
						this.cards[i].Move=System.Convert.ToInt32(cardInfo[5]);
						this.cards[i].IdOWner=System.Convert.ToInt32(cardInfo[6]);
						this.cards[i].UsernameOwner=cardInfo[7];
						this.cards[i].IdClass=System.Convert.ToInt32(cardInfo[8]);
						this.cards[i].PowerLevel=System.Convert.ToInt32(cardInfo[9]);
						this.cards[i].LifeLevel=System.Convert.ToInt32(cardInfo[10]);
						this.cards[i].AttackLevel=System.Convert.ToInt32(cardInfo[11]);
						this.cards[i].MoveLevel=System.Convert.ToInt32(cardInfo[12]);
						this.cards[i].SpeedLevel=System.Convert.ToInt32(cardInfo[13]);
						this.cards[i].Experience=System.Convert.ToInt32(cardInfo[14]);
						this.cards[i].ExperienceLevel=System.Convert.ToInt32(cardInfo[15]);
						this.cards[i].PercentageToNextLevel=System.Convert.ToInt32(cardInfo[16]);
						this.cards[i].NextLevelPrice=System.Convert.ToInt32(cardInfo[17]);
						this.cards[i].onSale=System.Convert.ToInt32(cardInfo[18]);
						this.cards[i].Price=System.Convert.ToInt32(cardInfo[19]);
						//this.cards[i].OnSaleDate=new DateTime(cardInfo[20]);
						this.cards[i].nbWin=System.Convert.ToInt32(cardInfo[21]);
						this.cards[i].nbLoose=System.Convert.ToInt32(cardInfo[22]);
						this.cards[i].destructionPrice=System.Convert.ToInt32(cardInfo[23]);
						this.cards[i].Power=System.Convert.ToInt32(cardInfo[24]);
					}
					else
					{
						this.cards[i].Skills.Add(new Skill ());
						this.cards[i].Skills[j-1].Name=cardInfo[1];
						this.cards[i].Skills[j-1].Id=System.Convert.ToInt32(cardInfo[1]);
						this.cards[i].Skills[j-1].cible=System.Convert.ToInt32(cardInfo[2]);
						this.cards[i].Skills[j-1].IsActivated=System.Convert.ToInt32(cardInfo[3]);
						this.cards[i].Skills[j-1].Level=System.Convert.ToInt32(cardInfo[4]);
						this.cards[i].Skills[j-1].Power=System.Convert.ToInt32(cardInfo[5]);
						this.cards[i].Skills[j-1].Description=cardInfo[6];
						this.cards[i].Skills[j-1].proba=System.Convert.ToInt32(cardInfo[7]);
						this.cards[i].Skills[j-1].nextDescription=cardInfo[8];
						this.cards[i].Skills[j-1].proba=System.Convert.ToInt32(cardInfo[9]);
						
						if (this.cards[i].Skills[j-1].Id==9){
							this.cards[i].Skills[j-1].nbLeft = 1 ;
						}
					}
				}
			}
		}
	}
}