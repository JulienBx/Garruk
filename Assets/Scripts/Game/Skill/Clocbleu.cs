using UnityEngine;
using System.Collections.Generic;

public class Chocbleu : GameSkill
{
	public Chocbleu(){
		this.numberOfExpectedTargets = 1 ; 
		base.ciblage = 1 ; 
		base.auto = false;
		base.id = 132 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		this.displayTargets();
	}
	
	public override void resolve(List<Tile> targets)
	{	
		GameController.instance.play(this.id);
		int target = GameView.instance.getTileCharacterID(targets[0].x, targets[0].y);
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
	
		if (Random.Range(1,101) <= GameView.instance.getCard(target).getEsquive())
		{                             
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				GameController.instance.applyOn(target);
			}
			else{
				GameController.instance.esquive(target,this.getText(0));
			}
		}
		GameController.instance.playSound(25);
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard = GameView.instance.getCard(target);
		int level = GameView.instance.getCurrentSkill().Power;
		float attackBonus = 0 ; 
		float malusBonus = 0 ;

		if(level==1){
			attackBonus = 1.2f;
			malusBonus = 0.6f;
		}
		else if(level==2){
			attackBonus = 1.2f;
			malusBonus = 0.5f;
		}
		else if(level==3){
			attackBonus = 1.3f;
			malusBonus = 0.5f;
		}
		else if(level==4){
			attackBonus = 1.3f;
			malusBonus = 0.4f;
		}
		else if(level==5){
			attackBonus = 1.4f;
			malusBonus = 0.4f;
		}
		else if(level==6){
			attackBonus = 1.4f;
			malusBonus = 0.3f;
		}
		else if(level==7){
			attackBonus = 1.5f;
			malusBonus = 0.3f;
		}
		else if(level==8){
			attackBonus = 1.5f;
			malusBonus = 0.2f;
		}
		else if(level==9){
			attackBonus = 1.6f;
			malusBonus = 0.2f;
		}
		else if(level==10){
			attackBonus = 1.6f;
			malusBonus = 0.1f;
		}

		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(attackBonus)));

		string text = "-"+damages+"PV";				

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,11,this.getText(0),damages+" dégats subis"), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.addAnim(3,GameView.instance.getTile(target));
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(1.2f+0.04f*GameView.instance.getCurrentSkill().Power)));
		int malus = Mathf.RoundToInt(currentCard.getAttack()*0.5f);

		string text = this.getText(0)+"\n-"+damages+"PV";

		int amount = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		int level = GameView.instance.getCurrentSkill().Power;
		float attackBonus = 0 ; 
		float malusBonus = 0 ;

		if(level==1){
			attackBonus = 1.2f;
			malusBonus = 0.6f;
		}
		else if(level==2){
			attackBonus = 1.2f;
			malusBonus = 0.5f;
		}
		else if(level==3){
			attackBonus = 1.3f;
			malusBonus = 0.5f;
		}
		else if(level==4){
			attackBonus = 1.3f;
			malusBonus = 0.4f;
		}
		else if(level==5){
			attackBonus = 1.4f;
			malusBonus = 0.4f;
		}
		else if(level==6){
			attackBonus = 1.4f;
			malusBonus = 0.3f;
		}
		else if(level==7){
			attackBonus = 1.5f;
			malusBonus = 0.3f;
		}
		else if(level==8){
			attackBonus = 1.5f;
			malusBonus = 0.2f;
		}
		else if(level==9){
			attackBonus = 1.6f;
			malusBonus = 0.2f;
		}
		else if(level==10){
			attackBonus = 1.6f;
			malusBonus = 0.1f;
		}

		GameCard currentCard = GameView.instance.getCurrentCard();
		int malus = Mathf.RoundToInt(currentCard.getAttack()*malusBonus);
		GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).updateAttack(currentCard.getAttack());
		GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addAttackModifyer(new Modifyer(-1*malus, -1, 11, this.getText(0), ". Permanent"));

		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), this.getText(0)+"\n-"+malus+" ATK", 0);
		GameView.instance.addAnim(2,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		int level = s.Power;
		float attackBonus = 0 ; 
		float malusBonus = 0 ;

		if(level==1){
			attackBonus = 1.2f;
			malusBonus = 0.6f;
		}
		else if(level==2){
			attackBonus = 1.2f;
			malusBonus = 0.5f;
		}
		else if(level==3){
			attackBonus = 1.3f;
			malusBonus = 0.5f;
		}
		else if(level==4){
			attackBonus = 1.3f;
			malusBonus = 0.4f;
		}
		else if(level==5){
			attackBonus = 1.4f;
			malusBonus = 0.4f;
		}
		else if(level==6){
			attackBonus = 1.4f;
			malusBonus = 0.3f;
		}
		else if(level==7){
			attackBonus = 1.5f;
			malusBonus = 0.3f;
		}
		else if(level==8){
			attackBonus = 1.5f;
			malusBonus = 0.2f;
		}
		else if(level==9){
			attackBonus = 1.6f;
			malusBonus = 0.2f;
		}
		else if(level==10){
			attackBonus = 1.6f;
			malusBonus = 0.1f;
		}

		int proba = WordingSkills.getProba(s.Id,s.Power);
		float malusAttack = currentCard.getAttack()*malusBonus; ;
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*(attackBonus)));
		int score ;
		if(damages>=targetCard.getLife()){
			score=Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*(200)+targetCard.getLife()/10f);
		}
		else{
			score=Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*(damages+5-targetCard.getLife()/10f));
		}

		score -= Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*(currentCard.getLife()/50f)*(2f)*(malusAttack));
		score = score * GameView.instance.IA.getAgressiveFactor() ;

		return score ;
	}
}
