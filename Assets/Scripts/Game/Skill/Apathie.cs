using UnityEngine;
using System.Collections.Generic;

public class Apathie : GameSkill
{
	public Apathie()
	{

	}
	
	public override void launch()
	{
		Debug.Log("Je lance apathie");
		GameController.instance.lookForTarget("Choisir une cible pour Apathie", "Lancer Apathie");
	}
	 
	public override void resolve(int[] args)
	{
		if (args.Length != 1)
		{
			Debug.Log("Mauvais paramètres de résolution envoyés");
		} else
		{
			int targetID = args [0];
			GameController.instance.getCard(targetID).modifiers.Add(new StatModifier(GameController.instance.getCurrentSkill().Power * -1, ModifierType.Type_BonusMalus, ModifierStat.Stat_Speed));
			GameController.instance.reloadSortedList();
		}
	}
}
