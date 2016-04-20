using UnityEngine;
using System.Collections.Generic;

public class PistoSape : GameSkill
{
	public PistoSape()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "PistoSape";
		base.ciblage = 3 ;
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
		int level = GameView.instance.getCurrentSkill().Power;

		if (Random.Range(1,101) <= GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn2(target, Random.Range(1, 3+2*level+1));
				if(GameView.instance.getCurrentCard().isVirologue()){
					List<Tile> adjacents = GameView.instance.getPlayingCardTile(target).getImmediateNeighbourTiles();
					for(int i = 0 ; i < adjacents.Count ; i++){
						if(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=-1 && GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y)!=GameView.instance.getCurrentPlayingCard()){
							GameController.instance.applyOnViro2(GameView.instance.getTileCharacterID(adjacents[i].x, adjacents[i].y), GameView.instance.getCurrentCard().Skills[0].Power*5+25, Random.Range(1, 3+2*level+1));
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
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		int level = Mathf.Min(targetCard.getAttack()-1, value);
		
		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(-1*level, 1, 4, text, ". Actif 1 tour"));
		GameView.instance.displaySkillEffect(target, (-1*level)+"ATK\n1 tour", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 4);
	}

	public override void applyOnViro2(int target, int value, int amount){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		int level = Mathf.Min(targetCard.getAttack()-1,Mathf.RoundToInt(amount*value/100f));

		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(-1*level, 1, 4, text, ". Actif 1 tour"));
		GameView.instance.displaySkillEffect(target, "Virus\n"+level+"ATK\n1 tour", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 4);
	}

	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		int minLevel = Mathf.Min(1, targetCard.getAttack()-1);
		int maxLevel = Mathf.Min(3+2*GameView.instance.getCurrentSkill().Power, targetCard.getAttack()-1);

		string text = "ATK : "+targetCard.getAttack()+" -> ["+(targetCard.getAttack()-minLevel)+"-"+(targetCard.getAttack()-maxLevel)+"]\nActif 1 tour";
		
		int amount = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}
}
