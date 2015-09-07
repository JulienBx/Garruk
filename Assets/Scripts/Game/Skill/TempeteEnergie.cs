using UnityEngine;
using System.Collections.Generic;

public class TempeteEnergie : GameSkill
{
	public TempeteEnergie()
	{
		this.numberOfExpectedTargets = 0 ; 
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		List<int> targets = GameView.instance.getEveryone();
		int arg;
		
		for(int i = 0 ; i < targets.Count ; i++){
			if (Random.Range(1,101) > GameView.instance.getCard(targets[i]).GetMagicalEsquive())
			{
				arg = Random.Range(3,base.skill.ManaCost);
				GameController.instance.applyOn(targets[i], arg);
			}
			else{
				GameController.instance.failedToCastOnSkill(targets[i], 0);
			}
		}
		
		GameController.instance.play();
	}
	
	public override void applyOn(int target, int arg){
		GameController.instance.addCardModifier(target, arg, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		GameView.instance.displaySkillEffect(target, "-"+arg+" PV", 1);
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		GameView.instance.displaySkillEffect(target, "ESQUIVE", 4);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAnyone();
	}
}
