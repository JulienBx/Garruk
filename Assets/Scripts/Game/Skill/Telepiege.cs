using UnityEngine;
using System.Collections.Generic;

public class Telepiege : GameSkill
{
	public Telepiege(){
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Télépiège";
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
		int amount = GameView.instance.getCurrentSkill().Power;
		GameController.instance.addTelepiege(amount, targetsTile[0]);
		GameController.instance.endPlay();
	}
	
	public override string getTargetText(int i){
		int amount = GameView.instance.getCurrentSkill().Power;
		string s = "Pose un piège qui téléportera l'unité touchée dans un rayon de "+amount+" cases";
		return s ;
	}
}
