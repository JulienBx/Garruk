using UnityEngine;
using System.Collections.Generic;

public class Renaissance : GameSkill
{
	public Renaissance()
	{
		this.numberOfExpectedTargets = 1 ;
		base.ciblage = 17 ;
		base.auto = true;
		base.id = 100 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().setTexts("Renaissance", "Choisissez une unité à ressusciter, elle réapparaitra à l'endroit de sa mort");
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().displayAllEnemyTypes();
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().show(true);
		GameController.instance.play(this.id);
	}
	
	public override void resolve(List<Tile> targets)
	{	                     
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().show(false);
		GameController.instance.applyOn(GameView.instance.getTileCharacterID(targets[0].x, targets[0].y));
		GameController.instance.endPlay();
	}

	public override void applyOn(int i){
			
		int level = GameView.instance.getCurrentSkill().Power;
		int life = 20+4*level;

		GameView.instance.getPlayingCardController(i).rebirth();
		GameView.instance.displaySkillEffect(i, "Renaissance!", 1);
		GameView.instance.addAnim(0,GameView.instance.getTile(i));
	}
}
