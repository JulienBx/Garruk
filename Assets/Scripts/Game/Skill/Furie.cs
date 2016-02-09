using UnityEngine;
using System.Collections.Generic;

public class Furie : GameSkill
{
	public Furie()
	{
		this.numberOfExpectedTargets = 0 ; 
		base.name = "Furie";
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
		GameCard currentCard = GameView.instance.getCurrentCard();
		int level = GameView.instance.getCurrentSkill().Power;
		int life =  -1*Mathf.Min(currentCard.GetTotalLife()-currentCard.getLife(),currentCard.GetTotalLife()*(20+5*level)/100);
			
		GameView.instance.getPlayingCardController(target).addDamagesModifyer(new Modifyer(-1*life, -1, 0, text, "Furie\n+"+life+" PV"));
		GameView.instance.getCard(target).setState(new Modifyer(0, 1, 9, text, "Furieux. Se déplace et attaque seul, ne peut plus etre controlé par son colon"));
	}
}
