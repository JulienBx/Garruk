using UnityEngine;
using System.Collections.Generic;

public class Antibiotique : GameSkill
{
	public Antibiotique()
	{
		this.numberOfExpectedTargets = 1 ;
		base.name = "Antibiotique";
		base.ciblage = 7 ;
	}
	
	public override void launch()
	{
		GameView.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAllButMeModifiersTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.play(GameView.instance.runningSkill);
		int target = targetsPCC[0];
		int proba = GameView.instance.getCurrentSkill().proba;
		
		if (Random.Range(1,101) < GameView.instance.getCard(target).getMagicalEsquive()){
			GameController.instance.esquive(target,1);
		}
		else{
			if (Random.Range(1,101) < proba){
				GameController.instance.applyOn(target);
			}
			else{
				GameController.instance.esquive(target,7);
			}
		}
		
		if(GameView.instance.getCurrentCard().isGenerous()){
			List<int> targets = GameView.instance.getEveryoneButMe();
			targets.Remove(target);
			target = targets[Random.Range(0,targets.Count)];
			GameController.instance.applyOn(target);	
		}
	}
	
	public override void applyOn(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		
		GameView.instance.getCard(target).emptyModifiers();
		GameView.instance.getPlayingCardController(target).show(true);
		GameView.instance.displaySkillEffect(target, base.name+"\nEffets dissipés", 1);
		GameView.instance.displaySkillEffect(GameView.instance.getCurrentPlayingCard(), base.name, 0);
	}	
	
	public override string getTargetText(int target){
		string text = base.name;
		GameCard targetCard = GameView.instance.getCard(target);
		GameCard currentCard = GameView.instance.getCurrentCard();
		
		text += "\nDissipe les effets";
		
		int amount = GameView.instance.getCurrentSkill().proba;
		int probaEsquive = targetCard.getMagicalEsquive();
		int probaHit = Mathf.Max(0,amount*(100-probaEsquive)/100) ;
		text += "\nHIT% : "+probaHit;
		
		return text ;
	}
}
