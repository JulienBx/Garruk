using UnityEngine;
using System.Collections.Generic;

public class Electropiege : GameSkill
{
	public Electropiege(){
		this.numberOfExpectedTargets = 1 ;
		base.name = "Electropiège";
		base.ciblage = 6 ;
		base.auto = false;
		base.id = 13 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		this.displayTargets();
	}
	
	public override void resolve(List<Tile> targetsTile)
	{	
		GameController.instance.play(this.id);
		int amount = 10+2*GameView.instance.getCurrentSkill().Power;
		GameController.instance.addElectropiege(amount, targetsTile[0]);
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}

	public override string getTargetText(int i){
		int amount = 10+2*GameView.instance.getCurrentSkill().Power;
		string s = "Pose un piège. Infligera "+amount+" dégats à l'unité touchée";
		return s ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}
}
