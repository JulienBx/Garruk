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
		int success = 0 ;
		
		List<int> aliveCharacters = new List<int>();
//		for (int i = 0 ; i < GameController.instance.playingCards.Length ; i++){
//			if (!GameController.instance.getPCC(i).isDead){
//				aliveCharacters.Add(i);
//			}
//		}
		
		int target = aliveCharacters[Random.Range(0, aliveCharacters.Count)];
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).getMagicalEsquive())
		{                             
			GameController.instance.applyOn(target);
			success = 1 ;
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 1);
		}
		
		//GameController.instance.play();
	
	}
	
	public override void applyOn(){
		
//		Card targetCard = GameController.instance.getCard(target);
//		int currentLife = targetCard.GetLife();
//		int amount = GameController.instance.getCurrentSkill().ManaCost;
//		amount = Mathf.Min(currentLife,amount);
//		
//		GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
//		
//		GameController.instance.displaySkillEffect(target, "-"+amount+" PV", 3, 1);
	}
	
	public override string isLaunchable(){
		return "" ;
	}
}
