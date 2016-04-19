﻿using UnityEngine;
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
		this.displayTargets(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}
	
	public override void resolve(List<Tile> targetsTile)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		GameController.instance.addCharacter(6, 0, GameView.instance.getCurrentSkill().Power*5 , targetsTile[0]);
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}

	public override string getTargetText(int i){
		int amount = 5*GameView.instance.getCurrentSkill().Power;
		string s = "Invoque un robot bouclier (0ATK, "+amount+"PV)";
		return s ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}
}

