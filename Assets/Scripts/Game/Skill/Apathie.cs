using UnityEngine;
using System.Collections.Generic;

public class Apathie : GameSkill
{
	public Apathie()
	{

	}
	
	public override void launch()
	{
		Debug.Log("Je lance apathie");
		GameController.instance.lookForTarget("Choisir une cible pour Apathie", "Lancer Apathie");
	}
	 
	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().Power * -1;
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé apathie \n " 
			+ amount 
			+ " " 
			+ convertStatToString(ModifierStat.Stat_Speed));

		int targetID = args [0];
		GameController.instance.getCard(targetID).modifiers.Add(new StatModifier(amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Speed));
		GameController.instance.reloadSortedList();

	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
}
