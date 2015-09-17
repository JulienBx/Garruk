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
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	                     
		GameController.instance.applyOn();
		GameController.instance.play();
	}
	
	public override void applyOn(){
		int esquive = base.skill.ManaCost;
		int target = GameController.instance.getCurrentPlayingCard();
		
		GameController.instance.addCardModifier(target, esquive, ModifierType.Type_EsquivePercentage, ModifierStat.Stat_No, -1, 1, "Esquive", esquive+"% de chances d'esquiver les attaques au contact", "Permanent");
		
		GameView.instance.displaySkillEffect(target, "SUCCES\nEsquive = "+esquive+"%", 4);
	}
	
	public override string isLaunchable(){
		return "" ;
	}
}
