using UnityEngine;
using System.Collections.Generic;

public class Lenteur : GameSkill
{
	public Lenteur()
	{

	}
	
	public override void launch()
	{
		Debug.Log("Je lance Lenteur");
		GameController.instance.lookForTarget("Choisir une cible pour Lenteur", "Lancer Lenteur");
	}
	
	public override void resolve(int[] args)
	{
		if (args.Length != 1)
		{
			Debug.Log("Mauvais paramètres de résolution envoyés");
		} else
		{
			int targetID = args [0];
			GameController.instance.getCard(targetID).modifiers.Add(new StatModifier(GameController.instance.getCurrentSkill().Power * -1, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, 1, false));
			GameController.instance.reloadDestinationTiles();
			GameController.instance.reloadCard(targetID);
		}
	}
}
