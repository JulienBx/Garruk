using System;
using UnityEngine;

public class Gamecards
{
	GameObject[] cards ;

	public Gamecards ()
	{
		this.cards = new GameObject[8];
	}

	public CardC getGameCard(int i){
		return this.cards[i].GetComponent<CardC>();
	}
}

