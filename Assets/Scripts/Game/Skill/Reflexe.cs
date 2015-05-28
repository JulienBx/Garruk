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
		GameController.instance.lookForTarget();
	}

	public override void resolve(int[] args)
	{
		if (args.Length != 1)
		{
			Debug.Log("Mauvais paramètres de résolution envoyés");
		} else
		{
			int targetID = args [0];
			GameController.instance.getCard(targetID).modifiers.Add(new StatModifier(GameController.instance.getCurrentSkill().Power, ModifierType.Type_BonusMalus, ModifierStat.Stat_Speed));
			GameController.instance.reloadSortedList();
		}
	}
}
