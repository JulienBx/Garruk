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
		int amount = GameController.instance.getCurrentSkill().Power * -1;
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé sape \n " 
			+ amount 
			+ " " 
			+ convertStatToString(ModifierStat.Stat_Attack)
			+ " au prochain tour");
		GameController.instance.getCard(targetID).modifiers.Add(new StatModifier(amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, 1, false));
		GameController.instance.reloadCard(targetID);
	}
}
