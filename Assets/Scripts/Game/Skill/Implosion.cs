using UnityEngine;
using System.Collections.Generic;

public class Implosion : GameSkill
{
	public Implosion()
	{
		this.numberOfExpectedTargets = 0 ;
		base.name = "Implosion";
		base.ciblage = -1 ;
		base.auto = true;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name, GameView.instance.getCurrentSkill().Description);
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = GameView.instance.getCurrentSkill().proba;
		List<Tile> tempTiles;
		Tile t = GameView.instance.getPlayingCardTile(GameView.instance.getCurrentPlayingCard());
		tempTiles = t.getImmediateNeighbourTiles();
		GameCard targetCard ;

		int i = 0 ;
		int tempInt ;
		int minDamages ;
		int maxDamages ;
		
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
							targetCard = GameView.instance.getCard(tempInt);
							minDamages = currentCard.getNormalDamagesAgainst(targetCard, 5+2*GameView.instance.getCurrentSkill().Power);
							maxDamages = currentCard.getNormalDamagesAgainst(targetCard, 20+3*GameView.instance.getCurrentSkill().Power);
							if(currentCard.isFou()){
								maxDamages = Mathf.RoundToInt(1.25f*maxDamages);
							}
							GameController.instance.applyOn2(tempInt, Random.Range(minDamages, maxDamages+1));
						}
						else{
							GameController.instance.esquive(tempInt,base.name);
						}
					}
				}
			}
			i++;
		}
		GameController.instance.applyOn2(GameView.instance.getCurrentPlayingCard(), currentCard.getLife());
		GameController.instance.endPlay();
		int myLevel = currentCard.Skills[0].Power;
		if(currentCard.isFou()){
			GameController.instance.launchFou(28,GameView.instance.getCurrentPlayingCard());
		}
	}

	public override void launchFou(int c){
		int myLevel = GameView.instance.getCard(c).Skills[0].Power;
		GameView.instance.getPlayingCardController(c).addDamagesModifyer(new Modifyer((10-myLevel), -1, 24, base.name, (10-myLevel)+" dégats subis"));
		GameView.instance.displaySkillEffect(c, base.name+"\n-"+(10-myLevel)+"PV", 0);
	}
	
	public override void applyOn(int target, int value){
		GameView.instance.displaySkillEffect(target, base.name+"\n-"+value+"PV", 0);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(value,-1,28,base.name,value+" dégats subis"));
		GameView.instance.addAnim(GameView.instance.getTile(target), 28);
	}
}
