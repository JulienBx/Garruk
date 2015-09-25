using UnityEngine;
using System.Collections.Generic;

public class Rugissement : GameSkill
{
	public Rugissement()
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
				GameController.instance.applyOn(targets[i]);
			}
			else{
				GameController.instance.failedToCastOnSkill(targets[i], 0);
			}
		}
		
		GameController.instance.play();
	}
	
//	public override void applyOn(int target){
//		int amount = base.skill.ManaCost;
//		
//		GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, 1, 9, "Rugissement", "+"+amount+" ATK pour un tour", "Actif 1 tour");
//		GameView.instance.displaySkillEffect(target, "+"+amount+" ATK", 4);	
//	}
//	
//	public override void failedToCastOn(int target, int indexFailure){
//		GameView.instance.displaySkillEffect(target, "Esquive", 5);
//	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAllysButMeTargets();
	}
}
