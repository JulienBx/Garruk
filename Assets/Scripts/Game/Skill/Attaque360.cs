using UnityEngine;
using System.Collections.Generic;

public class Attaque360 : GameSkill
{
	public Attaque360()
	{
		this.numberOfExpectedTargets = 0 ;
		base.name = "Attaque 360";
		base.ciblage = 1 ;
		base.auto = true;
		base.id = 17 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name, GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
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
							GameController.instance.esquive(tempInt,base.name);
						}
					}
				}
			}
			i++;
		}
		SoundController.instance.playSound(25);

		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int percentage = Mathf.RoundToInt(currentCard.getAttack()*(GameView.instance.getCurrentSkill().Power*5f+50f)/100f);
		int damages = currentCard.getNormalDamagesAgainst(targetCard, percentage);
		
		GameView.instance.displaySkillEffect(target, "-"+damages+"PV", 0);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,17,base.name,damages+" dégats subis"), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.addAnim(3,GameView.instance.getTile(target));
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		int score = 0 ;
		GameCard targetCard ;
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages;

		List<Tile> neighbours = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()).getImmediateNeighbourTiles();
		for(int i = 0 ; i < neighbours.Count ; i++){
			if(GameView.instance.getTileCharacterID(neighbours[i].x,neighbours[i].y)!=-1){
				targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(neighbours[i].x,neighbours[i].y));
				damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(0.5f+0.05f*s.Power)));
				if(targetCard.isMine){
					if(damages>=targetCard.getLife()){
						score+=200;
					}
					else{
						score+=Mathf.RoundToInt(((100-targetCard.getEsquive())/100f)*(damages+Mathf.Max(0,30-(targetCard.getLife()-damages))));
					}
				}
				else{
					if(damages>=targetCard.getLife()){
						score-=200;
					}
					else{
						score-=Mathf.RoundToInt(((100-targetCard.getEsquive())/100f)*(damages+Mathf.Max(0,30-(targetCard.getLife()-damages))));
					}
				}
			}
		}

		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
