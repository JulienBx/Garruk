using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class CardGameController : CardController
{

	public void setGameCard(Card c)
	{
		base.setCard (c);
		base.setExperience ();
		base.setSkills ();
		base.show ();
	}
	public void resetGameCard(Card c)
	{
		this.eraseCard ();
		this.setGameCard (c);
	}
	public override void eraseCard()
	{
		base.eraseCard ();
	}
	public override void updateExperience()
	{
		base.updateExperience ();
	}
	public override void resize()
	{
		base.resize();
	}
}

