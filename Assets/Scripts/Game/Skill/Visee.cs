using UnityEngine;
using System.Collections.Generic;

public class Visee : GameSkill
{
	public Visee()
	{
		this.numberOfExpectedTargets = 0 ; 
		base.name = "Visee";
		base.ciblage = 0 ;
		base.auto = true;
	}

	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name, WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power));
	}

	public override void resolve(List<int> targetsPCC)
	{	                     
		GameController.instance.play(GameView.instance.runningSkill);
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}

	public override void applyOnMe(int target){
		target = GameView.instance.getCurrentPlayingCard();
		int bonus = 50 + GameView.instance.getCurrentSkill().Power*10;
		GameCard currentCard = GameView.instance.getCurrentCard();

		GameView.instance.getPlayingCardController(target).updateAttack(currentCard.getAttack());
		GameView.instance.getPlayingCardController(target).addBonusModifyer(new Modifyer(bonus, 2, 25, base.name, "dégats +"+bonus+"%. Actif 1 tour"));
		GameView.instance.displaySkillEffect(target, base.name+"\nDégats +"+bonus+"%", 2);
		GameView.instance.addAnim(GameView.instance.getTile(target), 25);
	}
}
