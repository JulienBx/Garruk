using UnityEngine;
using System.Collections.Generic;

public class Visee : GameSkill
{
	public Visee()
	{
		this.numberOfExpectedTargets = 0 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Visée","Focus"});
		texts.Add(new string[]{". Actif 1 tour",". For 1 turn"});
		texts.Add(new string[]{"PV : ARG1 -> ARG2","HP : ARG1 -> ARG2"});
		base.ciblage = 0 ;
		base.auto = true;
		base.id = 25 ;
	}

	public override void launch()
	{
		GameView.instance.launchValidationButton(this.getText(0), GameView.instance.getCurrentCard().getSkillText(WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1)));
	}

	public override void resolve(List<Tile> targets)
	{	                     
		GameController.instance.play(this.id);
		GameController.instance.applyOnMe(-1);
		GameController.instance.playSound(37);
		GameController.instance.endPlay();
	}

	public override void applyOnMe(int target){
		target = GameView.instance.getCurrentPlayingCard();
		int bonus = 50 + GameView.instance.getCurrentSkill().Power*10;
		GameCard currentCard = GameView.instance.getCurrentCard();

		GameView.instance.getPlayingCardController(target).updateAttack(currentCard.getAttack());
		GameView.instance.getPlayingCardController(target).addBonusModifyer(new Modifyer(bonus, 2, 25, this.getText(0), this.getText(1)));
		GameView.instance.displaySkillEffect(target, this.getText(0)+"\n"+this.getText(2, new List<int>{bonus}), 2);
		GameView.instance.addAnim(7,GameView.instance.getTile(target));
	}

	public override int getActionScore(Tile t, Skill s){
		return 10;
	}
}
