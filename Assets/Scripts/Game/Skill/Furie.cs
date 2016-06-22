﻿using UnityEngine;
using System.Collections.Generic;

public class Furie : GameSkill
{
	public Furie()
	{
		this.numberOfExpectedTargets = 0 ; 
		base.ciblage = 0 ;
		base.auto = true;
		base.id = 93 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(this.getText(0), WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1));
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targets)
	{	                     
		GameController.instance.applyOnMe(-1);
		GameController.instance.playSound(37);
		GameController.instance.endPlay();
	}
	
	public override void applyOnMe(int target){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int bonusLife = level*2+10;
		int bonusAttack = 5+level;
		target = GameView.instance.getCurrentPlayingCard();

		string text = "Furie\n+"+bonusAttack+" ATK";
		if(bonusLife>0){
			text+="\n+"+bonusLife+"PV";
		}

		GameView.instance.getPlayingCardController(target).updateAttack(currentCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(bonusAttack, -1, 93, this.getText(0), ". Permanent"));
		GameView.instance.getPlayingCardController(target).updateLife(currentCard.getLife());
		GameView.instance.getPlayingCardController(target).addPVModifyer(new Modifyer(bonusLife, -1, 93, this.getText(0), ". Permanent"));

		GameView.instance.getCard(target).setFurious(new Modifyer(0, -1, 93, this.getText(0), "Incontrolable!"));
		GameView.instance.getPlayingCardController(target).showIcons();

		GameView.instance.displaySkillEffect(target, text, 1);
		GameView.instance.addAnim(7,GameView.instance.getTile(target));

	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int score = 0;
		List<int> allys = GameView.instance.getAllys(false);
		if(allys.Count<=1){
			score+=50;
		}

		score+=10-currentCard.getLife();
		return score ;
	}
}
