using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Cards{

	public List<Card> cards ;
	
	public Cards()
	{
		this.cards = new List<Card>();
	}

	public virtual Card getCard(int index)
	{
		return this.cards [index];
	}

	public virtual CardM getCardM(int index)
	{
		return new CardM(this.cards [index]);
	}
	
	public virtual GameCard getGameCard(int index)
	{
		return new GameCard(this.cards [index]);
	}

	public void add()
	{
		this.cards.Add(new Card());
	}
    public void addCards(Cards cards)
    {
        for(int i=0; i<cards.getCount();i++)
        {
            this.cards.Insert(0,cards.cards[cards.getCount()-i-1]);
        }
    }
	public void remove(int index)
	{
		this.cards.RemoveAt(index);
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