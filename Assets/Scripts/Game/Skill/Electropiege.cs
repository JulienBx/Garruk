using UnityEngine;
using System.Collections.Generic;

public class Electropiege : GameSkill
{
	public Electropiege(){
		this.numberOfExpectedTargets = 1 ;
		base.name = "Electropiège";
		base.ciblage = 6 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentTileTargets();
	}
	
	public override void resolve(List<Tile> targetsTile)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		GameController.instance.applyOnTile(targetsTile[0]);
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 0);
	}
	
	public override void applyOn(Tile t){
		int amount = 4+GameView.instance.getCurrentSkill().Power;
		GameController.instance.addElectropiege(amount, t);
	}
	
	public override string getTargetText(int i){
		int amount = 4+GameView.instance.getCurrentSkill().Power;
		string s = "Pose un electropiège infligeant "+amount+" dégats à l'unité piégée";
		return s ;
	}
	
	
}
