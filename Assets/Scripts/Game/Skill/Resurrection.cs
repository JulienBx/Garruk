using UnityEngine;
using System.Collections.Generic;

public class Resurrection : GameSkill
{
	public Resurrection()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance Resurrection");
		GameController.instance.lookForDeadTarget("Choisir une cible pour Resurrection", "Lancer Resurrection");
	}

	public void nextStep()
	{
		GameController.instance.lookForEmptyAdjacentTile("Choisir un emplacement pour Resurrection", "Lancer Resurrection", HaloSkill.DeadHalo);
	}
	
	public override void resolve(int[] args)
	{
		if (args [2] == -1)
		{
			GameController.instance.clearDeads();
			nextStep();
		} else
		{
			int amount = GameController.instance.getCurrentSkill().ManaCost;

			
			int targetID = args [0];
			int targetX = args [1];
			int targetY = args [2];

			GameController.instance.relive(targetID, targetX, targetY);

			GameController.instance.play(GameController.instance.getCurrentCard().Title + 
				" a lancé Resurrection\n ");
		}
	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
