using UnityEngine;
using System.Collections.Generic;

public class Sermon : GameSkill
{
	public Sermon()
	{
		this.numberOfExpectedTargets = 0 ;
		base.name = "Sermon";
		base.ciblage = 12 ;
		base.auto = true;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name, WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power));
		GameController.instance.play(GameView.instance.runningSkill);
	}
	
	public override void resolve(List<Tile> targets)
	{	
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);

		List<int> tempTiles = GameView.instance.getEveryone();
		int i = 0 ;
		int tempInt ;

		while (i<tempTiles.Count){ 
			tempInt = tempTiles[i];
			if (tempInt!=-1)
			{
				if (GameView.instance.getPlayingCardController(tempInt).canBeTargeted())	
				{
					if (Random.Range(1,101) <= GameView.instance.getCard(tempInt).getEsquive())
					{                             
						GameController.instance.esquive(tempInt,1);
					}
					else{
						if (Random.Range(1,101) <= proba){
							GameController.instance.applyOn(tempInt);
						}
						else{
							GameController.instance.esquive(tempInt,base.name);
						}
					}
				}
			}
			i++;
		}
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		int level = GameView.instance.getCurrentSkill().Power+5;
		GameCard targetCard = GameView.instance.getCard(target);

		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(level, 1, 102, base.name, ". Actif 1 tour"));
		GameView.instance.displaySkillEffect(target, "+"+level+"ATK", 2);
		GameView.instance.addAnim(GameView.instance.getTile(target), 102);
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}
}
