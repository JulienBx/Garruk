using UnityEngine;
using System.Collections.Generic;

public class RayonEnergie : GameSkill
{
	public RayonEnergie()
	{
	
	}
	
	public override void launch()
	{
		//GameController.instance.lookForTarget("Choisir une cible à attaquer", "Lancer attaque");
	}
	
	public override void resolve(List<int> targetsPCC)
	{
//		int targetID = args [0];
//		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(new Card());
//		int amount = GameController.instance.getCurrentSkill().ManaCost*(100+damageBonusPercentage)/100;
//		
//		int myPlayerID = GameController.instance.currentPlayingCard;
//		string myPlayerName = GameController.instance.getCurrentCard().Title;
//		string hisPlayerName = GameController.instance.getCard(targetID).Title;
//		
//		GameController.instance.displaySkillEffect(myPlayerID, "Rayon d'energie", 3, 2);
//		
//		if (Random.Range(1, 100) > GameController.instance.getCard(targetID).GetEsquive())
//		{                             
//			//GameController.instance.addModifier(targetID, amount, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
//			GameController.instance.displaySkillEffect(targetID, "prend "+amount+" dégats", 3, 1);
//		}
//		else{
//			GameController.instance.displaySkillEffect(targetID, hisPlayerName+" esquive", 3, 0);
//		}
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
}
