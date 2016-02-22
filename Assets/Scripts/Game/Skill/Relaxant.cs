using UnityEngine;
using System.Collections.Generic;

public class Relaxant : GameSkill
{
	public Relaxant()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Relaxant";
		base.ciblage = 1 ;
		base.auto = false;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		
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
							GameController.instance.applyOnViro(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y), GameView.instance.getCurrentCard().Skills[0].Power*5);
						}
					}
				}
			}
			else{
				GameController.instance.esquive(target,base.name);
			}
		}
		GameController.instance.endPlay();
	}

	public override void applyOn(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = Mathf.Max(1-targetCard.getAttack(), -5-2*GameView.instance.getCurrentSkill().Power);
		
		GameView.instance.getCard(target).attackModifyers.Add(new Modifyer(level, 1, 3, text, level+"ATK. Actif 1 tour"));
		GameView.instance.getPlayingCardController(target).updateAttack();
		GameView.instance.displaySkillEffect(target, level+"ATK pour 1 tour", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 4);
	}

	public override void applyOnViro(int target, int value){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = Mathf.RoundToInt((-5-2*GameView.instance.getCurrentSkill().Power)*value/100f);

		GameView.instance.getCard(target).attackModifyers.Add(new Modifyer(level, 1, 3, text, level+"ATK. Actif 1 tour"));
		GameView.instance.getPlayingCardController(target).updateAttack();
		GameView.instance.displaySkillEffect(target, "Virus\n"+level+"ATK pour 1 tour", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 4);
	}

	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = -5-2*GameView.instance.getCurrentSkill().Power;

		string text = ""+level+" ATK\nActif 1 tour";
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
