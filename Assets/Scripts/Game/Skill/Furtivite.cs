﻿using UnityEngine;
using System.Collections.Generic;

public class Furtivite : GameSkill
{
	public Furtivite()
	{
		this.initTexts();
		this.numberOfExpectedTargets = 1 ; 
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Furtivité","Stealth"});
		texts.Add(new string[]{". Actif 1 tour",". For 1 turn"});
		texts.Add(new string[]{"+ARG1 ATK","+ARG1 ATK"});
		texts.Add(new string[]{"ATK : ARG1 -> ARG2","ATK : ARG1 -> ARG2"});
		base.ciblage = 10 ;
		base.auto = true;
		base.id = 9 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(this.getText(0), GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targets)
	{	                     
		GameController.instance.applyOnMe(-1);
		GameController.instance.playSound(37);

		GameController.instance.endPlay();
	}
	
	public override void applyOnMe(int target){
		target = GameView.instance.getCurrentPlayingCard();
		GameCard targetCard = GameView.instance.getCard(target);
		int attack = GameView.instance.getCurrentSkill().Power+5;

		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(attack, 2, 9, this.getText(0), this.getText(1)));

		GameView.instance.getPlayingCardController(target).addMagicalEsquiveModifyer(new Modifyer(100, 2, 9, this.getText(0), this.getText(1)));
		GameView.instance.getPlayingCardController(target).showIcons();

		GameView.instance.displaySkillEffect(target, this.getText(0)+"\n"+this.getText(2, new List<int>{attack}), 2);
		GameView.instance.addAnim(7,GameView.instance.getTile(target));
	}

	public override string getTargetText(int target){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int attack = GameView.instance.getCurrentSkill().Power+2;

		string text = this.getText(3,new List<int>{currentCard.getAttack(),(currentCard.getAttack()+attack)});

		text += "\nHIT% : 100";
		
		return text ;
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard ;
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int score = Mathf.RoundToInt((proba/100f)*(currentCard.getLife()/50f)*(2f+s.Power));
		List<int> enemies = GameView.instance.getOpponents(false);
		for(int i = 0 ; i < enemies.Count ; i++){
			targetCard = GameView.instance.getCard(enemies[i]);
			if(targetCard.CardType.Id==3){
				score+=15;
			}
		}

		score = score * GameView.instance.IA.getSoutienFactor() ;

		return score ;
	}
}
