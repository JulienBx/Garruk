using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable] 
public class Decks{

	public List<Deck> decks ;
	public string String;
	
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
	public void update(Deck deck)
	{
		bool exists = false;
		for (int i = 0; i < this.decks.Count; i++) 
		{
			if (this.decks [i].Id == deck.Id) 
			{
				this.decks [i] = deck;
				exists = true;
				break;
			}
		}
		if (!exists) 
		{
			this.decks.Insert(0,deck);
		}
	}
	public void remove(int index)
	{
		this.decks.RemoveAt(index);
	}
	public void setString()
	{
		this.String="";

		for (int i = 0; i < this.decks.Count; i++) 
		{
			this.decks [i].setString ();
			this.String += this.decks [i].String + "DATADECK";
		}
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
			if (this.decks [i].Id == ApplicationModel.player.SelectedDeckId) {
				ApplicationModel.player.SelectedDeckIndex = i;
			}
		}
	}
}