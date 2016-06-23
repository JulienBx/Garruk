using UnityEngine;
using System.Collections.Generic;

public class Telepiege : GameSkill
{
	public Telepiege(){
		this.initTexts();
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Telepiège","TeleporTrap"});
		texts.Add(new string[]{"Pose un piège qui téléportera l'unité touchée dans un rayon de ARG1 case(s)","Sets a trap which will teleport the wounded unit ARG1 tile(s) away"});
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
		string s = this.getText(1, new List<int>{amount});
		return s ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0), 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}
}
