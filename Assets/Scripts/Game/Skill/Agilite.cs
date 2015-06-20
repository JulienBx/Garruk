using UnityEngine;
using System.Collections.Generic;

public class Agilite : GameSkill
{
	public Agilite(){
		this.idSkill = 16 ; 
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		//this.numberOfTargets = 0 ;
		//this.targets = new int[numberOfExpectedTargets];
		GameController.instance.lookForValidation ();
	}

	public override void resolve(List<int> targetsPCC)
	{
		int amount = GameController.instance.getCurrentSkill().ManaCost ;
		int myPlayerID = GameController.instance.currentPlayingCard;
		
		GameController.instance.addCardModifier(myPlayerID, amount, ModifierType.Type_EsquivePercentage, ModifierStat.Stat_No, -1, 1, "Agilité", "Possède "+amount+"% de chances d'esquiver les attaques ou compétences le ciblant", "Permanent");
		GameController.instance.displaySkillEffect(myPlayerID, "Esquive : +"+amount+"%", 3, 0);
		GameController.instance.play();
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
}
