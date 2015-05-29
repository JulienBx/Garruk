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

		GameController.instance.lookForTarget("Choisir une cible pour Sape", "Lancer Sape");
	}
	
	public override void resolve(int[] args)
	{
		int targetID = args [0];
		GameController.instance.play(GameController.instance.getCurrentCard().Title + " a lancé furtivité");
		GameController.instance.getCard(targetID).modifiers.Add(new StatModifier(GameController.instance.getCurrentSkill().Power * -1, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, 1));
		GameController.instance.reloadCard(targetID);
	}
}
