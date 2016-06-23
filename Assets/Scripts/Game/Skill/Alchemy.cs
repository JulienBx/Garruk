using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Alchemy : GameSkill
{
	public Alchemy()
	{
		this.initTexts();
		this.numberOfExpectedTargets = 1 ; 
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Alchimie","Alchemy"});
		texts.Add(new string[]{"ARG1 cristaux créés","Creation of ARG1 cristals"});
		texts.Add(new string[]{"1 cristal créé","Creation of 1 cristal"});
		texts.Add(new string[]{"échec","fail"});

		base.ciblage = 6 ;
		base.auto = true;
		base.id = 42 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(this.getText(0), GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
		GameController.instance.play(base.id);
	}
	
	public override void resolve(List<Tile> targets)
	{	                     
		List<Tile> neighbours = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()).getImmediateNeighbourTiles();
		int level = GameView.instance.getCurrentSkill().Power*4+20;
		int compteur = 0 ; 
		for(int i = 0 ; i < neighbours.Count ; i++){
			if(GameView.instance.getTileController(neighbours[i]).getCharacterID()==-1 && !GameView.instance.getTileController(neighbours[i]).isRock()){
				if (Random.Range(1,101) <= level){
					GameController.instance.applyOn2(neighbours[i].x, neighbours[i].y);
					compteur++;
				}
			}
		}
		if(compteur>=1){
			GameController.instance.playSound(28);
		}
		else{
			GameController.instance.playSound(30);
		}
		GameController.instance.applyOnMe(compteur);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int value){
		GameView.instance.getTileController(target,value).addRock(42);
		GameView.instance.recalculateDestinations();
	}

	public override void applyOnMe(int value){
		if(value>1){
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0)+"\n"+this.getText(1, new List<int>{value}), 2);
		}
		else if(value==1){
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0)+"\n"+this.getText(2), 2);
		}
		else{
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0)+"\n"+this.getText(3), 0);
		}
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		List<Tile> neighbours = t.getImmediateNeighbourTiles();
		List<int> allys = GameView.instance.getAllys(false);

		int score = 0 ;
		for (int i = 0 ; i < neighbours.Count ; i++){
			if(GameView.instance.getTileController(neighbours[i].x, neighbours[i].y).canBeDestination()){
				score += Mathf.RoundToInt(5f * (GameView.instance.getCurrentSkill().Power*4f+20f) /100f);
				for (int j = 0 ; j < allys.Count ; j++){
					if(GameView.instance.getCard(allys[j]).Skills[0].Id == 139 || GameView.instance.getCard(allys[j]).Skills[0].Id == 141){
						score +=Mathf.RoundToInt(10f * (s.Power*4f+20f) /100f);
					}
				}
			}
		}
		score = score * GameView.instance.IA.getSoutienFactor() ;
		return score ;
	}
}
