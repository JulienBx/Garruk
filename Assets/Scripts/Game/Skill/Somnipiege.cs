using UnityEngine;
using System.Collections.Generic;

public class Somnipiege : GameSkill
{
	public Somnipiege(){
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Parapiège";
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
		int amount = 1;
		if(Random.Range(1,100)<50+(GameView.instance.getCurrentSkill().Power-1)*5){
			amount = 2 ; 
		}
		GameController.instance.addParapiege(amount, t, 50+(GameView.instance.getCurrentSkill().Power-1)*5);
	}
	
	public override string getTargetText(int i){
		int amount = 50+(GameView.instance.getCurrentSkill().Power-1)*5;
		string s = "Pose un parapiège paralysant l'unité piégée. HIT% : "+amount;
		return s ;
	}
}
