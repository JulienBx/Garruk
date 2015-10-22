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

	public Card getCard(int index)
	{
		return this.cards [index];
	}
	public int getCount()
	{
		return this.cards.Count;
	}
	
	public IEnumerator getCards()
	{
		Debug.Log("blabla");
		WWWForm form = new WWWForm(); 											// Création de la connexion
		form.AddField("myform_hash", ApplicationModel.hash); 					// hashcode de sécurité, doit etre identique à celui sur le serveur
		form.AddField("myform_parameters", "2");
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
		
		cardsData=s.Split(new string[] { "#CARD#" }, System.StringSplitOptions.None);
		for(int i=0;i<cardsData.Length;i++)
		{
			this.cards.Add(new Card());
			this.cards[i].parseCard(cardsData[i]);
		}
		
		Debug.Log("----------------");
		Debug.Log("Lif : "+this.cards[0].Life);
		Debug.Log("Next : "+this.cards[0].UpgradedLife);
		Debug.Log("Att : "+this.cards[0].Attack);
		Debug.Log("Next : "+this.cards[0].UpgradedAttack);
		Debug.Log("Speed : "+this.cards[0].Speed);
		Debug.Log("Next : "+this.cards[0].UpgradedSpeed);
		Debug.Log("----------------");
		Debug.Log("Lif : "+this.cards[1].Life);
		Debug.Log("Next : "+this.cards[1].UpgradedLife);
		Debug.Log("Att : "+this.cards[1].Attack);
		Debug.Log("Next : "+this.cards[1].UpgradedAttack);
		Debug.Log("Speed : "+this.cards[1].Speed);
		Debug.Log("Next : "+this.cards[1].UpgradedSpeed);
		Debug.Log("----------------");
		Debug.Log("Lif : "+this.cards[2].Life);
		Debug.Log("Next : "+this.cards[2].UpgradedLife);
		Debug.Log("Att : "+this.cards[2].Attack);
		Debug.Log("Next : "+this.cards[2].UpgradedAttack);
		Debug.Log("Speed : "+this.cards[2].Speed);
		Debug.Log("Next : "+this.cards[2].UpgradedSpeed);
		Debug.Log("----------------");
		Debug.Log("Lif : "+this.cards[3].Life);
		Debug.Log("Next : "+this.cards[3].UpgradedLife);
		Debug.Log("Att : "+this.cards[3].Attack);
		Debug.Log("Next : "+this.cards[3].UpgradedAttack);
		Debug.Log("Speed : "+this.cards[3].Speed);
		Debug.Log("Next : "+this.cards[3].UpgradedSpeed);
		
	}
}