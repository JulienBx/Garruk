using UnityEngine;
using System.Collections.Generic;

public class Tourelle : GameSkill
{
	public Tourelle()
	{
		this.numberOfExpectedTargets = 0 ; 
		base.name = "Tourelle";
		base.ciblage = 0 ;
		base.auto = true;
		base.id = 38 ;
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
		target = GameView.instance.getCurrentPlayingCard();
		string text = "Tourelle\nNiv.+"+level;
		 
		GameView.instance.getCard(target).setTourelle(new Modifyer(5+level, -1, 38, base.name, "Tourelle!"));
		GameView.instance.getPlayingCardController(target).showIcons();

		GameView.instance.displaySkillEffect(target, text, 1);
		GameView.instance.addAnim(GameView.instance.getTile(target), 38);
	}

	public override void applyOn(int target){
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard = GameView.instance.getCard(target);

		int level = GameView.instance.getCurrentCard().getTourelleDamages();
		int damages = currentCard.getNormalDamagesAgainst(targetCard, level);
		 
		GameView.instance.displaySkillEffect(target, "Tourelle\n-"+damages+"PV", 0);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,1,"Attaque",damages+" dégats subis"), false, -1);
		GameView.instance.addAnim(GameView.instance.getTile(target), 38);
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();

		int score = Mathf.RoundToInt((5f-t.y)*10f-(Mathf.Abs(2.5f-t.x)*5f));
		return score ;
	}
}
