using UnityEngine;
using System.Collections.Generic;

public class Vampire : GameSkill
{
	public Vampire()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance Vampire");
		GameController.instance.lookForAdjacentTarget("Choisir une cible pour Vampire", "Lancer Vampire");
	}
	
	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().Power * -1;
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé Vampire\n " 
			+ " "
			+ amount 
			+ " " 
			+ convertStatToString(ModifierStat.Stat_Life)
			+ " d'absorbés");
		
		int targetID = args [0];
		GameController.instance.getCard(targetID).modifiers.Add(new StatModifier(amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage));
		GameController.instance.getCurrentPCC().card.modifiers.Add(new StatModifier(-amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage));
		GameController.instance.reloadCard(targetID);
		GameController.instance.reloadCard(GameController.instance.getCurrentPCC().IDCharacter);
	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}

