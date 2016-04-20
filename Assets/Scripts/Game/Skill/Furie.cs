using UnityEngine;
using System.Collections.Generic;

public class Furie : GameSkill
{
	public Furie()
	{
		this.numberOfExpectedTargets = 0 ; 
		base.name = "Furie";
		base.ciblage = 0 ;
		base.auto = true;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name, WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power));
		GameController.instance.play(GameView.instance.runningSkill);
	}
	
	public override void resolve(List<Tile> targets)
	{	                     
		GameController.instance.applyOnMe(-1);
		GameController.instance.endPlay();
	}
	
	public override void applyOnMe(int target){
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int bonusLife =  Mathf.Min(currentCard.GetTotalLife()-currentCard.getLife(),level*2+10);
		int bonusAttack = 5+level;
		target = GameView.instance.getCurrentPlayingCard();

		string text = "Furie\n+"+bonusAttack+" ATK";
		if(bonusLife>0){
			text+="\n+"+bonusLife+"PV";
		}
		 
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(-1*bonusLife, -1, 93, base.name, "+"+bonusLife+"PV"), false);
		GameView.instance.getPlayingCardController(target).updateAttack(currentCard.getAttack());
		GameView.instance.getPlayingCardController(target).addAttackModifyer(new Modifyer(bonusAttack, -1, 93, base.name, ". Permanent"));
		GameView.instance.getCard(target).setFurious(new Modifyer(0, -1, 93, base.name, "Incontrolable!"));
		GameView.instance.getPlayingCardController(target).showIcons();

		GameView.instance.displaySkillEffect(target, text, 1);
		GameView.instance.addAnim(GameView.instance.getTile(target), 93);
	}
}
