﻿using UnityEngine;
using System.Collections.Generic;

public class Blesser : GameSkill
{
	public Blesser(){
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Blesser";
		base.ciblage = 1 ; 
		base.auto = false;
		base.id = 11 ;
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
		int minMalus = GameView.instance.getCurrentSkill().Power ;
		int maxMalus = GameView.instance.getCurrentSkill().Power*3+5;

		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn2(target, Random.Range(minMalus, maxMalus+1));
			}
			else{
				GameController.instance.esquive(target,base.name);
			}
		}
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*0.5f));
		int malus = Mathf.Min(targetCard.getAttack()-1, value);
		string text = "-"+damages+"PV\n-"+value+"ATK\n1 tour";				
		if (currentCard.isLache() && !currentCard.hasMoved){
			damages = currentCard.getNormalDamagesAgainst(targetCard, damages+5+currentCard.getSkills()[0].Power);
			text ="-"+damages+"PV\n-"+value+"ATK\n1 tour\n(lâche)";
		}
		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(-1*malus, 1, 11, base.name, ". Actif 1 tour"));
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,11,base.name,damages+" dégats subis"), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.addAnim(GameView.instance.getTile(target), 11);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*0.5f));
		int minMalus = GameView.instance.getCurrentSkill().Power ;
		int maxMalus = GameView.instance.getCurrentSkill().Power*3+5;
		string text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages)+"\nATK : "+targetCard.getAttack()+" -> ["+Mathf.Max(1, targetCard.getAttack()-minMalus)+"-"+Mathf.Max(1, targetCard.getAttack()-maxMalus)+"]\nActif 1 tour";				

		if (currentCard.isLache() && !currentCard.hasMoved){
			damages = currentCard.getNormalDamagesAgainst(targetCard, damages+5+currentCard.getSkills()[0].Power);
			text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages)+"\nATK : "+targetCard.getAttack()+" -> ["+Mathf.Max(1, targetCard.getAttack()-minMalus)+"-"+Mathf.Max(1, targetCard.getAttack()-maxMalus)+"]\nActif 1 tour\n(lâche)";				
		}

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
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*0.5f));
		int score ;
		if(damages>=targetCard.getLife()){
			score=200;
		}
		else{
			score=Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*(damages+Mathf.Max(0,30-(targetCard.getLife()-damages))));
		}

		int levelMin = s.Power;
		int levelMax = 5+s.Power*3;

		score+=Mathf.RoundToInt((proba-targetCard.getEsquive()/100f)*(targetCard.getLife()/50f)*Mathf.Min(targetCard.getAttack(),((levelMin+levelMax)/2)));

		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
