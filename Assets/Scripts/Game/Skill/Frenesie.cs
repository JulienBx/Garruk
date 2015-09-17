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
		GameController.instance.applyOn();
		GameController.instance.play();
	}
	
	public override void applyOn(){
		int manacost = base.skill.ManaCost;
		int myCurrentLife = base.card.GetAttack();
		int target = GameController.instance.getCurrentPlayingCard() ;
		
		GameController.instance.addCardModifier(target, manacost, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		GameController.instance.addCardModifier(target, manacost, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, 9, "Frénésie", "Attaque augmentée de "+manacost+". Permanent", "Permanent");
		
		if(myCurrentLife>manacost){
			GameView.instance.displaySkillEffect(target, "+"+manacost+" ATK\n-"+manacost+" PV", 4);
		}
	}
	
	public override string isLaunchable(){
		return "" ;
	}
}
