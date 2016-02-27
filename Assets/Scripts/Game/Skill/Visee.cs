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
		GameView.instance.launchValidationButton(base.name, GameView.instance.getCurrentSkill().Description);
	}

	public override void resolve(List<int> targetsPCC)
	{	                     
		GameController.instance.play(GameView.instance.runningSkill);
		GameController.instance.applyOn(-1);
		GameController.instance.endPlay();
	}

	public override void applyOn(int target){
		GameCard currentCard = GameView.instance.getCurrentCard();
		target = GameView.instance.getCurrentPlayingCard();
		int bonus = 50 + GameView.instance.getCurrentSkill().Power*10;

		GameView.instance.getCard(target).magicalBonusModifyers.Add(new Modifyer(bonus, 2, 25, base.name, "+"+bonus+"% aux dégats à distance. Actif 1 tour"));
		GameView.instance.getPlayingCardController(target).updateAttack();
		GameView.instance.displaySkillEffect(target, base.name+"\nDégats +"+bonus+"%", 1);
		GameView.instance.addAnim(GameView.instance.getTile(target), 25);
	}
}
