using UnityEngine;
using System.Collections.Generic;

public class Sermon : GameSkill
{
	public Sermon()
	{
		this.numberOfExpectedTargets = 0 ;
		base.name = "Sermon";
		base.ciblage = 0 ;
		base.auto = true;
		base.id = 102 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name, GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targets)
	{	
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int level = GameView.instance.getCurrentSkill().Power+5;

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
							GameController.instance.applyOn2(tempInt,UnityEngine.Random.Range(1,level+1));
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
		GameController.instance.playSound(37);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int value){
		Debug.Log("J'apply on "+target);
		int level = GameView.instance.getCurrentSkill().Power+5;
		GameCard targetCard = GameView.instance.getCard(target);

		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(value, 1, 102, base.name, ". Actif 1 tour"));
		GameView.instance.displaySkillEffect(target, "+"+value+"ATK", 2);
		GameView.instance.addAnim(7,GameView.instance.getTile(target));
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard ;
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int score = 0;
		List<int> neighbours = GameView.instance.getEveryone();
		int bonus = Mathf.RoundToInt((6f+s.Power)/2f);

		for(int i = 0 ; i < neighbours.Count ; i++){
			targetCard = GameView.instance.getCard(neighbours[i]);
			if(targetCard.CardType.Id!=5 && targetCard.CardType.Id!=8){
				if(targetCard.isMine){
					score-=Mathf.RoundToInt((100f-targetCard.getMagicalEsquive())*(bonus*0.02f));
				}
				else{
					score+=Mathf.RoundToInt((100f-targetCard.getMagicalEsquive())*(bonus*0.02f));
				}
			}
		}

		score = score * GameView.instance.IA.getSoutienFactor() ;
		return score ;
	}
}
