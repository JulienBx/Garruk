using UnityEngine;
using System.Collections.Generic;

public class Berserk : GameSkill
{
	public Berserk(){
		this.numberOfExpectedTargets = 1 ;
		base.name = "Berserk";
		base.ciblage = 1 ;
		base.auto = false;
		base.id = 16 ;
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
				GameController.instance.esquive(target,base.name);
			}
		}
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}

	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*1.25f));
		string text = base.name+"\n-"+damages+"PV";				

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,16,base.name,damages+" dégats subis"), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.addAnim(3,GameView.instance.getTile(target));
		SoundController.instance.playSound(25);
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*1.25f));
		string text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages);				
		
		int amount = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		int probaEsquive = targetCard.getEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int autoDamages = currentCard.getNormalDamagesAgainst(currentCard, 25-level*2);
		string autotext = base.name+"\n-"+autoDamages+" PV";				
		GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).addDamagesModifyer(new Modifyer(autoDamages,-1,16,base.name,autoDamages+" dégats subis"), true, -1);
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), autotext, 0);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		int proba = WordingSkills.getProba(s.Id,s.Power);
		float malusAttack = currentCard.getAttack()/2f; ;
		int damages = currentCard.getNormalDamagesAgainst(targetCard, Mathf.RoundToInt(currentCard.getAttack()*1.25f));
		int score ;
		if(damages>=targetCard.getLife()){
			score=200;
		}
		else{
			score=Mathf.RoundToInt(((proba-targetCard.getEsquive())/100f)*(damages+Mathf.Max(0,30-(targetCard.getLife()-damages))));
		}

		int ownDamages = 25-2*s.Power;
		if(ownDamages>=currentCard.getLife()){
			score-=200;
		}
		else{
			score-=Mathf.RoundToInt(2*ownDamages+(40-currentCard.getLife()));
		}

		score = score * GameView.instance.IA.getAgressiveFactor() ;

		return score ;
	}
}
