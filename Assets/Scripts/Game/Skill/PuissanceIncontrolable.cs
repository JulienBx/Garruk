using UnityEngine;
using System.Collections.Generic;

public class PuissanceIncontrolable : GameSkill
{
	public PuissanceIncontrolable()
	{
		this.numberOfExpectedTargets = 0 ; 
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.startPlayingSkill();
		int success = 0 ;
		
		List<int> aliveCharacters = new List<int>();
//		for (int i = 0 ; i < GameController.instance.playingCards.Length ; i++){
//			if (!GameController.instance.getPCC(i).isDead){
//				aliveCharacters.Add(i);
//			}
//		}
		
		int target = aliveCharacters[Random.Range(0, aliveCharacters.Count)];
		
		if (Random.Range(1,101) > GameController.instance.getCard(target).GetMagicalEsquive())
		{                             
			GameController.instance.applyOn(target);
			success = 1 ;
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 1);
		}
		
		GameController.instance.playSkill(success);
		GameController.instance.play();
	
	}
	
	public override void applyOn(int target){
		
		Card targetCard = GameController.instance.getCard(target);
		int currentLife = targetCard.GetLife();
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		amount = Mathf.Min(currentLife,amount);
		
		GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		
		GameController.instance.displaySkillEffect(target, "-"+amount+" PV", 3, 1);
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		//GameController.instance.displaySkillEffect(target, GameController.instance.castFailures.getFailure(indexFailure), 5, 1);
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
	
	public override string getSuccessText(){
		return "A lancé puissance incontrolable" ;
	}
	
	public override string getFailureText(){
		return "Puissance incontrolable a échoué" ;
	}
}
