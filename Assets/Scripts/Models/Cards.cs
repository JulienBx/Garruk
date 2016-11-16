using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] 
public class Cards{

	public List<Card> cards ;
	public string String;
	
	public Cards()
	{
		this.cards = new List<Card>();
	}

	public virtual Card getCard(int index)
	{
		return this.cards [index];
	}

	public int getCardIndex(int id)
	{
		for(int i=0;i<this.cards.Count;i++)
		{
			if (this.cards [i].Id == id) 
			{
				return i;
			}
		}
		return -1;
	}

	public virtual CardM getCardM(int index)
	{
		return new CardM(this.cards [index]);
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
	public void update(Card card)
	{
		bool exists = false;
		for (int i = 0; i < this.cards.Count; i++) 
		{
			if (this.cards [i].Id == card.Id) 
			{
				this.cards [i] = card;
				exists = true;
				break;
			}
		}
		if (!exists) 
		{
			this.cards.Insert(0,card);
		}
	}
	public void setString()
	{
		this.String="";

		for (int i = 0; i < this.cards.Count; i++) 
		{
			this.cards [i].setString ();
			this.String += this.cards [i].String + "DATACARD";
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