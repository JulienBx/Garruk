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
		//GameController.instance.lookForDeadTarget("Choisir une cible pour Resurrection", "Lancer Resurrection");
	}
	
	public override void resolve(List<int> targetsPCC)
	{
		int amount = GameController.instance.getCurrentSkill().Power;
		GameController.instance.play();
		
//		int targetID = args [0];
	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
