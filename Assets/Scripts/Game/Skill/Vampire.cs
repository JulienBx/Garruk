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
		int amount = GameController.instance.getCurrentSkill().Power;

		int targetID = args [0];

		GameController.instance.addCardModifier(amount, targetID, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage, -1);
		GameController.instance.addCardModifier(-amount, GameController.instance.getCurrentPCC().IDCharacter, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage, -1);

		GameController.instance.addGameEvent(GameController.instance.getCurrentSkill().Action, GameController.instance.getPCC(targetID).card.Title);
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé Vampire\n " 
			+ " "
			+ amount 
			+ " " 
			+ convertStatToString(ModifierStat.Stat_Life)
			+ " d'absorbés");

	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}

