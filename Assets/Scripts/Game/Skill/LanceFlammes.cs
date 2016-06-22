using UnityEngine;
using System.Collections.Generic;

public class LanceFlammes : GameSkill
{
	public LanceFlammes()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.ciblage = -2 ;
		base.auto = false;
		base.id = 27 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameView.instance.setHoveringZone(2, "LanceFlammes", "");
	}
	
	public override void resolve(List<Tile> targetsTile)
	{	
		GameController.instance.play(this.id);
		GameCard currentCard = GameView.instance.getCurrentCard();
		Tile currentTile = GameView.instance.getTile(GameView.instance.getCurrentPlayingCard());
		Tile tile = targetsTile[0];
		int playerID ;
		int level = GameView.instance.getCurrentSkill().Power;
		int minDamages ;
		int maxDamages ;
		GameCard targetCard ;
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);

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
							minDamages = currentCard.getNormalDamagesAgainst(targetCard, 2+level);
							maxDamages = currentCard.getNormalDamagesAgainst(targetCard, 10+level);
							if(currentCard.isFou()){
								minDamages = Mathf.RoundToInt(1.25f*minDamages);
								maxDamages = Mathf.RoundToInt(1.25f*maxDamages);
							}
							GameController.instance.applyOn2(playerID, Random.Range(minDamages,maxDamages+1));
						}
						else{
							GameController.instance.esquive(playerID,this.getText(0));
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
							minDamages = currentCard.getNormalDamagesAgainst(targetCard, 2+level);
							maxDamages = currentCard.getNormalDamagesAgainst(targetCard, 10+level);
							if(currentCard.isFou()){
								minDamages = Mathf.RoundToInt(1.25f*minDamages);
								maxDamages = Mathf.RoundToInt(1.25f*maxDamages);
							}
							GameController.instance.applyOn2(playerID, Random.Range(minDamages,maxDamages+1));
						}
						else{
							GameController.instance.esquive(playerID,this.getText(0));
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
							minDamages = currentCard.getNormalDamagesAgainst(targetCard, 2+level);
							maxDamages = currentCard.getNormalDamagesAgainst(targetCard, 10+level);
							if(currentCard.isFou()){
								minDamages = Mathf.RoundToInt(1.25f*minDamages);
								maxDamages = Mathf.RoundToInt(1.25f*maxDamages);
							}
							GameController.instance.applyOn2(playerID, Random.Range(minDamages,maxDamages+1));
						}
						else{
							GameController.instance.esquive(playerID,this.getText(0));
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
							minDamages = currentCard.getNormalDamagesAgainst(targetCard, 2+level);
							maxDamages = currentCard.getNormalDamagesAgainst(targetCard, 10+level);

							if(currentCard.isFou()){
								minDamages = Mathf.RoundToInt(1.25f*minDamages);
								maxDamages = Mathf.RoundToInt(1.25f*maxDamages);
							}
							GameController.instance.applyOn2(playerID, Random.Range(minDamages,maxDamages+1));
						}
						else{
							GameController.instance.esquive(playerID,this.getText(0));
						}
					}
				}
			}
		}
		GameController.instance.playSound(36);

		if(currentCard.isFou()){
			GameController.instance.applyOnMe(1);
		}
		else{
			GameController.instance.applyOnMe(-1);
		}
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int value){
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(value, -1, 27, this.getText(0), value+" dégats subis"), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.displaySkillEffect(target, "-"+value+"PV", 0);	
		GameView.instance.addAnim(5,GameView.instance.getTile(target));
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;

		int damages = 2+level;
		if(currentCard.isFou()){
			damages = Mathf.RoundToInt(1.25f*damages);
		}
		int minDamages = currentCard.getNormalDamagesAgainst(targetCard, damages);
		damages = 10+level;
		if(currentCard.isFou()){
			damages = Mathf.RoundToInt(1.25f*damages);
		}
		int maxDamages = currentCard.getNormalDamagesAgainst(targetCard, damages);

		string text = "PV : "+targetCard.getLife()+" -> ["+(targetCard.getLife()-minDamages)+"-"+(targetCard.getLife()-maxDamages)+"]";
		
		int amount = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		if(value==1){
			int myLevel = GameView.instance.getCurrentCard().Skills[0].Power;
			GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addDamagesModifyer(new Modifyer((11-myLevel), -1, 24, this.getText(0), (10-myLevel)+" dégats subis"), true,-1);
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0)+"\nFou\n-"+(11-myLevel)+"PV", 0);
		}
		else{
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0), 1);
		}
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		float bestScore = 0 ;
		float score ;
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard;
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int levelMin ;
		int levelMax ;
		int playerID ;

		score = 0f;
		for(int i = t.y-1 ; i>=0 ; i--){
			playerID = GameView.instance.getTileController(new Tile(t.x, i)).getCharacterID();
			if(playerID != -1){
				targetCard = GameView.instance.getCard(playerID);
				levelMin = currentCard.getNormalDamagesAgainst(targetCard, 2+s.Power);
				levelMax = currentCard.getNormalDamagesAgainst(targetCard, 10+s.Power);
				if(currentCard.isFou()){
					levelMin = Mathf.RoundToInt(1.25f*levelMin);
					levelMax = Mathf.RoundToInt(1.25f*levelMax);
				}
				if(targetCard.isMine){
					score+=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
				else{
					score-=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
			}
		}
		if(score>bestScore){
			bestScore = score ;
		}
		score = 0f;
		for(int i = t.y+1 ; i<GameView.instance.boardHeight ; i++){
			playerID = GameView.instance.getTileController(new Tile(t.x, i)).getCharacterID();
			if(playerID != -1){
				targetCard = GameView.instance.getCard(playerID);
				levelMin = currentCard.getNormalDamagesAgainst(targetCard, 2+s.Power);
				levelMax = currentCard.getNormalDamagesAgainst(targetCard, 10*s.Power);
				if(currentCard.isFou()){
					levelMin = Mathf.RoundToInt(1.25f*levelMin);
					levelMax = Mathf.RoundToInt(1.25f*levelMax);
				}
				if(targetCard.isMine){
					score+=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
				else{
					score-=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
			}
		}
		if(score>bestScore){
			bestScore = score ;
		}
		score = 0f;
		for(int i = t.x-1 ; i>=0 ; i--){
			playerID = GameView.instance.getTileController(new Tile(i, t.y)).getCharacterID();
			if(playerID != -1){
				targetCard = GameView.instance.getCard(playerID);
				levelMin = currentCard.getNormalDamagesAgainst(targetCard, 2+s.Power);
				levelMax = currentCard.getNormalDamagesAgainst(targetCard, 10+s.Power);
				if(currentCard.isFou()){
					levelMin = Mathf.RoundToInt(1.25f*levelMin);
					levelMax = Mathf.RoundToInt(1.25f*levelMax);
				}
				if(targetCard.isMine){
					score+=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
				else{
					score-=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
			}
		}
		if(score>bestScore){
			bestScore = score ;
		}
		score = 0f;
		for(int i = t.x+1 ; i<GameView.instance.boardWidth ; i++){
			playerID = GameView.instance.getTileController(new Tile(i, t.y)).getCharacterID();
			if(playerID != -1){
				targetCard = GameView.instance.getCard(playerID);
				levelMin = currentCard.getNormalDamagesAgainst(targetCard, 2+s.Power);
				levelMax = currentCard.getNormalDamagesAgainst(targetCard, 10+s.Power);
				if(currentCard.isFou()){
					levelMin = Mathf.RoundToInt(1.25f*levelMin);
					levelMax = Mathf.RoundToInt(1.25f*levelMax);
				}
				if(targetCard.isMine){
					score+=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
				else{
					score-=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
			}
		}
		if(score>bestScore){
			bestScore = score ;
		}

		if(currentCard.isFou()){
			int damages = 11-currentCard.Skills[0].Power;
			if(damages>=currentCard.getLife()){
				score-=100;
			}
			else{
				score-=damages;
			}
		}

		bestScore = bestScore * GameView.instance.IA.getAgressiveFactor() ;

		return (int)bestScore ;
	}

	public override int getBestChoice(Tile t, Skill s){
		float bestScore = -200 ;
		float score ;
		int bestResult =-1;
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard;
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int levelMin ;
		int levelMax ;
		int playerID ;

		score = 0f;
		for(int i = t.y-1 ; i>=0 ; i--){
			playerID = GameView.instance.getTileController(new Tile(t.x, i)).getCharacterID();
			if(playerID != -1){
				targetCard = GameView.instance.getCard(playerID);
				levelMin = currentCard.getNormalDamagesAgainst(targetCard, 2+s.Power);
				levelMax = currentCard.getNormalDamagesAgainst(targetCard, 10+s.Power);
				if(currentCard.isFou()){
					levelMin = Mathf.RoundToInt(1.25f*levelMin);
					levelMax = Mathf.RoundToInt(1.25f*levelMax);
				}
				if(targetCard.isMine){
					score+=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
				else{
					score-=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
			}
		}
		if(score>bestScore){
			bestScore = score ;
			bestResult = 0 ;
		}
		score = 0f;
		for(int i = t.y+1 ; i<GameView.instance.boardHeight ; i++){
			playerID = GameView.instance.getTileController(new Tile(t.x, i)).getCharacterID();
			if(playerID != -1){
				targetCard = GameView.instance.getCard(playerID);
				levelMin = currentCard.getNormalDamagesAgainst(targetCard, 2+s.Power);
				levelMax = currentCard.getNormalDamagesAgainst(targetCard, 10+s.Power);
				if(currentCard.isFou()){
					levelMin = Mathf.RoundToInt(1.25f*levelMin);
					levelMax = Mathf.RoundToInt(1.25f*levelMax);
				}
				if(targetCard.isMine){
					score+=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
				else{
					score-=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
			}
		}
		if(score>bestScore){
			bestScore = score ;
			bestResult = 1 ;
		}
		score = 0f;
		for(int i = t.x-1 ; i>=0 ; i--){
			playerID = GameView.instance.getTileController(new Tile(i, t.y)).getCharacterID();
			if(playerID != -1){
				targetCard = GameView.instance.getCard(playerID);
				levelMin = currentCard.getNormalDamagesAgainst(targetCard, 2+s.Power);
				levelMax = currentCard.getNormalDamagesAgainst(targetCard, 10+s.Power);
				if(currentCard.isFou()){
					levelMin = Mathf.RoundToInt(1.25f*levelMin);
					levelMax = Mathf.RoundToInt(1.25f*levelMax);
				}
				if(targetCard.isMine){
					score+=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
				else{
					score-=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
			}
		}
		if(score>bestScore){
			bestScore = score ;
			bestResult = 2 ;
		}
		score = 0f;
		for(int i = t.x+1 ; i<GameView.instance.boardWidth ; i++){
			playerID = GameView.instance.getTileController(new Tile(i, t.y)).getCharacterID();
			if(playerID != -1){
				targetCard = GameView.instance.getCard(playerID);
				levelMin = currentCard.getNormalDamagesAgainst(targetCard, 2+s.Power);
				levelMax = currentCard.getNormalDamagesAgainst(targetCard, 10+s.Power);
				if(currentCard.isFou()){
					levelMin = Mathf.RoundToInt(1.25f*levelMin);
					levelMax = Mathf.RoundToInt(1.25f*levelMax);
				}
				if(targetCard.isMine){
					score+=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
				else{
					score-=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
			}
		}
		if(score>bestScore){
			bestScore = score ;
			bestResult = 3 ;
		}

		return bestResult ;
	}
}
