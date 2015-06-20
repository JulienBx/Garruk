using UnityEngine;
using System.Collections.Generic;

public class TirALarc : GameSkill
{
	public override void launch()
	{
		//GameController.instance.lookForTarget("Choisir une cible pour le tir", "Lancer Tir à l'arc");
	}
	
	public override void resolve(List<int> targetsPCC)
	{
//		int targetID = args [0];
//		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(new Card());
//		int attack = GameController.instance.getCurrentSkill().ManaCost*(100+damageBonusPercentage)/100;
//		int myPlayerID = GameController.instance.currentPlayingCard;
//		
//		string myPlayerName = GameController.instance.getCurrentCard().Title;
//		string hisPlayerName = GameController.instance.getCard(targetID).Title;
//		
//		GameController.instance.displaySkillEffect(myPlayerID, "Tir à l'arc", 3, 2);
//		
//		if (Random.Range(1, 100) > GameController.instance.getCard(targetID).GetEsquive())
//		{                             
//			//GameController.instance.addModifier(targetID, attack, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
//			GameController.instance.useSkill();
//			GameController.instance.displaySkillEffect(targetID, "prend "+attack+" dégats", 3, 1);
//		}
//		else{
//			GameController.instance.displaySkillEffect(targetID, hisPlayerName+" esquive", 3, 0);
//		}
//		GameController.instance.play();
	}
	
	public override bool isLaunchable(Skill s){
		return (s.nbLeft > 0) ;
	}
}
