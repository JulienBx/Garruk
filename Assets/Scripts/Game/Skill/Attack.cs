using UnityEngine;
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
		if (args.Length != 1)
		{
			Debug.Log("Mauvais paramètres de résolution envoyés");
		} else
		{
			int targetID = args [0];
			GameController.instance.getCard(targetID).modifiers.Add(new StatModifier(GameController.instance.getCurrentCard().Attack, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage));
			GameController.instance.reloadCard(targetID);
			GameController.instance.reloadSelectedPlayingCard(targetID);
		}
	}
}
