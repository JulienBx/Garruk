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
		if(currentCard.isSniper()){
			proba = 100 ;
		}
		int isFou = 1 ;
		if(currentCard.isFou()){
			if(Random.Range(1,101)<26){
				isFou = -1 ;
				List<Tile> potentialTiles = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()).getImmediateNeighbourTiles() ;
				List<Tile> potentialTargets = new List<Tile>();
				for (int i = 0 ; i < potentialTiles.Count ; i++){
					if(tile.x!=potentialTiles[i].x || tile.y!=potentialTiles[i].y){
						potentialTargets.Add(potentialTiles[i]);
					}
				}
				tile = potentialTargets[Random.Range(0,potentialTargets.Count)];
			}
		}

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
							minDamages = currentCard.getMagicalDamagesAgainst(targetCard, level);
							maxDamages = currentCard.getMagicalDamagesAgainst(targetCard, 2+2*level);
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
							minDamages = currentCard.getMagicalDamagesAgainst(targetCard, level);
							maxDamages = currentCard.getMagicalDamagesAgainst(targetCard, 2+2*level);
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
							minDamages = currentCard.getMagicalDamagesAgainst(targetCard, level);
							maxDamages = currentCard.getMagicalDamagesAgainst(targetCard, 2+2*level);
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
							minDamages = currentCard.getMagicalDamagesAgainst(targetCard, level);
							maxDamages = currentCard.getMagicalDamagesAgainst(targetCard, 2+2*level);
							GameController.instance.applyOn2(playerID, Random.Range(minDamages,maxDamages+1));
						}
						else{
							GameController.instance.esquive(playerID,base.name);
						}
					}
				}
			}
		}
		GameController.instance.applyOn(isFou);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int value){
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(value, -1, 27, base.name, value+" dégats subis"));
		GameView.instance.displaySkillEffect(target, "-"+value+"PV", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 27);
	}

	public override void applyOn(int target){
		if(target==-1){
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), "Fou!\nse trompe de cible!", 0);	
			GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 23);
		}
		else{
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);	
			GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 23);
		}
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int minDamages = currentCard.getMagicalDamagesAgainst(targetCard, level);
		int maxDamages = currentCard.getMagicalDamagesAgainst(targetCard, 2+2*level);

		string text = "PV : "+targetCard.getLife()+" -> ["+(targetCard.getLife()-minDamages)+"-"+(targetCard.getLife()-maxDamages)+"]";
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
