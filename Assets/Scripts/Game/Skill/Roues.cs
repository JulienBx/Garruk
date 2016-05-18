using UnityEngine;
using System.Collections.Generic;

public class Roues : GameSkill
{
	public Roues()
	{
		this.numberOfExpectedTargets = 0 ; 
		base.name = "Roues";
		base.ciblage = 0 ;
		base.auto = true;
		base.id = 50 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name,GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targets)
	{	                     
		GameController.instance.applyOnMe(-1);
		GameController.instance.playSound(37);
		GameController.instance.endPlay();
	}
	
	public override void applyOnMe(int i){
		GameCard currentCard = GameView.instance.getCurrentCard();

		int level = GameView.instance.getCurrentSkill().Power;
		int malusAttack = Mathf.RoundToInt((0.5f-level*0.05f)*currentCard.getAttack());
		int target = GameView.instance.getCurrentPlayingCard();

		GameView.instance.getPlayingCardController(target).addMoveModifyer(new Modifyer(2,-1,50,base.name,". Permanent"));
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.getPlayingCardController(target).updateAttack(currentCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(-1*malusAttack, -1, 50, base.name, ". Permanent"));
		GameView.instance.displaySkillEffect(target, base.name+"\n+2MOV\n-"+malusAttack+"ATK", 1);
		GameView.instance.addAnim(7,GameView.instance.getTile(target));
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int proba = WordingSkills.getProba(s.Id,s.Power);
		int score = 0;
		score = 10-2*Mathf.RoundToInt((0.5f-s.Power*0.05f)*currentCard.getAttack()) - (currentCard.getMove()-2)*5;

		score = score * GameView.instance.IA.getSoutienFactor() ;
		return score ;
	}
}
