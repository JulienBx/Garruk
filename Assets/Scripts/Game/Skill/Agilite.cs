using UnityEngine;
using System.Collections.Generic;

public class Agilite : GameSkill
{
	public Agilite()
	{
		this.numberOfExpectedTargets = 0 ;
		base.name = "Agilité";
		base.ciblage = 0 ;
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	                     
		GameController.instance.play(GameView.instance.runningSkill);
		GameController.instance.applyOn(-1);
		GameController.instance.showResult(true);
		GameController.instance.endPlay();
	}
	
	public override void applyOn(int target){
		string text = base.name;
		int esquive = GameView.instance.getCurrentSkill().Power*5;
		text += "\nEsquive : "+esquive+"%";
		
		GameView.instance.getCurrentCard().addEsquiveModifyer(new Modifyer(esquive, -1, 14, base.name, text));
		GameView.instance.getPlayingCardController(GameView.instance.getCurrentPlayingCard()).showIcons();
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name+"\n"+text, 0);
	}
}
