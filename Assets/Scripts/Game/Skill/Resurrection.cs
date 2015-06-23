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
		//GameController.instance.lookForEmptyAdjacentTile("Choisir un emplacement pour Resurrection", "Lancer Resurrection", HaloSkill.DeadHalo);
	}
	
	public override void resolve(List<int> targetsPCC)
	{

//		if (args [2] == -1)
//		{
//			GameController.instance.clearDeads();
//			nextStep();
//		} else
//		{
//			int targetID = args [0];
//			int amount = GameController.instance.getCurrentSkill().ManaCost;
//			if (Random.Range(0, 100) < amount)
//			{
//
//				int targetX = args [1];
//				int targetY = args [2];
//				
//				GameController.instance.relive(targetID, targetX, targetY);
//
//				GameController.instance.addGameEvent(GameController.instance.getCurrentSkill().Action, GameController.instance.getPCC(targetID).card.Title);
//
//				GameController.instance.play(GameController.instance.getCurrentCard().Title + 
//					" a lancé Resurrection\n ");
//			} else
//			{
//				GameController.instance.addGameEvent(GameController.instance.getCurrentSkill().Action, GameController.instance.getPCC(targetID).card.Title + " mais a échoué");
//				GameController.instance.play(GameController.instance.getCurrentCard().Title + 
//					" a lancé Resurrection\n mais a échoué");
//			}
//			
//
//		}

	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
