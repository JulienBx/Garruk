using UnityEngine;
using System.Collections.Generic;

public class Protection : GameSkill
{
	public Protection(){
		this.numberOfExpectedTargets = 1 ;
		base.name = "Protection";
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
		GameController.instance.addCharacter(6, 0, GameView.instance.getCurrentSkill().Power*5 , targetsTile[0]);
		GameController.instance.endPlay();
	}

	public override string getTargetText(int i){
		int amount = 5*GameView.instance.getCurrentSkill().Power;
		string s = "Invoque un robot bouclier (0ATK, "+amount+"PV)";
		return s ;
	}
}

