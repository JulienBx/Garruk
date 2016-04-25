using UnityEngine;
using System.Collections.Generic;

public class PluieBleue : GameSkill
{
	public PluieBleue()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Pluie Bleue";
		base.ciblage = -2 ;
		base.auto = true;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameView.instance.setHoveringZone(1, "Pluie Bleue", "");
	}
	
	public override void resolve(List<Tile> targetsTile)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		GameCard targetCard ;
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int level = GameView.instance.getCurrentSkill().Power;

		int minDamages;
		int maxDamages;
		int playerID;
		bool hasFoundMe = false ;
	
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
				minDamages = currentCard.getNormalDamagesAgainst(targetCard, 1);
				maxDamages = currentCard.getNormalDamagesAgainst(targetCard, 4+2*level);
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
					minDamages = currentCard.getNormalDamagesAgainst(targetCard, 1);
					maxDamages = currentCard.getNormalDamagesAgainst(targetCard, 4+2*level);
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
					minDamages = currentCard.getNormalDamagesAgainst(targetCard, 1);
					maxDamages = currentCard.getNormalDamagesAgainst(targetCard, 4+2*level);
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
					minDamages = currentCard.getNormalDamagesAgainst(targetCard, 1);
					maxDamages = currentCard.getNormalDamagesAgainst(targetCard, 4+2*level);
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
					minDamages = currentCard.getNormalDamagesAgainst(targetCard, 1);
					maxDamages = currentCard.getNormalDamagesAgainst(targetCard, 4+2*level);
					GameController.instance.applyOn2(playerID, Random.Range(minDamages,maxDamages+1));
				}
				else{
					GameController.instance.esquive(playerID,base.name);
				}
			}
		}
		if(!hasFoundMe){
			GameController.instance.applyOnMe(-1);
		}
		GameController.instance.endPlay();
	}

	public override void applyOn(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);
		if(GameView.instance.getCard(target).CardType.Id==6){
			GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(-1*value, -1, 130, base.name, value+" dégats subis"), (target==GameView.instance.getCurrentPlayingCard()),-1);
			if(targetCard.getLife()==targetCard.getLife()){
				GameView.instance.displaySkillEffect(target, "Sans effet", 1);
			}	
			else{
				GameView.instance.displaySkillEffect(target, "+"+Mathf.Min(targetCard.GetTotalLife()-targetCard.getLife())+" PV", 1);
			}
			GameView.instance.addAnim(GameView.instance.getTile(target), 130);
		}
		else{
			GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(value, -1, 130, base.name, value+" dégats subis"), (target==GameView.instance.getCurrentPlayingCard()), GameView.instance.getCurrentPlayingCard());
			GameView.instance.displaySkillEffect(target, "-"+value+"PV", 0);	
			GameView.instance.addAnim(GameView.instance.getTile(target), 130);
		}
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int minDamages, maxDamages;

		minDamages = currentCard.getNormalDamagesAgainst(targetCard, 1);
		maxDamages = currentCard.getNormalDamagesAgainst(targetCard, 4+2*level);
		string text ;

		if(GameView.instance.getCard(target).CardType.Id!=6){
			text = "PV : "+targetCard.getLife()+" -> ["+(targetCard.getLife()-minDamages)+"-"+(targetCard.getLife()-maxDamages)+"]";
		}
		else{
			text = "PV : "+targetCard.getLife()+" -> ["+Mathf.Min(targetCard.GetTotalLife(),targetCard.getLife()+minDamages)+"-"+Mathf.Min(targetCard.GetTotalLife(),(targetCard.getLife()+maxDamages))+"]";
		}

		int amount = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}

	public override int getActionScore(Tile t, Skill s){
		float score = 0 ;

		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard;
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int missingLife;
		int levelMin ;
		int levelMax ;

		int playerID = GameView.instance.getTileController(t).getCharacterID();
		if(playerID!=-1){
			targetCard = GameView.instance.getCard(playerID);
			levelMin = currentCard.getNormalDamagesAgainst(targetCard,1);
			levelMax = currentCard.getNormalDamagesAgainst(targetCard,4+s.Power*2);
			if(targetCard.CardType.Id==6){
				missingLife = targetCard.GetTotalLife()-targetCard.getLife();
				if(targetCard.isMine){
					score-=(proba-targetCard.getMagicalEsquive()/100f)*((missingLife*(Mathf.Max(0f,levelMax-missingLife)))+(((levelMin+Mathf.Min(levelMax,missingLife))/2f)*Mathf.Min(levelMax,missingLife)))/(levelMax-levelMin+1f);
				}
				else{
					score+=(proba-targetCard.getMagicalEsquive()/100f)*((missingLife*(Mathf.Max(0f,levelMax-missingLife)))+(((levelMin+Mathf.Min(levelMax,missingLife))/2f)*Mathf.Min(levelMax,missingLife)))/(levelMax-levelMin+1f);
				}
			}
			else{
				if(targetCard.isMine){
					score+=(proba-targetCard.getMagicalEsquive()/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
				else{
					score-=(proba-targetCard.getMagicalEsquive()/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
				}
			}
		}
		if(t.x<GameView.instance.boardWidth-1){
			playerID = GameView.instance.getTileController(new Tile(t.x+1, t.y)).getCharacterID();
			if(playerID!=-1){
				targetCard = GameView.instance.getCard(playerID);
				levelMin = currentCard.getNormalDamagesAgainst(targetCard,1);
				levelMax = currentCard.getNormalDamagesAgainst(targetCard,4+s.Power*2);

				if(targetCard.CardType.Id==6){
					missingLife = targetCard.GetTotalLife()-targetCard.getLife();
					if(targetCard.isMine){
						score-=(proba-targetCard.getMagicalEsquive()/100f)*((missingLife*(Mathf.Max(0f,levelMax-missingLife)))+(((levelMin+Mathf.Min(levelMax,missingLife))/2f)*Mathf.Min(levelMax,missingLife)))/(levelMax-levelMin+1f);
					}
					else{
						score+=(proba-targetCard.getMagicalEsquive()/100f)*((missingLife*(Mathf.Max(0f,levelMax-missingLife)))+(((levelMin+Mathf.Min(levelMax,missingLife))/2f)*Mathf.Min(levelMax,missingLife)))/(levelMax-levelMin+1f);
					}
				}
				else{
					if(targetCard.isMine){
						score+=(proba-targetCard.getMagicalEsquive()/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
					}
					else{
						score-=(proba-targetCard.getMagicalEsquive()/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
					}
				}
			}
		}
		if(t.x>0){
			playerID = GameView.instance.getTileController(new Tile(t.x-1, t.y)).getCharacterID();
			if(playerID!=-1){
				targetCard = GameView.instance.getCard(playerID);
				levelMin = currentCard.getNormalDamagesAgainst(targetCard,1);
				levelMax = currentCard.getNormalDamagesAgainst(targetCard,4+s.Power*2);

				if(targetCard.CardType.Id==6){
					missingLife = targetCard.GetTotalLife()-targetCard.getLife();
					if(targetCard.isMine){
						score-=(proba-targetCard.getMagicalEsquive()/100f)*((missingLife*(Mathf.Max(0f,levelMax-missingLife)))+(((levelMin+Mathf.Min(levelMax,missingLife))/2f)*Mathf.Min(levelMax,missingLife)))/(levelMax-levelMin+1f);
					}
					else{
						score+=(proba-targetCard.getMagicalEsquive()/100f)*((missingLife*(Mathf.Max(0f,levelMax-missingLife)))+(((levelMin+Mathf.Min(levelMax,missingLife))/2f)*Mathf.Min(levelMax,missingLife)))/(levelMax-levelMin+1f);
					}
				}
				else{
					if(targetCard.isMine){
						score+=(proba-targetCard.getMagicalEsquive()/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
					}
					else{
						score-=(proba-targetCard.getMagicalEsquive()/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
					}
				}
			}
		}
		if(t.y<GameView.instance.boardHeight-1){
			playerID = GameView.instance.getTileController(new Tile(t.x, t.y+1)).getCharacterID();
			if(playerID!=-1){
				targetCard = GameView.instance.getCard(playerID);
				levelMin = currentCard.getNormalDamagesAgainst(targetCard,1);
				levelMax = currentCard.getNormalDamagesAgainst(targetCard,4+s.Power*2);
				if(targetCard.CardType.Id==6){
					missingLife = targetCard.GetTotalLife()-targetCard.getLife();
					if(targetCard.isMine){
						score-=(proba-targetCard.getMagicalEsquive()/100f)*((missingLife*(Mathf.Max(0f,levelMax-missingLife)))+(((levelMin+Mathf.Min(levelMax,missingLife))/2f)*Mathf.Min(levelMax,missingLife)))/(levelMax-levelMin+1f);
					}
					else{
						score+=(proba-targetCard.getMagicalEsquive()/100f)*((missingLife*(Mathf.Max(0f,levelMax-missingLife)))+(((levelMin+Mathf.Min(levelMax,missingLife))/2f)*Mathf.Min(levelMax,missingLife)))/(levelMax-levelMin+1f);
					}
				}
				else{
					if(targetCard.isMine){
						score+=(proba-targetCard.getMagicalEsquive()/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
					}
					else{
						score-=(proba-targetCard.getMagicalEsquive()/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
					}
				}
			}
		}
		if(t.y>0){
			playerID = GameView.instance.getTileController(new Tile(t.x, t.y-1)).getCharacterID();
			if(playerID!=-1){
				targetCard = GameView.instance.getCard(playerID);
				levelMin = currentCard.getNormalDamagesAgainst(targetCard,1);
				levelMax = currentCard.getNormalDamagesAgainst(targetCard,4+s.Power*2);
				if(targetCard.CardType.Id==6){
					missingLife = targetCard.GetTotalLife()-targetCard.getLife();
					if(targetCard.isMine){
						score-=((missingLife*(Mathf.Max(0f,levelMax-missingLife)))+(((levelMin+Mathf.Min(levelMax,missingLife))/2f)*Mathf.Min(levelMax,missingLife)))/(levelMax-levelMin+1f);
					}
					else{
						score+=((missingLife*(Mathf.Max(0f,levelMax-missingLife)))+(((levelMin+Mathf.Min(levelMax,missingLife))/2f)*Mathf.Min(levelMax,missingLife)))/(levelMax-levelMin+1f);
					}
				}
				else{
					if(targetCard.isMine){
						score+=(proba-targetCard.getMagicalEsquive()/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
					}
					else{
						score-=(proba-targetCard.getMagicalEsquive()/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
					}
				}
			}
		}
		score = score * GameView.instance.IA.getAgressiveFactor() ;

		return (int)score ;
	}
}
