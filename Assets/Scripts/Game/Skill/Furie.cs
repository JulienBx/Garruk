using UnityEngine;
using System.Collections.Generic;

public class Furie : GameSkill
{
	public Furie()
	{
		this.numberOfExpectedTargets = 0 ; 
		base.name = "Furie";
		base.ciblage = 0 ;
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
		int level = GameView.instance.getCurrentSkill().Power;
		int bonusLife =  -1*Mathf.Min(currentCard.GetTotalLife()-currentCard.getLife(),level*2);
		int bonusAttack = level;
		target = GameView.instance.getCurrentPlayingCard();

		string text = "Furie\n+"+bonusAttack+" ATK";
		if(bonusLife>0){
			text+="\n+"+bonusLife+"PV";
		}
		 
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(-1*bonusLife, -1, 93, base.name, "+"+bonusLife+"PV"));
		GameView.instance.getCard(target).attackModifyers.Add(new Modifyer(bonusAttack, -1, 93, base.name, "+"+bonusAttack+"ATK. Permanent"));
		GameView.instance.getPlayingCardController(target).updateAttack();
		GameView.instance.displaySkillEffect(target, text, 1);
		GameView.instance.addAnim(GameView.instance.getTile(target), 93);

		GameView.instance.getCard(target).setState(new Modifyer(0, -1, 2, base.name, "Se déplace et attaque seul, amis et ennemis"));
		GameView.instance.getPlayingCardController(target).showIcons();
	}
}
