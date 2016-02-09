using UnityEngine;
using System.Collections.Generic;

public class Renfoderme : GameSkill
{
	public Renfoderme()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Renfoderme";
		base.ciblage = 2 ;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentAllyTargets();
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
						if(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=-1){
							GameController.instance.applyOnViro(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y), GameView.instance.getCurrentCard().Skills[0].Power*5);
						}
					}
				}
			}
			else{
				GameController.instance.esquive(target,39);
			}
		}
		GameController.instance.endPlay();
	}

	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int bonusShield = level*5;
		
		GameView.instance.getCard(target).addShieldModifyer(new Modifyer(bonusShield, -1, 39, base.name, "Bouclier : "+bonusShield+"%. Permanent"));
		GameView.instance.displaySkillEffect(target, "Bouclier "+bonusShield+"%", 1);
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.addAnim(GameView.instance.getTile(target), 39);
	}	

	public override void applyOnViro(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int bonusShield = Mathf.RoundToInt(level*5*value/100f);
		
		GameView.instance.getCard(target).addShieldModifyer(new Modifyer(bonusShield, -1, 39, base.name, "Bouclier : "+bonusShield+"%. Permanent"));
		GameView.instance.displaySkillEffect(target, "Virus\nBouclier "+bonusShield+"%", 1);
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.addAnim(GameView.instance.getTile(target), 39);
	}

	public override string getTargetText(int target){
		
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int bonusShield = 5*level;
		
		string text = "Bouclier "+bonusShield+"%";
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
