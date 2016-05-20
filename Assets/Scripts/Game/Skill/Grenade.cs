using UnityEngine;
using System.Collections.Generic;

public class Grenade : GameSkill
{
	public Grenade()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Grenade";
		base.ciblage = -2 ;
		base.auto = false;
		base.id = 23 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameView.instance.setHoveringZone(1, "Grenade", "");
	}
	
	public override void resolve(List<Tile> targetsTile)
	{	
		GameController.instance.play(this.id);
		GameCard targetCard ;
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int level = GameView.instance.getCurrentSkill().Power;
		bool hasFoundMe = false ;
		int minDamages;
		int maxDamages;
		int playerID;
	
		playerID = GameView.instance.getTileController(new Tile(targetsTile[0].x, targetsTile[0].y)).getCharacterID();
		if(playerID!=-1){
			if(GameView.instance.getCurrentPlayingCard()==playerID){
				hasFoundMe = true ;
			}
			if (Random.Range(1,101) <= GameView.instance.getCard(playerID).getMagicalEsquive()){
				GameController.instance.esquive(playerID,1);
			}
			else if (Random.Range(1,101) <= proba){
				targetCard = GameView.instance.getCard(playerID);
				minDamages = currentCard.getNormalDamagesAgainst(targetCard, 4+level);
				maxDamages = currentCard.getNormalDamagesAgainst(targetCard, 9+level);
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
				if(GameView.instance.getCurrentPlayingCard()==playerID){
					hasFoundMe = true ;
				}
				if (Random.Range(1,101) <= GameView.instance.getCard(playerID).getMagicalEsquive()){
					GameController.instance.esquive(playerID,1);
				}
				else if (Random.Range(1,101) <= proba){
					targetCard = GameView.instance.getCard(playerID);
					minDamages = currentCard.getNormalDamagesAgainst(targetCard, 4+level);
					maxDamages = currentCard.getNormalDamagesAgainst(targetCard, 9*level);
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
				if(GameView.instance.getCurrentPlayingCard()==playerID){
					hasFoundMe = true ;
				}
				if (Random.Range(1,101) <= GameView.instance.getCard(playerID).getMagicalEsquive()){
					GameController.instance.esquive(playerID,1);
				}
				else if (Random.Range(1,101) <= proba){
					targetCard = GameView.instance.getCard(playerID);
					minDamages = currentCard.getNormalDamagesAgainst(targetCard, 4+level);
					maxDamages = currentCard.getNormalDamagesAgainst(targetCard, 9+level);
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
				if(GameView.instance.getCurrentPlayingCard()==playerID){
					hasFoundMe = true ;
				}
				if (Random.Range(1,101) <= GameView.instance.getCard(playerID).getMagicalEsquive()){
					GameController.instance.esquive(playerID,1);
				}
				else if (Random.Range(1,101) <= proba){
					targetCard = GameView.instance.getCard(playerID);
					minDamages = currentCard.getNormalDamagesAgainst(targetCard, 4+level);
					maxDamages = currentCard.getNormalDamagesAgainst(targetCard, 9+level);
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
				if(GameView.instance.getCurrentPlayingCard()==playerID){
					hasFoundMe = true ;
				}
				if (Random.Range(1,101) <= GameView.instance.getCard(playerID).getMagicalEsquive()){
					GameController.instance.esquive(playerID,1);
				}
				else if (Random.Range(1,101) <= proba){
					targetCard = GameView.instance.getCard(playerID);
					minDamages = currentCard.getNormalDamagesAgainst(targetCard, 4+level);
					maxDamages = currentCard.getNormalDamagesAgainst(targetCard, 9+level);
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
		GameController.instance.playSound(35);
		if(currentCard.isFou()){
			if(!hasFoundMe){
				GameController.instance.applyOnMe(1);
			}
		}
		else{
			if(!hasFoundMe){
				GameController.instance.applyOnMe(-1);
			}
		}
		GameController.instance.endPlay();
	}

	public override void applyOn(int target, int value){
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(value, -1, 23, base.name, value+" dégats subis"), (target==GameView.instance.getCurrentPlayingCard()), GameView.instance.getCurrentPlayingCard());
		GameView.instance.displaySkillEffect(target, "-"+value+"PV", 0);	
		GameView.instance.addAnim(6,GameView.instance.getTile(target));
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int minDamages, maxDamages;
		int damages = 4+level;
		if(currentCard.isFou()){
			damages = Mathf.RoundToInt(1.25f*damages);
		}
		minDamages = currentCard.getNormalDamagesAgainst(targetCard, damages);
		damages = 9+level;
		if(currentCard.isFou()){
			damages = Mathf.RoundToInt(1.25f*damages);
		}
		maxDamages = currentCard.getNormalDamagesAgainst(targetCard, damages);

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
			GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addDamagesModifyer(new Modifyer((11-myLevel), -1, 24, base.name, (10-myLevel)+" dégats subis"), true,-1);
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name+"\nFou\n-"+(11-myLevel)+"PV", 0);
		}
		else{
			GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		}
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		float score = 0 ;

		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard;
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int levelMin ;
		int levelMax ;

		int playerID = GameView.instance.getTileController(t).getCharacterID();
		if(playerID!=-1){
			targetCard = GameView.instance.getCard(playerID);
			levelMin = currentCard.getNormalDamagesAgainst(targetCard,4+s.Power)+Mathf.RoundToInt(5-targetCard.getLife()/10f);;
			levelMax = currentCard.getNormalDamagesAgainst(targetCard,9+s.Power)+Mathf.RoundToInt(5-targetCard.getLife()/10f);;
			if(targetCard.isMine){
				score+=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
			}
			else{
				score-=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
			}
		}
		if(t.x<GameView.instance.boardWidth-1){
			playerID = GameView.instance.getTileController(new Tile(t.x+1, t.y)).getCharacterID();
			if(playerID!=-1){
				targetCard = GameView.instance.getCard(playerID);
				levelMin = currentCard.getNormalDamagesAgainst(targetCard,4+s.Power)+Mathf.RoundToInt(5-targetCard.getLife()/10f);;
				levelMax = currentCard.getNormalDamagesAgainst(targetCard,9+s.Power)+Mathf.RoundToInt(5-targetCard.getLife()/10f);;

				if(targetCard.isMine){
					score+=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
				else{
					score-=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
			}
		}
		if(t.x>0){
			playerID = GameView.instance.getTileController(new Tile(t.x-1, t.y)).getCharacterID();
			if(playerID!=-1){
				targetCard = GameView.instance.getCard(playerID);
				levelMin = currentCard.getNormalDamagesAgainst(targetCard,4+s.Power)+Mathf.RoundToInt(5-targetCard.getLife()/10f);;
				levelMax = currentCard.getNormalDamagesAgainst(targetCard,9+s.Power)+Mathf.RoundToInt(5-targetCard.getLife()/10f);;

				if(targetCard.isMine){
					score+=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
				else{
					score-=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
			}
		}
		if(t.y<GameView.instance.boardHeight-1){
			playerID = GameView.instance.getTileController(new Tile(t.x, t.y+1)).getCharacterID();
			if(playerID!=-1){
				targetCard = GameView.instance.getCard(playerID);
				levelMin = currentCard.getNormalDamagesAgainst(targetCard,4+s.Power)+Mathf.RoundToInt(5-targetCard.getLife()/10f);;
				levelMax = currentCard.getNormalDamagesAgainst(targetCard,9+s.Power)+Mathf.RoundToInt(5-targetCard.getLife()/10f);;

				if(targetCard.isMine){
					score+=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
				else{
					score-=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
			}
		}
		if(t.y>0){
			playerID = GameView.instance.getTileController(new Tile(t.x, t.y-1)).getCharacterID();
			if(playerID!=-1){
				targetCard = GameView.instance.getCard(playerID);
				levelMin = currentCard.getNormalDamagesAgainst(targetCard,4+s.Power)+Mathf.RoundToInt(5-targetCard.getLife()/10f);;
				levelMax = currentCard.getNormalDamagesAgainst(targetCard,9+s.Power)+Mathf.RoundToInt(5-targetCard.getLife()/10f);;

				if(targetCard.isMine){
					score+=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
				else{
					score-=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
			}
		}
		score = score * GameView.instance.IA.getAgressiveFactor() ;

		return (int)score ;
	}
}
