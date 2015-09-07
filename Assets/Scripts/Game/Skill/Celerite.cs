using UnityEngine;
using System.Collections.Generic;

public class Celerite : GameSkill
{
	public Celerite()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAllysButMeTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		if (GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int target = targetsPCC[0];
		
		int successChances = base.skill.ManaCost;
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
		{                             
			if (Random.Range(1,101) <= successChances)
			{ 
				GameController.instance.applyOn(target);
			}
			else{
				GameController.instance.failedToCastOnSkill(target, 2);
			}
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 1);
		}
		GameController.instance.play();
	}
	
	public override void applyOn(int target){
		GameController.instance.rankNext(target);
		GameView.instance.displaySkillEffect(target, "CELERITE", 5);
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		if (indexFailure==1){
			GameView.instance.displaySkillEffect(target, "ESQUIVE", 4);
		}
		else if (indexFailure==2){
			GameView.instance.displaySkillEffect(target, "ECHEC CELERITE", 4);
		}
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchOpponentsTargets();
	}
	
	public override string getTargetText(Card targetCard){
		
		int amount = base.skill.ManaCost;
		int attack = base.card.GetAttack();
		string text;
		
		text = "Avance le tour du héros\n";
		
		int probaEsquive = targetCard.GetMagicalEsquive();
		int probaHit = Mathf.Max(0,amount-probaEsquive) ;
		text += "HIT : "+probaHit;
		
		return text ;
	}
}
