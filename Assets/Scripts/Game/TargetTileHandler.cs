using UnityEngine;
using System.Collections.Generic;

public class TargetTileHandler
{
	List<Tile> targetsTile ;
	int numberOfExpectedTargets ;
	
	public TargetTileHandler(int n){
		this.numberOfExpectedTargets = n ;
		this.targetsTile = new List<Tile>();
	}
	
	public void addTargetTile(Tile targetTile)
	{
		this.targetsTile.Add(targetTile);
		if (this.targetsTile.Count>=this.numberOfExpectedTargets){
			GameView.instance.hideTargets();
			GameSkills.instance.getCurrentSkill().resolve(this.targetsTile);
			//GameController.instance.getCurrentGameSkill().resolve (targetsTile);
		}
	}	
}




