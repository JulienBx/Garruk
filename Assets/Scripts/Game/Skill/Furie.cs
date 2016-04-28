using UnityEngine;
using System.Collections.Generic;

public class Furie : GameSkill
{
	public Furie()
	{
		this.numberOfExpectedTargets = 0 ; 
		base.name = "Furie";
		base.ciblage = 0 ;
		base.auto = true;
		base.id = 93 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name, WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power));
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targets)
	{	                     
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOnMe(int target){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int bonusLife = level*2+10;
		int bonusAttack = 5+level;
		target = GameView.instance.getCurrentPlayingCard();

		string text = "Furie\n+"+bonusAttack+" ATK";
		if(bonusLife>0){
			text+="\n+"+bonusLife+"PV";
		}

		GameView.instance.getPlayingCardController(target).updateAttack(currentCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(bonusAttack, -1, 93, base.name, ". Permanent"));
		GameView.instance.getPlayingCardController(target).updateLife(currentCard.getLife());
		GameView.instance.getPlayingCardController(target).addPVModifyer(new Modifyer(bonusLife, -1, 93, base.name, ". Permanent"));

		GameView.instance.getCard(target).setFurious(new Modifyer(0, -1, 93, base.name, "Incontrolable!"));
		GameView.instance.getPlayingCardController(target).showIcons();

		GameView.instance.displaySkillEffect(target, text, 1);
		GameView.instance.addAnim(GameView.instance.getTile(target), 93);
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard ;
		int score = 20+4*s.Power;

		List<int> everyone = GameView.instance.getEveryoneButMe();
		int smallestMove = 10; 
		bool isSmallestMoveEnnemi = false ;
		int nbEnnemis =0; 
		int nbAllies =0;
		int moveAllies =0;
		int moveEnnemis =0;
		int move ;
		Tile tile ;
		for(int i = 0 ; i < everyone.Count ; i++){
			targetCard = GameView.instance.getCard(everyone[i]);
			tile = GameView.instance.getTile(everyone[i]);
			move = Mathf.Abs(t.x-tile.x)+Mathf.Abs(t.y-tile.y);
			if(move<=smallestMove){
				if(move==smallestMove){
					if(!targetCard.isMine){
						isSmallestMoveEnnemi = false ;
					}
				}
				else{
					smallestMove = move ;
					isSmallestMoveEnnemi = targetCard.isMine;
				}
			}
			if(targetCard.isMine){
				nbEnnemis++;
				moveEnnemis+=move;
			}
			else{
				nbAllies++;
				moveAllies+=move;
			}
		}

		if(!isSmallestMoveEnnemi){
			score-=100;
		}
		//print()
		score += Mathf.RoundToInt(100f*((1.0f*moveAllies/nbAllies)-(1.0f*moveEnnemis/nbEnnemis)));
				
		return score ;
	}
}
