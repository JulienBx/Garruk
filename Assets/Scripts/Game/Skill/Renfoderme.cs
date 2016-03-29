using UnityEngine;
using System.Collections.Generic;

public class Renfoderme : GameSkill
{
	public Renfoderme()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Renfoderme";
		base.ciblage = 2 ;
		base.auto = false;
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
		int level = GameView.instance.getCurrentSkill().Power;
		int bonusShield = 10+level*4;
		
		GameView.instance.getCard(target).addShieldModifyer(new Modifyer(bonusShield, -1, 39, base.name, "Bouclier : "+bonusShield+"%"));
		GameView.instance.displaySkillEffect(target, base.name+"\nBouclier "+bonusShield+"%", 1);
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.addAnim(GameView.instance.getTile(target), 39);
	}	

	public override void applyOnViro(int target, int value){
		int level = GameView.instance.getCurrentSkill().Power;
		int bonusShield = Mathf.RoundToInt((10+level*4)*value/100f);
		
		GameView.instance.getCard(target).addShieldModifyer(new Modifyer(bonusShield, -1, 39, base.name, "Bouclier : "+bonusShield+"%"));
		GameView.instance.displaySkillEffect(target, base.name+"\nVirus\nBouclier "+bonusShield+"%", 1);
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.addAnim(GameView.instance.getTile(target), 39);
	}

	public override string getTargetText(int target){
		
		GameCard targetCard = GameView.instance.getCard(target);
		int level = GameView.instance.getCurrentSkill().Power;
		int bonusShield = 10+4*level;
		
		string text = "Bouclier "+bonusShield+"%";
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
