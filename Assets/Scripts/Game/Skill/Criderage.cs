using UnityEngine;
using System.Collections.Generic;

public class Criderage : GameSkill
{
	public Criderage()
	{
		this.numberOfExpectedTargets = 0 ;
		base.ciblage = 2 ;
		base.auto = true;
		base.id = 19 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(this.getText(0), GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targets)
	{	
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);

		List<Tile> tempTiles;
		Tile t = GameView.instance.getPlayingCardTile(GameView.instance.getCurrentPlayingCard());
		tempTiles = t.getImmediateNeighbourTiles();
		
		int i = 0 ;
		int tempInt ; 
		
		while (i<tempTiles.Count){
			t = tempTiles[i];
			tempInt = GameView.instance.getTileCharacterID(t.x, t.y);
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
							GameController.instance.esquive(tempInt,this.getText(0));
						}
					}
				}
			}
			i++;
		}
		GameController.instance.playSound(37);
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		int level = GameView.instance.getCurrentSkill().Power+2;
		GameCard targetCard = GameView.instance.getCard(target);

		GameView.instance.getPlayingCardController(target).updateAttack(targetCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(level, 1, 19, this.getText(0), ". Actif 1 tour"));
		GameView.instance.displaySkillEffect(target, "+"+level+"ATK", 2);
		GameView.instance.addAnim(7,GameView.instance.getTile(target));
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0), 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard ;
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int score = 0;

		List<Tile> neighbours = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()).getImmediateNeighbourTiles();
		for(int i = 0 ; i < neighbours.Count ; i++){
			if(GameView.instance.getTileCharacterID(neighbours[i].x, neighbours[i].y)!=-1){
				targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(neighbours[i].x, neighbours[i].y));
				if(targetCard.isMine){
					score-=Mathf.RoundToInt(((100f-targetCard.getEsquive())/100f)*(s.Power+2));
				}
				else{
					score+=Mathf.RoundToInt(((100f-targetCard.getEsquive())/100f)*(s.Power+2));
				}
			}
		}

		score = score * GameView.instance.IA.getSoutienFactor() ;

		return score ;
	}
}
