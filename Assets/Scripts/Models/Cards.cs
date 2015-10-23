using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Cards
{
	public List<Card> cards ;
	
	public Cards()
	{
		this.cards = new List<Card>();
	}

	public virtual Card getCard(int index)
	{
		return this.cards [index];
	}
	
	public int getCount()
	{
		return this.cards.Count;
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
	}
}