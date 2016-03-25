﻿using UnityEngine;
using System.Collections.Generic;

public class Cristopower : GameSkill
{
	public Cristopower()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "CristoPower";
		base.ciblage = 11 ;
		base.auto = false;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentRockTargets();
	}
	
	public override void resolve(List<Tile> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		Tile target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		int level = GameView.instance.getCurrentSkill().Power;

		if (Random.Range(1,101) <= proba){
			GameController.instance.applyOn2(GameView.instance.getCurrentPlayingCard(), Random.Range(level, 1+6+2*level));
			GameController.instance.removeRock(target);
		}
		else{
			GameController.instance.esquive(GameView.instance.getCurrentPlayingCard(),base.name);
		}
		
		GameController.instance.endPlay();
	}

	public override void applyOn(int target, int value){
		GameView.instance.getCard(target).attackModifyers.Add(new Modifyer(value, -1, 128, base.name, "+"+value+" ATK. Permanent"));
		GameView.instance.getPlayingCardController(target).updateAttack();
		GameView.instance.displaySkillEffect(target, base.name+"\n+"+value+" ATK. Permanent", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 128);
	}
	
	public override string getTargetText(int target){
		int level = GameView.instance.getCurrentSkill().Power;
		int minBonus = level;
		int maxBonus = 2*level + 6 ;

		string text = "Mange le cristal et gagne : ["+minBonus+" - "+maxBonus+"] ATK";
		text += "\n\nHIT% : 100";
		
		return text ;
	}
}