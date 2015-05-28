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
		GameController.instance.lookForTarget();
	}
	
	public override void resolve(int[] args)
	{
		if (args.Length != 1)
		{
			Debug.Log("Mauvais paramètres de résolution envoyés");
		} else
		{
			int targetID = args [0];
			GameController.instance.getCard(targetID).modifiers.Add(new StatModifier(GameController.instance.getCurrentSkill().Power * -1, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, 1));
			GameController.instance.reloadCard(targetID);
			GameController.instance.reloadSelectedPlayingCard(targetID);
		}
	}
}
