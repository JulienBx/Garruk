﻿using UnityEngine;
using System.Collections.Generic;

public class Vampire : GameSkill
{
	public Vampire(){
		this.numberOfExpectedTargets = 0 ; 
		base.name = "Vampire";
		base.ciblage = 16 ;
		base.auto = true;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name, WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1));
	}
	
	public override void resolve(List<Tile> targetsP)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		GameCard currentCard = GameView.instance.getCurrentCard();
		List<int> targets = GameView.instance.getEveryoneNextCristal() ; 
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		for(int i = 0 ; i < targets.Count ; i++){
			if (Random.Range(1,101) <= GameView.instance.getCard(targets[i]).getMagicalEsquive()){
				GameController.instance.esquive(targets[i],1);
			}
			else{
				if (Random.Range(1,101) <= proba){
					GameController.instance.applyOn(targets[i]);
				}
				else{
					GameController.instance.esquive(targets[i],base.name);
				}
			}
		}

		GameController.instance.applyOnMe(targets.Count);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = 2+GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getNormalDamagesAgainst(targetCard, level);

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 40, base.name, damages+" dégats subis"),  (target==GameView.instance.getCurrentPlayingCard()));
		GameView.instance.displaySkillEffect(target, "-"+damages+"PV", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 40);
	}

	public override void applyOnMe(int value){
		int level = 2+GameView.instance.getCurrentSkill().Power;
		int damages = value*level;

		GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addDamagesModifyer(new Modifyer(-1*damages, -1, 40, base.name, damages+" dégats subis"), false);
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name+"\n+"+damages+"PV", 2);	
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}

	public override int getActionScore(Tile t, Skill s){
		int score = 0 ;

		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard;
		List<int> targets = GameView.instance.getEveryoneNextCristal() ; 
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int level = 2+s.Power;
		int life = currentCard.getLife();

		for(int i = 0 ; i < targets.Count ; i++){
			targetCard = GameView.instance.getCard(targets[i]);
			if(level>=targetCard.getLife()){
				if(targetCard.isMine){
					score+=Mathf.RoundToInt(200f*(proba-targetCard.getMagicalEsquive())/100f);
				}
				else{
					score-=Mathf.RoundToInt(200f*(proba-targetCard.getMagicalEsquive())/100f);
				}
				score+=Mathf.Min(currentCard.GetTotalLife()-life,targetCard.getLife());
				life+=Mathf.RoundToInt(targetCard.getLife()*(proba-targetCard.getMagicalEsquive())/100f);
			}
			else{
				score+=Mathf.RoundToInt(level*(proba-targetCard.getMagicalEsquive())/100f);
				if(!targetCard.isMine){
					score = score*(-1);
				}
				score+=Mathf.Min(currentCard.GetTotalLife()-life,level);
				life+=Mathf.RoundToInt(level*(proba-targetCard.getMagicalEsquive())/100f);
			}
		}
		score = score * GameView.instance.IA.getAgressiveFactor() ;

		return score ;
	}
}
