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
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power*5;
		int life = Mathf.RoundToInt((0.75f-level*0.05f)*currentCard.getAttack());
		int target = GameView.instance.getCurrentPlayingCard();
		int damages = currentCard.getNormalDamagesAgainst(currentCard, life);

		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(level,-1,-1*i,base.name,damages+" dégats subis"), true);
		GameView.instance.getCard(target).attackModifyers.Add(new Modifyer(10, -1, 18, base.name, "+10ATK. Permanent"));
		GameView.instance.getPlayingCardController(target).updateAttack();
		GameView.instance.displaySkillEffect(target, base.name+"\n+10ATK\n-"+damages+"PV", 1);
		GameView.instance.addAnim(GameView.instance.getTile(target), 18);
	}
}
