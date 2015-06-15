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
		GameController.instance.lookForDeadTarget();
		GameController.instance.lookForEmptyTileTarget("Choisir une cible et un emplacement pour Resurrection", "Lancer Resurrection", HaloSkill.DeadHalo);
	}
	
	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé Resurrection\n ");
		
		int targetID = args [0];
		int targetX = args [1];
		int targetY = args [2];
		GameController.instance.getPCC(targetID).GetComponent<PlayingCardController>().card.Life = 1;
		GameController.instance.getPCC(targetID).GetComponent<PlayingCardController>().isDead = false;
	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
