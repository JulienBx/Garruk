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
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name, GameView.instance.getCurrentSkill().Description);
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int proba = GameView.instance.getCurrentSkill().proba;

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
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int percentage = Mathf.RoundToInt(currentCard.getAttack()*(GameView.instance.getCurrentSkill().Power*5f+50f)/100f);
		int damages = currentCard.getNormalDamagesAgainst(targetCard, percentage);
		
		GameView.instance.displaySkillEffect(target, "-"+damages+"PV", 0);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,1,base.name,damages+" dégats subis"));
		GameView.instance.addAnim(GameView.instance.getTile(target), 17);
	}
}
