using UnityEngine;
using System.Collections.Generic;

public class Agilite : GameSkill
{
	public Agilite()
	{
		this.numberOfExpectedTargets = 0 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	                     
		GameController.instance.startPlayingSkill();
		GameController.instance.applyOn(null);
		
		GameController.instance.playSkill();
		GameController.instance.play();
	}
	
	public override void applyOn(int[] targets){
		int esquive = GameController.instance.getCurrentSkill().ManaCost;
		int target = GameController.instance.currentPlayingCard ;
		
		GameController.instance.addCardModifier(target, esquive, ModifierType.Type_EsquivePercentage, ModifierStat.Stat_No, -1, 1, "Esquive", esquive+"% d'esquiver les dégats", "Permanent");
		GameController.instance.displaySkillEffect(target, "Esquive à "+esquive+"%", 3, 0);
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
	
	public override string getPlayText(){
		return "Agilite" ;
	}
}
