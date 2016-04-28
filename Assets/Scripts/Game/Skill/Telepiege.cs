using UnityEngine;
using System.Collections.Generic;

public class Telepiege : GameSkill
{
	public Telepiege(){
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Télépiège";
		base.ciblage = 6 ;
		base.auto = false;
		base.id = 58 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		this.displayTargets();
	}
	
	public override void resolve(List<Tile> targetsTile)
	{	
		GameController.instance.play(this.id);
		int amount = GameView.instance.getCurrentSkill().Power;
		GameController.instance.addTelepiege(amount, targetsTile[0]);
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override string getTargetText(int i){
		int amount = GameView.instance.getCurrentSkill().Power;
		string s = "Pose un piège qui téléportera l'unité touchée dans un rayon de "+amount+" cases";
		return s ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}
}
