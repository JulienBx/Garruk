using UnityEngine;
using System.Collections.Generic;

public class PerfoTir : GameSkill
{
	public PerfoTir()
	{
		this.numberOfExpectedTargets = 1 ; 
		base.ciblage = 3 ;
		base.auto = false;
		base.id = 31 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		this.displayTargets();
	}
	
	public override void resolve(List<Tile> targets)
	{	
		GameController.instance.play(this.id);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int target = GameView.instance.getTileCharacterID(targets[0].x, targets[0].y);
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);

		if (Random.Range(1,101) <= GameView.instance.getCard(target).getMagicalEsquive()){
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
		GameController.instance.playSound(35);

		if(currentCard.isFou()){
			GameController.instance.applyOnMe(1);
		}
		else{
			GameController.instance.applyOnMe(-1);
		}
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getNormalDamagesAgainst(targetCard,  5+GameView.instance.getCurrentSkill().Power);
		if(currentCard.isFou()){
			damages = Mathf.RoundToInt(1.25f*damages);
		}

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages, -1, 31, this.getText(0), damages+" dégats subis"), false, GameView.instance.getCurrentPlayingCard());
		string text = "-"+damages+"PV";
		if(targetCard.getBouclier()>0){
			text+="\nBouclier détruit";
		}
		GameView.instance.getCard(target).emptyShieldModifyers();
		GameView.instance.getPlayingCardController(target).showIcons();

		GameView.instance.displaySkillEffect(target, text, 0);	
		GameView.instance.addAnim(6,GameView.instance.getTile(target));
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getNormalDamagesAgainst(targetCard,  5+GameView.instance.getCurrentSkill().Power);
		if(currentCard.isFou()){
			damages = currentCard.getNormalDamagesAgainst(targetCard,  Mathf.RoundToInt((5+GameView.instance.getCurrentSkill().Power)*1.25f));
		}
		string text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages);
		if(targetCard.getBouclier()>0){
			text+="\nBouclier détruit";
		}
		
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
		int score = 0 ;
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		GameCard currentCard = GameView.instance.getCurrentCard();

		int damages = currentCard.getNormalDamagesAgainst(targetCard, s.Power+5);
		if(currentCard.isFou()){
			damages = Mathf.RoundToInt(1.25f*damages);
		}

		if(damages>=targetCard.getLife()){
			score+=Mathf.RoundToInt(((100-targetCard.getMagicalEsquive())/100f)*(200f)+targetCard.getLife()/10f);
		}
		else{
			score+=Mathf.RoundToInt(((100-targetCard.getMagicalEsquive())/100f)*(damages)+5-targetCard.getLife()/10f);
		}

		score+=targetCard.getBouclier();

		if(currentCard.isFou()){
			int damagesMe = 11-currentCard.Skills[0].Power;
			if(damagesMe>=currentCard.getLife()){
				score-=100;
			}
			else{
				score-=damagesMe;
			}
		}
					
		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
