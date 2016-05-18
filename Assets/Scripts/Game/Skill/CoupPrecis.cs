using UnityEngine;
using System.Collections.Generic;

public class CoupPrecis : GameSkill
{
	public CoupPrecis(){
		this.numberOfExpectedTargets = 1 ; 
		base.name = "Coup précis";
		base.ciblage = 1 ;
		base.auto = false;
		base.id = 59 ;
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
		if (Random.Range(1,101) <= proba){
			GameController.instance.applyOn(target);
		}
		GameController.instance.playSound(25);
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = Mathf.RoundToInt(currentCard.getAttack()*(0.5f+level/20f));
		string text = "-"+damages+"PV";				
		if (currentCard.isLache() && !currentCard.hasMoved){
			damages = currentCard.getNormalDamagesAgainst(targetCard, damages+5+currentCard.getSkills()[0].Power);
			text = "-"+damages+"PV\n(lâche)";
		}
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,11,base.name,damages+" dégats subis"), false, GameView.instance.getCurrentPlayingCard());
		GameView.instance.displaySkillEffect(target, text, 0);
		GameView.instance.addAnim(3,GameView.instance.getTile(target));
	}
	
	public override string getTargetText(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int damages = Mathf.RoundToInt(currentCard.getAttack()*(0.5f+level/20f));
		string text = "PV : "+targetCard.getLife()+" -> "+(targetCard.getLife()-damages);				
		if (currentCard.isLache() && !currentCard.hasMoved){
			damages = currentCard.getNormalDamagesAgainst(targetCard, damages+5+currentCard.getSkills()[0].Power);
			text = base.name+"\n-"+damages+"PV\n(lâche)";
		}
		
		text += "\n\nHIT% : 100";
		
		return text ;
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(8,GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()));
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard = GameView.instance.getCard(GameView.instance.getTileCharacterID(t.x,t.y));
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int damages = Mathf.RoundToInt(Mathf.Min(targetCard.getLife(),currentCard.getAttack()*(0.5f+0.05f*s.Power)));
		int score ;
		if(damages>=targetCard.getLife()){
			score=200;
		}
		else{
			score=Mathf.RoundToInt(damages+Mathf.Max(0,30-(targetCard.getLife()-damages)));
		}

		score = score * GameView.instance.IA.getAgressiveFactor() ;
		return score ;
	}
}
