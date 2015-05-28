using UnityEngine;
using System.Collections.Generic;

public class Attack : GameSkill
{
	public Attack()
	{
	
	}
	
	public override void launch()
	{
		Debug.Log("Je lance attack");
		GameController.instance.lookForTarget("","");
	}
	
	public override void resolve(int[] args)
	{
		if (args.Length!=1){
			Debug.Log ("Mauvais paramètres de résolution envoyés");
		}
		else{
			int targetID = args[0];
			GameController.instance.getCard(targetID).modifiers.Add(new StatModifier(GameController.instance.getCurrentCard().Attack*-1, ModifierType.Type_BonusMalus, ModifierStat.Stat_Life));
			GameController.instance.reloadCard(targetID);
		}
	}
}
