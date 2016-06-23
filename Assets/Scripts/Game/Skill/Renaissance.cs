using UnityEngine;
using System.Collections.Generic;

public class Renaissance : GameSkill
{
	public Renaissance()
	{
		this.initTexts();
		this.numberOfExpectedTargets = 1 ;
		base.texts = new List<string[]>();
		texts.Add(new string[]{"Renaissance","Rebirth"});
		texts.Add(new string[]{"Choisissez une unité à ressusciter, elle réapparaitra à à coté de l'unité active","Choose a unit to revive, it will come back next to the active unit"});
		texts.Add(new string[]{"1 cristal créé","Creation of 1 cristal"});
		texts.Add(new string[]{"échec","fail"});
		base.ciblage = 17 ;
		base.auto = true;
		base.id = 100 ;
	}
	
	public override void launch()
	{
		GameView.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().setTexts(this.getText(0), this.getText(1));
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
		GameView.instance.displaySkillEffect(i, this.getText(0), 1);
		GameView.instance.addAnim(0,GameView.instance.getTile(i));
	}
}
