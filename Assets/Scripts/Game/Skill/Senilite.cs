using UnityEngine;
using System.Collections.Generic;

public class Senilite : GameSkill
{
	public Senilite()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Sénilité";
		base.ciblage = 3 ; 
		base.auto = false;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		this.displayTargets(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int max = 2 * GameView.instance.getCurrentSkill().Power+5;
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				int amount = Random.Range(1,max);
				GameController.instance.applyOn2(target, amount);
				if(GameView.instance.getCurrentCard().isVirologue()){
					List<Tile> adjacents = GameView.instance.getPlayingCardTile(target).getImmediateNeighbourTiles();
					for(int i = 0 ; i < adjacents.Count ; i++){
						if(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=-1 && GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=GameView.instance.getCurrentPlayingCard()){
							GameController.instance.applyOnViro(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y), Mathf.Max(1,Mathf.RoundToInt(amount*(0.25f+GameView.instance.getCurrentCard().Skills[0].Power*0.05f))));
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
	
	public override void applyOn(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);
		value = -1*Mathf.Min(value, targetCard.getAttack()-1);

		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(value, -1, 57, base.name, ". Permanent"));
		GameView.instance.displaySkillEffect(target, value+" ATK", 0);
		GameView.instance.addAnim(GameView.instance.getTile(target), 57);
	}

	public override void applyOnViro(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);
		value = -1*Mathf.Min(value, targetCard.getAttack()-1);

		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(value, -1, 57, base.name, ". Permanent"));
		GameView.instance.displaySkillEffect(target, "Virus\n"+value+" ATK", 0);
		GameView.instance.addAnim(GameView.instance.getTile(target), 57);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		int level = 5+2*GameView.instance.getCurrentSkill().Power;
		string text = "";

		if(targetCard.getAttack()>2){
			text = "ATK : "+targetCard.getAttack()+" -> ["+Mathf.Max(1,targetCard.getAttack()-1)+"-"+Mathf.Max(1,(targetCard.getAttack()-level))+"]\nPermanent";
		}
		else{
			text = "ATK : "+targetCard.getAttack()+" -> 1\nPermanent";
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
}
