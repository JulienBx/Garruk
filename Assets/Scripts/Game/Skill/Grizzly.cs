using UnityEngine;
using System.Collections.Generic;

public class Grizzly : GameSkill
{
	public Grizzly()
	{
		
	}
	
	public override void launch()
	{
		//GameController.instance.lookForEmptyAdjacentTile("invopque un Grizzly", "Lancer Grizzly", HaloSkill.Grizzly);
	}
	
	public override void resolve(List<int> targetsPCC)
	{

//		int amount = GameController.instance.getCurrentSkill().ManaCost;
//		
//		int targetX = args [0];
//		int targetY = args [1];
//		GameController.instance.spawnMinion("Grizzly", targetX, targetY, amount, true);
//		GameController.instance.addGameEvent(GameController.instance.getCurrentSkill().Action, "");
//
//		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
//			" a lancé Grizzly\n ");

	}
	
	public override string isLaunchable()
	{
		return "";
	}
}
