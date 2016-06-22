using UnityEngine;
using System.Collections.Generic;

public class Kunai : GameSkill
{
	public Kunai()
	{
		this.numberOfExpectedTargets = 1 ;
		base.ciblage = 0 ;
		base.auto = true;
		base.id = 8 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(this.getText(0),  GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targetsP)
	{	
		List<int> targets = GameView.instance.getOpponents(GameView.instance.getCurrentCard().isMine);

		int target = targets[Random.Range(0,targets.Count)];
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				int value = this.getValue(GameView.instance.getCurrentSkill().Power);
				GameController.instance.applyOn2(target, value);
			}
			else{
				GameController.instance.esquive(target,this.getText(0));
			}
		}
		GameController.instance.playSound(35);
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public int getValue(int level){
		int value = -1;
		if(level==1){
			value=Random.Range(3,8);
		}
		else if(level==2){
			value=Random.Range(4,9);
		}
		else if(level==3){
			value=Random.Range(5,10);
		}
		else if(level==4){
			value=Random.Range(6,11);
		}
		else if(level==5){
			value=Random.Range(7,12);
		}
		else if(level==6){
			value=Random.Range(8,13);
		}
		else if(level==7){
			value=Random.Range(9,14);
		}
		else if(level==8){
			value=Random.Range(10,15);
		}
		else if(level==9){
			value=Random.Range(11,16);
		}
		else if(level==10){
			value=Random.Range(12,17);
		}
		return value;
	}
	
	public override void applyOn(int target, int value){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getNormalDamagesAgainst(targetCard, value);
		string text = "-"+damages+"PV";

		if (currentCard.isLache() && !currentCard.hasMoved){
			damages = currentCard.getNormalDamagesAgainst(targetCard, damages+5+currentCard.getSkills()[0].Power);
			text = "-"+damages+"PV\n(lâche)";
		}

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,8,"Pistolero",damages+" dégats subis"), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.displaySkillEffect(target, "-"+damages+" PV", 0);
		GameView.instance.addAnim(6,GameView.instance.getTile(target));
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0), 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		float score = 0 ;
		GameCard targetCard ;
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int levelMin ;
		int levelMax ;

		List<int> enemies = GameView.instance.getOpponents(false);
		for(int i = 0 ; i < enemies.Count ; i++){
			targetCard = GameView.instance.getCard(enemies[i]);
			levelMin = currentCard.getNormalDamagesAgainst(targetCard,s.Power)+Mathf.RoundToInt(5-targetCard.getLife()/10f);
			levelMax = currentCard.getNormalDamagesAgainst(targetCard,5+s.Power*2)+Mathf.RoundToInt(5-targetCard.getLife()/10f);

			score+=((proba-targetCard.getMagicalEsquive())/100f)*((100f*(Mathf.Max(0f,levelMax-targetCard.getLife())))+((((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f)+Mathf.Max(0,30-(targetCard.getLife()-((levelMin+Mathf.Min(levelMax,targetCard.getLife()))/2f))))*Mathf.Min(levelMax,targetCard.getLife())))/(levelMax-levelMin+1f);
		}
		score = score/enemies.Count;

		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return (int)score ;
	}
}
