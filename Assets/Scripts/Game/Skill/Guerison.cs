using UnityEngine;
using System.Collections.Generic;

public class Guerison : GameSkill
{
	public Guerison()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance Guerison");
		GameController.instance.lookForTarget("Choisir une cible pour Guerison", "Lancer Guerison");
	}
	
	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().Power;

		int targetID = args [0];
		GameController.instance.addCardModifier(-amount, targetID, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage, -1);

		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé guerison\n " 
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
