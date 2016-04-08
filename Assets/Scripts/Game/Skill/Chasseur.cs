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
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().setTexts("Chasseur", "Choisis une faction. L'unité active recevra un bonus contre les unités de cette faction");
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().displayAllEnemyTypes();
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().show(true);
		GameController.instance.play(GameView.instance.runningSkill);
	}
	
	public override void resolve(List<int> targetsPCC)
	{	                     
		GameView.instance.choicePopUp.GetComponent<PopUpChoiceController>().show(false);
		GameController.instance.applyOnMe(targetsPCC[0]);
		GameController.instance.endPlay();
	}

	public override void applyOnMe(int i){
			
		int level = GameView.instance.getCurrentSkill().Power;
		int bonus = 20+4*level;
		int target = GameView.instance.getCurrentPlayingCard();

		GameView.instance.getPlayingCardController(target).addBonusModifyer(new Modifyer(bonus,-1, 131, base.name," VS "+WordingCardTypes.getName(i),i));
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.displaySkillEffect(target, base.name+"Dégats +"+bonus+"% VS "+WordingCardTypes.getName(i), 1);
		GameView.instance.addAnim(GameView.instance.getTile(target), 131);
	}
}
