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
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé apposition des mains \n " 
			+ convertStatToString(ModifierStat.Stat_Heal)
			+ " "
			+ amount 
			+ " " 
			+ convertStatToString(ModifierStat.Stat_Life));
		
		int targetID = args [0];
		GameController.instance.getCard(targetID).modifiers.Add(new StatModifier(-amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage));
		GameController.instance.reloadCard(targetID);
	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
