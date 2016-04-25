using UnityEngine;
using System.Collections.Generic;

public class Justice : GameSkill
{
	public Justice()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Justice";
		base.ciblage = 0 ;
		base.auto = true;
	}
	
	public override void launch()
	{
		GameView.instance.launchValidationButton(base.name,  WordingSkills.getDescription(GameView.instance.getCurrentSkill().Id, GameView.instance.getCurrentSkill().Power-1));
		GameController.instance.play(GameView.instance.runningSkill);
	}
	
	public override void resolve(List<Tile> targets)
	{	
		int proba = WordingSkills.getProba(GameView.instance.getCurrentSkill().Id,GameView.instance.getCurrentSkill().Power);
		GameController.instance.applyOn(-1);

		int targetMax = GameView.instance.getMaxPVCard();
		int targetMin = GameView.instance.getMinPVCard();
		if(targetMin!=GameView.instance.getCurrentPlayingCard() && targetMax!=GameView.instance.getCurrentPlayingCard()){
			GameController.instance.applyOnMe(-1);
		}
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		int targetMax = GameView.instance.getMaxPVCard();
		int targetMin = GameView.instance.getMinPVCard();
		int level = GameView.instance.getCurrentSkill().Power*2+10;

		GameView.instance.getPlayingCardController(targetMax).addDamagesModifyer(new Modifyer(level,-1,1,"Attaque",level+" dégats subis"), (targetMax==GameView.instance.getCurrentPlayingCard()), -1);
		GameView.instance.displaySkillEffect(targetMax, "-"+level+" PV", 0);
		GameView.instance.addAnim(GameView.instance.getTile(targetMax), 95);

		GameView.instance.getPlayingCardController(targetMin).addDamagesModifyer(new Modifyer(-1*level,-1,1,"Attaque",level+" dégats subis"), false, -1);
		GameView.instance.displaySkillEffect(targetMin, "+"+level+" PV", 2);
		GameView.instance.addAnim(GameView.instance.getTile(targetMin), 95);
	}

	public override void applyOnMe(int value){
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 1);
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 0);
	}
}
