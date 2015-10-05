using UnityEngine;
using System.Collections.Generic;

public class EnergieQuantique : GameSkill
{
	public EnergieQuantique()
	{
		this.numberOfExpectedTargets = 0 ; 
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		List<int> targets = GameView.instance.getAllys();
		
		for(int i = 0 ; i < targets.Count ; i++){
			if (Random.Range(1,101) > GameView.instance.getCard(targets[i]).GetMagicalEsquive())
			{
				GameView.instance.getGC().applyOn(targets[i]);
			}
			else{
				GameView.instance.getGC().failedToCastOnSkill(targets[i], 0);
			}
		}
		
		GameView.instance.getGC().play();
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
