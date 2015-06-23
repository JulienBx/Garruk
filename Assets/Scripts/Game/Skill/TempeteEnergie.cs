using UnityEngine;
using System.Collections.Generic;

public class TempeteEnergie : GameSkill {

	public TempeteEnergie()
	{
	
	}
	
	public override void launch()
	{
		//GameController.instance.lookForValidation(true, "Choisir une cible à attaquer", "Lancer Tempete");
	}
	
	public override void resolve(List<int> targetsPCC)
	{
		int maxAmount = GameController.instance.getCurrentSkill().ManaCost;
		int myPlayerID = GameController.instance.currentPlayingCard;
		
		GameController.instance.displaySkillEffect(myPlayerID, "Tempete d'energie", 3, 2);
		List<int> targetsToHit = new List<int>();
		PlayingCardController pcc ;
		for (int i = 0 ; i < 10 ; i++){
			pcc = GameController.instance.getPCC(i) ;
			if (!pcc.isDead && pcc.cannotBeTargeted==-1){
				if (Random.Range(1, 100) > GameController.instance.getCard(i).GetEsquive())
				{                             
					//int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus();
					//amount = Random.Range(5, maxAmount)*(100+damageBonusPercentage)/100;
					//GameController.instance.addModifier(i, amount, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
					//GameController.instance.displaySkillEffect(i, "prend "+amount+" dégats", 3, 1);
				}
				else{
					GameController.instance.displaySkillEffect(i, "Esquive", 3, 0);
				}
			}
		}
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
}
