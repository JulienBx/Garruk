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
		if (this.targetsTile.Count==this.numberOfExpectedTargets){
			GameView.instance.hideTargets();
			GameView.instance.hideAllTargets();
			GameView.instance.hoveringZone = -1 ;
			GameSkills.instance.getCurrentGameSkill().resolve(this.targetsTile);
		}
	}	
}




