using UnityEngine;
using System.Collections;

public class Sape : GameSkill
{
	public Sape()
	{

	}
	
	public override void launch()
	{
		Debug.Log("Je lance sape");
		GameController.instance.lookForTarget("Choisissez une cible", "Lancer Sape");
	}
	
	public override void resolve(int[] args)
	{
		int targetID = args[0];
		GameController.instance.getCard(targetID).modifiers.Add(new StatModifier(GameController.instance.getCurrentSkill().Power*-1, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack));
	}
}
