using UnityEngine;
using System.Collections.Generic;

public class Poison : GameSkill
{
	public Poison()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Poison";
		base.ciblage = 1 ;
		base.auto = false;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		this.displayTargets(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}
	
	public override void resolve(List<Tile> targets)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
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
							GameController.instance.applyOnViro(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y), 25+GameView.instance.getCurrentCard().Skills[0].Power*5);
						}
					}
				}
			}
			else{
				GameController.instance.esquive(target,base.name);
			}
		}
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		int level = 5+GameView.instance.getCurrentSkill().Power;

		GameView.instance.getCard(target).setPoison(new Modifyer(level, -1, 94, base.name, "-"+level+"PV en fin de tour"));
		GameView.instance.getPlayingCardController(target).showIcons();

		GameView.instance.displaySkillEffect(target, "Perd "+level+"PV par tour", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 94);
	}	

	public override void applyOnViro(int target, int value){
		int level = Mathf.RoundToInt((5+GameView.instance.getCurrentSkill().Power)*value/100f);

		GameView.instance.getCard(target).setPoison(new Modifyer(level, -1, 94, base.name, "-"+level+"PV en fin de tour"));
		GameView.instance.getPlayingCardController(target).showIcons();

		GameView.instance.displaySkillEffect(target, "(Virus)\nPerd "+level+"PV par tour", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 94);
	}
	
	public override string getTargetText(int target){	
		GameCard targetCard = GameView.instance.getCard(target);
		int level = GameView.instance.getCurrentSkill().Power+5;

		string text = "Poison\nPerd "+level+"PV en fin de tour";
		
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
}
