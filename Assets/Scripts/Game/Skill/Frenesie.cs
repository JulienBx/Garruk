using UnityEngine;
using System.Collections.Generic;

public class Frenesie : GameSkill
{
	public Frenesie()
	{
		this.numberOfExpectedTargets = 0 ; 
		base.name = "Frénésie";
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
	
	public override void applyOn(int i){
		string text = base.name;
		int level = GameView.instance.getCurrentSkill().Power;
		int target = GameView.instance.getCurrentPlayingCard();

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(22-level*2,-1,18,base.name,(22-level*2)+" dégats subis"));
		GameView.instance.getCard(target).attackModifyers.Add(new Modifyer(5, -1, 18, base.name, "+5 ATK. Permanent"));
		GameView.instance.getPlayingCardController(target).updateAttack();
		GameView.instance.displaySkillEffect(target, "+"+5+" ATK\n-"+(22-level*2)+"PV", 1);
		GameView.instance.addAnim(GameView.instance.getTile(target), 18);
	}
}
