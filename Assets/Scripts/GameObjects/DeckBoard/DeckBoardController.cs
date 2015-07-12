using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class DeckBoardController : MonoBehaviour 
{

	public void changeCardsColor(Color color)
	{
		for (int i=0;i<4;i++)
		{
			this.gameObject.transform.FindChild ("Card"+i).GetComponent<SpriteRenderer>().color=color;
		}
		this.gameObject.transform.FindChild("3stars").GetComponent<SpriteRenderer>().color=color;
		this.gameObject.transform.FindChild("2stars").GetComponent<SpriteRenderer>().color=color;
		this.gameObject.transform.FindChild("1star").GetComponent<SpriteRenderer>().color=color;
	}
}

