using UnityEngine;
using System.Collections.Generic;

public class Chasseur : GameSkill
{
	public Chasseur()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Chasseur";
		base.ciblage = 12 ;
		base.auto = true;
	}
	
	public override void launch()
	{
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().setTexts("Chasseur", "Choisis une faction. L'unité active recevra un bonus contre les unités de cette faction");
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().show(true);
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().displayAllEnemyTypes();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	                     
		GameController.instance.play(GameView.instance.runningSkill);
		GameController.instance.applyOn(targetsPCC[0]);
		GameController.instance.endPlay();
	}

	public override void applyOn(int i){
		int level = GameView.instance.getCurrentSkill().Power;
		int bonus = 20+4*level;
		int target = GameView.instance.getCurrentPlayingCard();

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(bonus,-1,-1*i,base.name,"dégats +"+bonus+"% contre "+WordingCardTypes.getName(i)), true);
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.displaySkillEffect(target, base.name+"dégats +"+bonus+"% contre "+WordingCardTypes.getName(i), 1);
		GameView.instance.addAnim(GameView.instance.getTile(target), 131);
	}
}
