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
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = 5+GameView.instance.getCurrentSkill().Power;
		int life = Mathf.RoundToInt(0.2f*currentCard.GetTotalLife());
		int target = GameView.instance.getCurrentPlayingCard();

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(life,-1,18,base.name,(22-level*2)+" dégats subis"));
		GameView.instance.getCard(target).attackModifyers.Add(new Modifyer(level, -1, 18, base.name, "+"+level+" ATK. Permanent"));
		GameView.instance.getPlayingCardController(target).updateAttack();
		GameView.instance.displaySkillEffect(target, base.name+"\n+"+level+" ATK\n-"+life+"PV", 1);
		GameView.instance.addAnim(GameView.instance.getTile(target), 18);
	}
}
