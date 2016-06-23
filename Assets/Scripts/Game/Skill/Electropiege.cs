using UnityEngine;
using System.Collections.Generic;

public class Electropiege : GameSkill
{
	public Electropiege(){
		this.initTexts();
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Electropiège","Elektrap"});
		texts.Add(new string[]{"Pose un piège. Infligera ARG1 dégats à l'unité touchée","Sets a trap. It will inflict ARG1 damages to the wounded unit"});
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
		string s = this.getText(1,new List<int>{amount});
		return s ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0), 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}
}
