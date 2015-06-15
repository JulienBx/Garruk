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
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé guerison\n " 
			+ convertStatToString(ModifierStat.Stat_Heal)
			+ " "
			+ amount 
			+ " " 
			+ convertStatToString(ModifierStat.Stat_Life));
		
		int targetID = args [0];
		GameController.instance.getCard(targetID).modifiers.Add(new StatModifier(-amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage,-1,"","",""));
		GameController.instance.reloadCard(targetID);
	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
