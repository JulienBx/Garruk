using UnityEngine;
using System.Collections.Generic;

public class Renaissance : GameSkill
{
	public Renaissance()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Renaissance";
		base.ciblage = 17 ;
		base.auto = true;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().setTexts("Renaissance", "Choisissez une unité à ressusciter, elle réapparaitra à l'endroit de sa mort");
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().displayAllEnemyTypes();
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().show(true);
		GameController.instance.play(GameView.instance.runningSkill);
	}
	
	public override void resolve(List<int> targetsPCC)
	{	                     
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().show(false);
		GameController.instance.applyOn(targetsPCC[0]);
		GameController.instance.endPlay();
	}

	public override void applyOn(int i){
			
		int level = GameView.instance.getCurrentSkill().Power;
		int life = 20+4*level;

		GameView.instance.getPlayingCardController(i).rebirth();
		GameView.instance.displaySkillEffect(i, "Renaissance!", 1);
		GameView.instance.addAnim(GameView.instance.getTile(i), 100);
	}
}
