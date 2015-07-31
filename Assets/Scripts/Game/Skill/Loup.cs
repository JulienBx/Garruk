using UnityEngine;
using System.Collections.Generic;

public class Loup : GameSkill
{
	public Loup()
	{
		
	}
	
	public override void launch()
	{
		//GameController.instance.lookForEmptyAdjacentTile("invoque un Loup", "Lancer Loup", HaloSkill.Wolf);
	}
	
	public override void resolve(List<int> targetsPCC)
	{

//		int amount = GameController.instance.getCurrentSkill().ManaCost;
//		
//		int targetX = args [0];
//		int targetY = args [1];
//		GameController.instance.spawnMinion("Loup", targetX, targetY, amount, true);
//		GameController.instance.addGameEvent(GameController.instance.getCurrentSkill().Action, "");
//		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
//			" a lancé Loup\n ");

	}
	
	public override string isLaunchable()
	{
		return "";
	}
}
