using UnityEngine;
using System.Collections.Generic;

public class Miracle : GameSkill
{
	public Miracle()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Miracle";
		base.ciblage = 3 ;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAllysButMeTargets();
	}

	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		int level = GameView.instance.getCurrentSkill().Power;

		if (Random.Range(1,101) <= GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) <= proba){
				int amount = Random.Range(1*level,3*level+1);
				GameController.instance.applyOn2(target, amount);
			}
			else{
				GameController.instance.esquive(target,2);
			}
		}
		GameController.instance.endPlay();
	}

	public override void applyOn(int target){
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();

		//GameView.instance.getCard(target).setState(new Modifyer(0, 1, 107, base.name, "Miracle. l'unité se régènere"));
		GameView.instance.displaySkillEffect(target, "Miraculé", 0);
		GameView.instance.getPlayingCardController(target).showIcons();
		GameView.instance.addAnim(GameView.instance.getTile(target), 107);
	}	

	public override string getTargetText(int target){
		
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();

		string text = "Malédiction";
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\n\nHIT% : "+probaHit;
		
		return text ;
	}
}



