using UnityEngine;
using System.Collections.Generic;

public class Fortifiant : GameSkill
{
	public Fortifiant()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Fortifiant";
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
		int level = GameView.instance.getCurrentSkill().Power;
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn2(target, Random.Range(level, 10+2*level+1));
				if(GameView.instance.getCurrentCard().isVirologue()){
					List<Tile> adjacents = GameView.instance.getPlayingCardTile(target).getImmediateNeighbourTiles();
					for(int i = 0 ; i < adjacents.Count ; i++){
						if(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=-1 && GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=GameView.instance.getCurrentPlayingCard()){
							GameController.instance.applyOnViro2(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y), GameView.instance.getCurrentCard().Skills[0].Power*5, Random.Range(level, 10+2*level+1));
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
	
	public override void applyOn(int target, int level){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();

		GameView.instance.getCard(target).attackModifyers.Add(new Modifyer(level, 1, 3, text, "+"+level+"ATK. Actif 1 tour"));
		GameView.instance.getPlayingCardController(target).updateAttack();
		GameView.instance.displaySkillEffect(target, base.name+"\n+"+level+"ATK pour 1 tour", 1);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 3);
	}	

	public override void applyOnViro2(int target, int value, int level){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int bonus = Mathf.RoundToInt(level*value/100f);

		GameView.instance.getCard(target).attackModifyers.Add(new Modifyer(level, 1, 3, text, "+"+level+"ATK. Actif 1 tour"));
		GameView.instance.getPlayingCardController(target).updateAttack();
		GameView.instance.displaySkillEffect(target, base.name+"\n(Virus)\n+"+level+"ATK pour 1 tour", 1);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 3);
	}

	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = 10+GameView.instance.getCurrentSkill().Power*2;

		string text = "ATK : "+targetCard.getAttack()+" -> ["+(targetCard.getAttack()+1)+"-"+(targetCard.getAttack()+level)+"]\nActif 1 tour";
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
