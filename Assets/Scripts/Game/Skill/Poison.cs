﻿using UnityEngine;
using System.Collections.Generic;

public class Poison : GameSkill
{
	public Poison()
	{
		this.initTexts();
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Poison","Poison"});
		texts.Add(new string[]{"-ARG1 PV en fin de tour","-ARG1 HP at the end of the turn"});
		texts.Add(new string[]{"Perd ARG1 PV par tour","Loses ARG1 HP per turn"});
		texts.Add(new string[]{"Perd ARG1 PV par tour\nVirus","Loses ARG1 HP per turn\nVirus"});
		base.ciblage = 1 ;
		base.auto = false;
		base.id = 94 ;
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
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn(target);
				if(GameView.instance.getCurrentCard().isVirologue()){
					List<Tile> adjacents = GameView.instance.getPlayingCardTile(target).getImmediateNeighbourTiles();
					for(int i = 0 ; i < adjacents.Count ; i++){
						if(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=-1 && GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=GameView.instance.getCurrentPlayingCard()){
							GameController.instance.applyOnViro(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y), 10+GameView.instance.getCurrentCard().Skills[0].Power*5);
						}
					}
				}
			}
			else{
				GameController.instance.esquive(target,this.getText(0));
			}
		}
		GameController.instance.playSound(30);
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		int level = 5+GameView.instance.getCurrentSkill().Power;

		GameView.instance.getCard(target).setPoison(new Modifyer(level, -1, 94, this.getText(0), this.getText(1, new List<int>{level})));
		GameView.instance.getPlayingCardController(target).showIcons();

		GameView.instance.displaySkillEffect(target, this.getText(2, new List<int>{level}), 0);	
		GameView.instance.addAnim(4,GameView.instance.getTile(target));
	}	

	public override void applyOnViro(int target, int value){
		int level = Mathf.RoundToInt((5+GameView.instance.getCurrentSkill().Power)*value/100f);

		GameView.instance.getCard(target).setPoison(new Modifyer(level, -1, 94, this.getText(0), this.getText(1, new List<int>{level})));
		GameView.instance.getPlayingCardController(target).showIcons();

		GameView.instance.displaySkillEffect(target, this.getText(3, new List<int>{level}), 0);	
		GameView.instance.addAnim(4,GameView.instance.getTile(target));
	}
	
	public override string getTargetText(int target){	
		GameCard targetCard = GameView.instance.getCard(target);
		int level = GameView.instance.getCurrentSkill().Power+5;

		string text = this.getText(0)+"\n"+ this.getText(1, new List<int>{level});
		
		int amount = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getEsquive();
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

		if(!targetCard.isPoisoned()){
			score+=Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*2*(5+s.Power));
		}

		if(currentCard.isVirologue()){
			int levelMin2 = Mathf.RoundToInt(s.Power*(10f+currentCard.Skills[0].Power*5f)/100f);
			int levelMax2 = Mathf.RoundToInt((5+s.Power)*(10f+currentCard.Skills[0].Power*5f)/100f);
			List<Tile> neighbours = t.getImmediateNeighbourTiles();
			for(int i = 0; i < neighbours.Count; i++){
				if(GameView.instance.getTileCharacterID(neighbours[i].x, neighbours[i].y)!=-1){
					targetCard2 = GameView.instance.getCard(GameView.instance.getTileCharacterID(neighbours[i].x, neighbours[i].y));
					if(!targetCard2.isPoisoned()){
						if(targetCard2.isMine){
							score+=Mathf.RoundToInt(((proba-targetCard2.getEsquive())/100f)*2*(5+s.Power));
						}
						else{
							score-=Mathf.RoundToInt(((proba-targetCard2.getEsquive())/100f)*2*(5+s.Power));
						}
					}
				}
			}
		}

		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
