using UnityEngine;
using System.Collections.Generic;

public class Guerison : GameSkill
{
	public Guerison()
	{
		
	}
	
	public override void launch()
	{
		//GameController.instance.lookForTarget();
	}
	
	public override void resolve(List<int> targetsPCC)
	{
//		int amount = GameController.instance.getCurrentSkill().Power;
//
//
//		int targetID = args [0];
//		GameController.instance.addCardModifier(-amount, targetID, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage, -1);
//		GameController.instance.addGameEvent(GameController.instance.getCurrentSkill().Action, GameController.instance.getPCC(targetID).card.Title);
//
//		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
//			" a lancé guerison\n " 
//			+ convertStatToString(ModifierStat.Stat_Heal)
//			+ " "
//			+ amount 
//			+ " " 
//			+ convertStatToString(ModifierStat.Stat_Life));

	}
	
	public override string isLaunchable()
	{
		return "";
	}
}
