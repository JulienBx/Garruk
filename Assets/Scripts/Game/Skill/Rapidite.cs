using UnityEngine;
using System.Collections.Generic;

public class Rapidite : GameSkill
{
	public Rapidite()
	{

	}
	
	public override void launch()
	{
		Debug.Log("Je lance rapidite");
		GameController.instance.lookForTarget("Choisir une cible pour Rapidite", "Lancer Rapidite");
	}
	
	public override void resolve(int[] args)
	{
		if (args.Length != 1)
		{
			Debug.Log("Mauvais paramètres de résolution envoyés");
		} else
		{
			int targetID = args [0];
			GameController.instance.getCard(targetID).modifiers.Add(new StatModifier(GameController.instance.getCurrentSkill().Power, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, 1, false));
			GameController.instance.reloadDestinationTiles();
			GameController.instance.reloadCard(targetID);
		}
	}
}
