﻿using UnityEngine;
using System.Collections.Generic;

public class PistoBoost : GameSkill
{
	public PistoBoost()
	{
		this.initTexts();
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"PistoBoost","PistoBoost"});
		texts.Add(new string[]{". Actif 1 tour",". For 1 turn"});
		texts.Add(new string[]{"+ARG1 ATK\nPour 1 tour","+ARG1 ATK\nFor 1 turn"});
		texts.Add(new string[]{"+ARG1 ATK\nPour 1 tour\nVirus","+ARG1 ATK\nFor 1 turn\nVirus"});
		texts.Add(new string[]{"ATK : ARG1 -> [ARG2-ARG3]", "ATK : ARG1 -> [ARG2-ARG3]"});
		base.ciblage = 4 ;
		base.auto = false;
		base.id = 3 ;
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
		int level = GameView.instance.getCurrentSkill().Power;
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn2(target, Random.Range(level, 5+level+1));
				if(GameView.instance.getCurrentCard().isVirologue()){
					List<Tile> adjacents = GameView.instance.getPlayingCardTile(target).getImmediateNeighbourTiles();
					for(int i = 0 ; i < adjacents.Count ; i++){
						if(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=-1 && GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=GameView.instance.getCurrentPlayingCard()){
							GameController.instance.applyOnViro2(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y), GameView.instance.getCurrentCard().Skills[0].Power*5+10, Random.Range(1, 3+2*level+1));
						}
					}
				}
			}
			else{
				GameController.instance.esquive(target,this.getText(0));
			}
		}
		GameController.instance.playSound(37);

		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int level){
		string text = this.getText(0);
		GameCard targetCard = GameView.instance.getCard(target);

		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(level, 1, 3, text, this.getText(1)));
		GameView.instance.displaySkillEffect(target, this.getText(2, new List<int>{level}), 2);	
		GameView.instance.addAnim(7,GameView.instance.getTile(target));
	}	

	public override void applyOnViro2(int target, int value, int level){
		string text = this.getText(0);
		int bonus = Mathf.RoundToInt(level*value/100f);
		GameCard targetCard = GameView.instance.getCard(target);

		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(bonus, 1, 3, text, this.getText(1)));
		GameView.instance.displaySkillEffect(target, this.getText(3, new List<int>{level}), 2);	
		GameView.instance.addAnim(7,GameView.instance.getTile(target));
	}

	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		int level = 5+GameView.instance.getCurrentSkill().Power;

		string text = this.getText(4, new List<int>{targetCard.getAttack(),(targetCard.getAttack()+GameView.instance.getCurrentSkill().Power),(targetCard.getAttack()+level)});
		
		int amount = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0), 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		int score = 0 ;
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard2;
		int proba = WordingSkills.getProba(s.Id,s.Power);

		int levelMin = s.Power;
		int levelMax = 5+s.Power;

		score+=Mathf.RoundToInt(((proba-targetCard.getMagicalEsquive())/100f)*(targetCard.getLife()/50f)*((levelMin+levelMax)/2));

		if(currentCard.isVirologue()){
			levelMax = Mathf.RoundToInt((5+s.Power*2)*(10f+currentCard.Skills[0].Power*5f)/100f);
			List<Tile> neighbours = t.getImmediateNeighbourTiles();
			for(int i = 0; i < neighbours.Count; i++){
				if(GameView.instance.getTileCharacterID(neighbours[i].x, neighbours[i].y)!=-1){
					targetCard2 = GameView.instance.getCard(GameView.instance.getTileCharacterID(neighbours[i].x, neighbours[i].y));
					if(targetCard2.isMine){
						score-=Mathf.RoundToInt(((proba-targetCard2.getMagicalEsquive())/100f)*(targetCard2.getLife()/50f)*((levelMin+levelMax)/2));
					}
					else{
						score+=Mathf.RoundToInt(((proba-targetCard2.getMagicalEsquive())/100f)*(targetCard2.getLife()/50f)*((levelMin+levelMax)/2));
					}
				}
			}
		}

		score = score * GameView.instance.IA.getSoutienFactor() ;
		return score ;
	}
}
