using UnityEngine;
using System.Collections.Generic;

public class Agilite : GameSkill
{
	public Agilite()
	{
		this.numberOfExpectedTargets = 0 ;
		base.name = "Agilité";
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
		int esquive = GameView.instance.getCurrentSkill().Power*3+20;
		string text = base.name+"\nEsquive : "+esquive+"%";
		
		GameView.instance.getCurrentCard().addEsquiveModifyer(new Modifyer(esquive, -1, 14, base.name, "Esquive : "+esquive+"%"));
		GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).showIcons();

		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), text, 1);
		GameView.instance.addAnim(GameView.instance.getTile(GameView.instance.getCurrentPlayingCard()), 14);
	}
}
