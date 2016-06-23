using UnityEngine;
using System.Collections.Generic;

public class Tourelle : GameSkill
{
	public Tourelle()
	{
		this.initTexts();
		this.numberOfExpectedTargets = 0 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Tourelle","Turret"});
		texts.Add(new string[]{"Niveau ARG1","Level ARG1"});
		texts.Add(new string[]{"-ARG1 PV","-ARG1 HP"});
		base.ciblage = 0 ;
		base.auto = true;
		base.id = 38 ;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(this.getText(0), GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targets)
	{	                     
		GameController.instance.applyOnMe(-1);
		GameController.instance.playSound(28);
		GameController.instance.endPlay();
	}
	
	public override void applyOnMe(int target){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		target = GameView.instance.getCurrentPlayingCard();
		string text = this.getText(0)+"\n"+this.getText(1, new List<int>{level});
		 
		GameView.instance.getCard(target).setTourelle(new Modifyer(5+level, -1, 38, this.getText(0), this.getText(0)));
		GameView.instance.getPlayingCardController(target).showIcons();

		GameView.instance.displaySkillEffect(target, text, 1);
		GameView.instance.addAnim(0,GameView.instance.getTile(target));
	}

	public override void applyOn(int target){
		GameCard currentCard = GameView.instance.getCurrentCard();
		GameCard targetCard = GameView.instance.getCard(target);

		int level = GameView.instance.getCurrentCard().getTourelleDamages();
		int damages = currentCard.getNormalDamagesAgainst(targetCard, level);
		 
		GameView.instance.displaySkillEffect(target, this.getText(0)+"\n"+this.getText(2, new List<int>{damages}), 0);
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(damages,-1,1,"",""), false, -1);
		GameView.instance.addAnim(0,GameView.instance.getTile(target));
	}

	public override int getActionScore(Tile t, Skill s){
		GameCard currentCard = GameView.instance.getCurrentCard();

		int score = Mathf.RoundToInt((5f-t.y)*10f-(Mathf.Abs(2.5f-t.x)*5f));
		return score ;
	}
}
