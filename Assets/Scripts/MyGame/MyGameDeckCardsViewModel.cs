using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class MyGameDeckCardsViewModel
{

	public List<int> deckCardsToBeDisplayed;
	public int nbCardsToDisplay;

	public MyGameDeckCardsViewModel ()
	{
		this.deckCardsToBeDisplayed = new List<int> ();
	}
}

