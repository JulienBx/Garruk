using UnityEngine;
using System.Collections.Generic;

public class Alchemy : GameSkill
{
	public Alchemy()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Alchemy";
		base.ciblage = 6 ;
		base.auto = true;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name,  WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1));
		GameController.instance.play(GameView.instance.runningSkill);
	}
	
	public override void resolve(List<int> targetsPCC)
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

		GameController.instance.applyOnMe(compteur);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int value){
		GameView.instance.getTileController(target,value).addRock(42);
	}

	public override void applyOnMe(int value){
		if(value>1){
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name+"\n"+value+"cristaux créés", 2);
		}
		else if(value==1){
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name+"\n"+value+"cristal créé", 2);
		}
		else{
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name+"\npas de cristal créé", 0);
		}
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}
}
