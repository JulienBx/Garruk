using UnityEngine;
using System.Collections.Generic;

public class Poisonpiege : GameSkill
{
	public Poisonpiege(){
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Parapiège";
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
		int amount = 2+GameView.instance.getCurrentSkill().Power;
		GameController.instance.addPoisonPiege(amount, targetsTile[0]);
		GameController.instance.endPlay();
	}
	
	public override string getTargetText(int i){
		int amount = 10+GameView.instance.getCurrentSkill().Power;
		string s = "Pose un piège qui empoisonnera l'unité touchée : "+amount+" dégats par tour";
		return s ;
	}
}
