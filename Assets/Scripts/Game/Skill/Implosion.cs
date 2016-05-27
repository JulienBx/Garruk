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
		base.id = 28 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name, GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targets)
	{	
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
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
							minDamages = currentCard.getNormalDamagesAgainst(targetCard, 10+2*GameView.instance.getCurrentSkill().Power);
							maxDamages = currentCard.getNormalDamagesAgainst(targetCard, 30+2*GameView.instance.getCurrentSkill().Power);
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
		GameController.instance.playSound(36);

		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target, int value){
		GameView.instance.displaySkillEffect(target, "-"+value+"PV", 0);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(value,-1,28,base.name,value+" dégats subis"), (target==GameView.instance.getCurrentPlayingCard()),-1);
		GameView.instance.addAnim(5,GameView.instance.getTile(target));
	}

	public override void applyOnMe(int value){
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addDamagesModifyer(new Modifyer(currentCard.getLife(),-1,28,base.name,value+" dégats subis"), true,-1);
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name+"\nSe détruit", 0);
		GameView.instance.addAnim(4,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		int score = 0 ;
		GameCard targetCard ;
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int target ; 
		int levelMin;
		int levelMax;

		score -=100 + 30 - currentCard.getLife();

		List<Tile> tempTiles = t.getImmediateNeighbourTiles();
		for(int i = 0 ; i < tempTiles.Count ; i++){
			target = GameView.instance.getTileController(tempTiles[i]).getCharacterID();
			if(target!=-1){
				targetCard = GameView.instance.getCard(target);
				levelMin = Mathf.FloorToInt((10+s.Power*2)*(1f+currentCard.getBonus(targetCard)/100f)*(1f-(targetCard.getBouclier()/100f)));
				levelMax = Mathf.FloorToInt((30+2*s.Power)*(1f+currentCard.getBonus(targetCard)/100f)*(1f-(targetCard.getBouclier()/100f)));
				if(currentCard.isFou()){
					levelMax = Mathf.RoundToInt(1.25f*levelMax);
				}
				if(targetCard.isMine){
					score+=Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*((200*(Mathf.Max(0f,levelMax-targetCard.getLife())))+(((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f));
				}
				else{
					score-=Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*((200*(Mathf.Max(0f,levelMax-targetCard.getLife())))+(((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f));
				}
			}
		}

		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
