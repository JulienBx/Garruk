using UnityEngine;
using System.Collections.Generic;

public class LanceFlammes : GameSkill
{
	public LanceFlammes()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Lanceflammes";
		base.ciblage = -2 ;
		base.auto = true;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameView.instance.setHoveringZone(2, "LanceFlammes", "");
	}
	
	public override void resolve(List<Tile> targetsTile)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		GameCard currentCard = GameView.instance.getCurrentCard();
		Tile currentTile = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard());
		Tile tile = targetsTile[0];
		int playerID ;
		int level = GameView.instance.getCurrentSkill().Power;
		int minDamages ;
		int maxDamages ;
		GameCard targetCard ;
		int proba = GameView.instance.getCurrentSkill().proba;

		if(tile.x==currentTile.x){
			if(tile.y<currentTile.y){
				for(int i = currentTile.y-1 ; i>=0 ; i--){
					playerID = GameView.instance.getTileController(new Tile(tile.x, i)).getCharacterID();
					if(playerID != -1){
						if (Random.Range(1,101) <= GameView.instance.getCard(playerID).getMagicalEsquive()){
							GameController.instance.esquive(playerID,1);
						}
						else if (Random.Range(1,101) <= proba){
							targetCard = GameView.instance.getCard(playerID);
							minDamages = currentCard.getMagicalDamagesAgainst(targetCard, 5+level);
							maxDamages = currentCard.getMagicalDamagesAgainst(targetCard, 10+3*level);
							if(currentCard.isFou()){
								minDamages = Mathf.RoundToInt(1.25f*minDamages);
								maxDamages = Mathf.RoundToInt(1.25f*maxDamages);
							}
							GameController.instance.applyOn2(playerID, Random.Range(minDamages,maxDamages+1));
						}
						else{
							GameController.instance.esquive(playerID,base.name);
						}
					}
				}
			}
			else if(tile.y>currentTile.y){
				for(int i = currentTile.y+1 ; i<GameView.instance.boardHeight ; i++){
					playerID = GameView.instance.getTileController(new Tile(tile.x, i)).getCharacterID();
					if(playerID != -1){
						if (Random.Range(1,101) <= GameView.instance.getCard(playerID).getMagicalEsquive()){
							GameController.instance.esquive(playerID,1);
						}
						else if (Random.Range(1,101) <= proba){
							targetCard = GameView.instance.getCard(playerID);
							minDamages = currentCard.getMagicalDamagesAgainst(targetCard, 5+level);
							maxDamages = currentCard.getMagicalDamagesAgainst(targetCard, 10+3*level);
							if(currentCard.isFou()){
								minDamages = Mathf.RoundToInt(1.25f*minDamages);
								maxDamages = Mathf.RoundToInt(1.25f*maxDamages);
							}
							GameController.instance.applyOn2(playerID, Random.Range(minDamages,maxDamages+1));
						}
						else{
							GameController.instance.esquive(playerID,base.name);
						}
					}
				}
			}
		}
		else if(tile.y==currentTile.y){
			if(tile.x<currentTile.x){
				for(int i = currentTile.x-1 ; i>=0 ; i--){
					playerID = GameView.instance.getTileController(new Tile(i, tile.y)).getCharacterID();
					if(playerID != -1){
						if (Random.Range(1,101) <= GameView.instance.getCard(playerID).getMagicalEsquive()){
							GameController.instance.esquive(playerID,1);
						}
						else if (Random.Range(1,101) <= proba){
							targetCard = GameView.instance.getCard(playerID);
							minDamages = currentCard.getMagicalDamagesAgainst(targetCard, 5+level);
							maxDamages = currentCard.getMagicalDamagesAgainst(targetCard, 10+3*level);
							if(currentCard.isFou()){
								minDamages = Mathf.RoundToInt(1.25f*minDamages);
								maxDamages = Mathf.RoundToInt(1.25f*maxDamages);
							}
							GameController.instance.applyOn2(playerID, Random.Range(minDamages,maxDamages+1));
						}
						else{
							GameController.instance.esquive(playerID,base.name);
						}
					}
				}
			}
			else if(tile.x>currentTile.x){
				for(int i = currentTile.x+1 ; i<GameView.instance.boardWidth ; i++){
					playerID = GameView.instance.getTileController(new Tile(i, tile.y)).getCharacterID();
					if(playerID != -1){
						if (Random.Range(1,101) <= GameView.instance.getCard(playerID).getMagicalEsquive()){
							GameController.instance.esquive(playerID,1);
						}
						else if (Random.Range(1,101) <= proba){
							targetCard = GameView.instance.getCard(playerID);
							minDamages = currentCard.getMagicalDamagesAgainst(targetCard, 5+level);
							maxDamages = currentCard.getMagicalDamagesAgainst(targetCard, 10+3*level);

							if(currentCard.isFou()){
								minDamages = Mathf.RoundToInt(1.25f*minDamages);
								maxDamages = Mathf.RoundToInt(1.25f*maxDamages);
							}
							GameController.instance.applyOn2(playerID, Random.Range(minDamages,maxDamages+1));
						}
						else{
							GameController.instance.esquive(playerID,base.name);
						}
					}
				}
			}
		}
		if(currentCard.isFou()){
			GameController.instance.applyOnMe(1);
		}
		else{
			GameController.instance.applyOnMe(-1);
		}
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int value){
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(value, -1, 27, base.name, value+" dégats subis"), false);
		GameView.instance.displaySkillEffect(target, "-"+value+"PV", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 27);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;

		int damages = 5+level;
		if(currentCard.isFou()){
			damages = Mathf.RoundToInt(1.25f*damages);
		}
		int minDamages = currentCard.getMagicalDamagesAgainst(targetCard, damages);
		damages = 10+3*level;
		if(currentCard.isFou()){
			damages = Mathf.RoundToInt(1.25f*damages);
		}
		int maxDamages = currentCard.getMagicalDamagesAgainst(targetCard, damages);

		string text = "PV : "+targetCard.getLife()+" -> ["+(targetCard.getLife()-minDamages)+"-"+(targetCard.getLife()-maxDamages)+"]";
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		if(value==1){
			int myLevel = GameView.instance.getCurrentCard().Skills[0].Power;
			GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addDamagesModifyer(new Modifyer((11-myLevel), -1, 24, base.name, (10-myLevel)+" dégats subis"), false);
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name+"\nFou\n-"+(11-myLevel)+"PV", 0);
		}
		else{
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		}
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}
}
