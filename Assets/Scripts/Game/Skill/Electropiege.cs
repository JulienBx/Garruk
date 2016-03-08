using UnityEngine;
using System.Collections.Generic;

public class Electropiege : GameSkill
{
	public Electropiege(){
		this.numberOfExpectedTargets = 1 ;
		base.name = "Electropiège";
		base.ciblage = 6 ;
		base.auto = false;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentTileTargets();
	}
	
	public override void resolve(List<Tile> targetsTile)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int amount = 10+2*GameView.instance.getCurrentSkill().Power;
		GameController.instance.addElectropiege(amount, targetsTile[0]);
		GameController.instance.endPlay();
	}

	public override string getTargetText(int i){
		int amount = 10+2*GameView.instance.getCurrentSkill().Power;
		string s = "Pose un piège qui infligera "+amount+" dégats à l'unité touchée";
		return s ;
	}
}
