﻿using UnityEngine;
using System.Collections.Generic;

public class Virus : GameSkill
{
	public Virus(){
		this.numberOfExpectedTargets = 1 ;
		base.name = "Virus";
		base.ciblage = 18 ;
		base.auto = false;
		base.id = 37 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		this.displayTargets();
	}
	
	public override void resolve(List<Tile> targets)
	{	
		GameController.instance.play(this.id);
		int target = GameView.instance.getTileCharacterID(targets[0].x, targets[0].y);
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn(target);
			}
			else{
				GameController.instance.esquive(target,base.name);
			}
		}
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}

	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		int damages = targetCard.getLife();
		
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 37, base.name, damages+" dégats subis"), false,-1);
		GameView.instance.displaySkillEffect(target, "Succès\nCible détruite", 0);
		GameView.instance.addAnim(GameView.instance.getTile(target), 37);
	}
	
	public override string getTargetText(int id){	
		GameCard targetCard = GameView.instance.getCard(id);
		string text = "Détruit l'unité!";

		int amount = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		
		text += "\n\nHIT% : "+probaHit;
		return text ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}

	public override int getActionScore(Tile t, Skill s){
		int score = 0 ;
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = WordingSkills.getProba(s.Id,s.Power);

		score+=Mathf.RoundToInt((100f*proba/100f)*(targetCard.getLife()/50f));
				
		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
