using UnityEngine;
using System.Collections.Generic;

public class TargetTileHandler
{
	List<Tile> targetsTile ;
	int numberOfExpectedTargets ;
	
	public void addTargetTile(Tile targetTile)
	{
		this.targetsTile.Add(targetTile);
		if (this.targetsTile.Count>=this.numberOfExpectedTargets){
			GameController.instance.hidePCCTargets();
			//GameController.instance.resolveSkill (targetsTile);
		}
	}	
}




