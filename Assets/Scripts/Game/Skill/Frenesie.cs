using UnityEngine;
using System.Collections.Generic;

public class Frenesie : GameSkill
{
	public Frenesie()
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
		GameController.instance.applyOn(null);
		
		GameController.instance.playSkill(1);
		GameController.instance.play();
	}
	
	public override void applyOn(int[] targets){
//		int manacost = GameController.instance.getCurrentSkill().ManaCost;
//		int target = GameController.instance.currentPlayingCard ;
//		
//		GameController.instance.addCardModifier(target, manacost, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
//		GameController.instance.addCardModifier(target, manacost, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, 9, "Arme enchantée", "Attaque augmentée de "+manacost, "Permanent");
//		
//		GameController.instance.displaySkillEffect(target, "+"+manacost+" ATK\n-"+manacost+" PV", 3, 2);
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
	
	public override string getSuccessText(){
		return "A lancé frénésie" ;
	}
}
