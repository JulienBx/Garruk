using UnityEngine;
using System.Collections.Generic;

public class Reflexe : GameSkill
{
	public Reflexe()
	{

	}

	public override void launch()
	{
		Debug.Log("Je lance réflexe");
		GameController.instance.lookForTarget("Choisir une cible pour Reflexe", "Lancer Reflexe");
	}

	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().Power;
	
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé reflexe \n +" 
			+ amount
			+ " " 
			+ convertStatToString(ModifierStat.Stat_Speed));

		int targetID = args [0];
		GameController.instance.getCard(targetID).modifiers.Add(new StatModifier(amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Speed));
		GameController.instance.reloadSortedList();

	}
}
