using UnityEngine;
using System.Collections.Generic;

public class Loup : GameSkill
{
	public Loup()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance Resurrection");
		GameController.instance.lookForTileTarget("invopque un Loup", "Lancer Loup");
	}
	
	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().Power * -1;
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé Loup\n ");
		
		int targetX = args [0];
		int targetY = args [1];
		//GameController.instance.spawnMinion("loup", targetX, targetY);
	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
