using UnityEngine;
using System.Collections.Generic;

public class EnergieQuantique : GameSkill
{
	public EnergieQuantique()
	{
	
	}
	
	public override void launch()
	{
		GameController.instance.lookForValidation();
	}
	
	public override void resolve(List<int> targetsPCC)
	{
		int targetID ;
		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(new Card());
		int amount = GameController.instance.getCurrentSkill().ManaCost*(100+damageBonusPercentage)/100;
		
		int myPlayerID = GameController.instance.currentPlayingCard;
		int currentPlayerID = GameController.instance.currentPlayingCard;
		List<int> potentialTargets = new List<int>();
		
		int debut ;
		if (currentPlayerID<5){
			debut = 5 ;
		}
		else{
			debut = 0 ;
		}
		PlayingCardController pcc ;
		
		GameController.instance.displaySkillEffect(myPlayerID, "Energie quantique", 3, 2);
		
		for (int i = debut ; i < debut + 5 ; i++){
			pcc = GameController.instance.getPCC(i) ;
			if (!pcc.isDead && pcc.cannotBeTargeted==-1){
				potentialTargets.Add(i);
			}
		}
		
		targetID = potentialTargets[Random.Range(0, potentialTargets.Count-1)];
		
		if (Random.Range(1, 100) > GameController.instance.getCard(targetID).GetEsquive())
		{                             
			GameController.instance.addCardModifier(targetID, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
			GameController.instance.displaySkillEffect(targetID, "prend "+amount+" dégats", 3, 1);
		}
		else{
			GameController.instance.displaySkillEffect(targetID, "Esquive", 3, 0);
		}
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
}
