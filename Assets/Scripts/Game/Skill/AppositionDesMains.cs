using UnityEngine;
using System.Collections.Generic;

public class AppositionDesMains : GameSkill
{
	public AppositionDesMains()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance Apposition des mains");
		GameController.instance.lookForAdjacentTarget("Choisir une cible pour Apposition des mains", "Lancer Apposition des mains");
	}
	
	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().Power;

		
		int targetID = args [0];

		GameController.instance.addCardModifier(-amount, targetID, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage, -1);
		GameController.instance.addGameEvent(GameController.instance.getCurrentSkill().Action, GameController.instance.getPCC(targetID).card.Title);
		
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé apposition des mains \n " 
			+ convertStatToString(ModifierStat.Stat_Heal)
			+ " "
			+ amount 
			+ " " 
			+ convertStatToString(ModifierStat.Stat_Life));
	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
