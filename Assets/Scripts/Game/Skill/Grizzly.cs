using UnityEngine;
using System.Collections.Generic;

public class Grizzly : GameSkill
{
	public Grizzly()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance Grizzly");
		GameController.instance.lookForEmptyAdjacentTile("invopque un Grizzly", "Lancer Grizzly");
	}
	
	public override void resolve(List<int> targetsPCC)
	{
		int amount = GameController.instance.getCurrentSkill().Power;
		GameController.instance.play();
		
//		int targetX = args [0];
//		int targetY = args [1];
		//GameController.instance.spawnMinion("grizzly", targetX, targetY);
	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
