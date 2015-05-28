﻿using UnityEngine;
using System.Collections.Generic;

public class Attack : GameSkill
{
	public Attack()
	{
	
	}
	
	public override void launch()
	{
		Debug.Log("Je lance attack");
		GameController.instance.lookForAdjacentTarget("Choisir une cible à attaquer", "Lancer attaque");
	}
	
	public override void resolve(int[] args)
	{
		int targetID = args [0];
		GameController.instance.getCard(targetID).modifiers.Add(new StatModifier(GameController.instance.getCurrentCard().Attack, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage));
		GameController.instance.play(GameController.instance.getCurrentCard().Title + " a lancé attack");

		if (GameController.instance.getCard(targetID).GetLife()<=0){
			GameController.instance.getPCC(targetID).kill();
			GameController.instance.reloadTimeline();
		}
		GameController.instance.reloadCard(targetID);
		GameController.instance.reloadSelectedPlayingCard(targetID);

	}
}
