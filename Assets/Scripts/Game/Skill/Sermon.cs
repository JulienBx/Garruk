using UnityEngine;
using System.Collections.Generic;

public class Sermon : GameSkill
{
	public Sermon()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Sermon";
		base.ciblage = 3 ;
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}

	public override void resolve(List<int> targetsPCC)
	{	
		bool isSuccess = false ;
		GameController.instance.play(GameView.instance.runningSkill);
		
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
						GameController.instance.applyOn(tempInt);
						isSuccess = true ;
					}
				}
			}
			i++;
		}
		//GameController.instance.showResult(isSuccess);
		GameController.instance.endPlay();
	}

	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();

		GameView.instance.getCard(target).setState(new Modifyer(0, 1, 102, base.name, "Fortifié. ATK augmentée de 2"));
		GameView.instance.displaySkillEffect(target, "Fortifé!", 0);
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.addAnim(GameView.instance.getTile(target), 102);
	}	

	public override string getTargetText(int target){
		
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();

		string text = "Fortifié";
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}



