using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Decks{

	public List<Deck> decks ;
	
	public Decks()
	{
		this.decks = new List<Deck>();
	}

	public Deck getDeck(int index)
	{
		return this.decks [index];
	}

	public int getCount()
	{
		return this.decks.Count;
	}
	public void add()
	{
		this.decks.Add(new Deck());
	}
	public void remove(int index)
	{
		this.decks.RemoveAt(index);
	}
	public void parseDecks(string s)
	{
		string[] decksIds;
		decksIds = s.Split(new string[] { "#D#" }, System.StringSplitOptions.None);

		string[] deckData = null;
		string[] deckInfo = null;
		
		for (int i = 0; i < decksIds.Length - 1; i++) 		// On boucle sur les attributs d'un deck
		{
			deckData = decksIds[i].Split(new string[] { "#C#" }, System.StringSplitOptions.None);
			for(int j=0 ; j<deckData.Length-1;j++)
			{
				
				deckInfo=deckData[j].Split(new string[] { "\\" }, System.StringSplitOptions.None); 
				if(j==0)
				{
					
					this.decks.Add(new Deck());
					this.decks[i].Id=System.Convert.ToInt32(deckInfo [0]);
					this.decks[i].Name=deckInfo [1];
					this.decks[i].NbCards=System.Convert.ToInt32(deckInfo [2]);
					this.decks[i].cards = new List<Card>();
				}
				else
				{
					this.decks[i].cards.Add(new Card ());
					this.decks[i].cards[j-1].Id=System.Convert.ToInt32(deckInfo [0]);
					this.decks[i].cards[j-1].deckOrder=System.Convert.ToInt32(deckInfo[1]);
				}
			}	                     
		}
	}
}