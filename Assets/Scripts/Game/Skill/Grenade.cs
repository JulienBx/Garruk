using UnityEngine;
using System.Collections.Generic;

public class Grenade : GameSkill
{
	public Grenade()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Grenade";
		base.ciblage = -2 ;
		base.auto = true;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameView.instance.setHoveringZone(1, "Grenade", "");
	}
	
	public override void resolve(List<Tile> targetsTile)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		GameCard targetCard ;
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = GameView.instance.getCurrentSkill().proba;
		int level = GameView.instance.getCurrentSkill().Power;

		int minDamages;
		int maxDamages;
		int playerID;
	
		playerID = GameView.instance.getTileController(new Tile(targetsTile[0].x, targetsTile[0].y)).getCharacterID();
		if(playerID!=-1){
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
		if(targetsTile[0].x<GameView.instance.boardWidth-1){
			playerID = GameView.instance.getTileController(new Tile(targetsTile[0].x+1, targetsTile[0].y)).getCharacterID();
			if(playerID!=-1){
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
		if(targetsTile[0].x>0){
			playerID = GameView.instance.getTileController(new Tile(targetsTile[0].x-1, targetsTile[0].y)).getCharacterID();
			if(playerID!=-1){
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
		if(targetsTile[0].y<GameView.instance.boardHeight-1){
			playerID = GameView.instance.getTileController(new Tile(targetsTile[0].x, targetsTile[0].y+1)).getCharacterID();
			if(playerID!=-1){
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
		if(targetsTile[0].y>0){
			playerID = GameView.instance.getTileController(new Tile(targetsTile[0].x, targetsTile[0].y-1)).getCharacterID();
			if(playerID!=-1){
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

		GameController.instance.endPlay();
		int myLevel = currentCard.Skills[0].Power;
		if(currentCard.isFou()){
			GameController.instance.launchFou(23,GameView.instance.getCurrentPlayingCard());
		}
	}

	public override void launchFou(int c){
		int myLevel = GameView.instance.getCard(c).Skills[0].Power;
		GameView.instance.getPlayingCardController(c).addDamagesModifyer(new Modifyer((10-myLevel), -1, 24, base.name, (10-myLevel)+" dégats subis"));
		GameView.instance.displaySkillEffect(c, base.name+"\n-"+(10-myLevel)+"PV", 0);
	}

	public override void applyOn(int target, int value){
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(value, -1, 23, base.name, value+" dégats subis"));
		GameView.instance.displaySkillEffect(target, base.name+"\n-"+value+"PV", 0);	
		GameView.instance.addAnim(GameView.instance.getTile(target), 23);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int minDamages, maxDamages;
		int damages = 5+level;
		if(currentCard.isFou()){
			damages = Mathf.RoundToInt(1.25f*damages);
		}
		minDamages = currentCard.getMagicalDamagesAgainst(targetCard, damages);
		damages = 10+3*level;
		if(currentCard.isFou()){
			damages = Mathf.RoundToInt(1.25f*damages);
		}
		maxDamages = currentCard.getMagicalDamagesAgainst(targetCard, damages);

		string text = "PV : "+targetCard.getLife()+" -> ["+(targetCard.getLife()-minDamages)+"-"+(targetCard.getLife()-maxDamages)+"]";
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}
