using UnityEngine;
using System.Collections.Generic;

public class Dissipation : GameSkill
{
	public Dissipation()
	{
	}
	
	public override void launch()
	{
		Debug.Log("Je lance dissipation");
		GameController.instance.lookForTarget("Choisir une cible pour Dissipation", "Lancer Dissipation");
	}

	public override void resolve(int[] args)
	{
		if (args.Length != 1)
		{
			Debug.Log("Mauvais paramètres de résolution envoyés");
		} else
		{
			if (Random.Range(0, 100) < GameController.instance.getCurrentSkill().Power)
			{
				int targetID = args [0];
				GameController.instance.getCard(targetID).clearBuffs();
				GameController.instance.reloadSortedList();
				GameController.instance.reloadDestinationTiles();
				GameController.instance.reloadCard(targetID);
			}
		}
	}
}
