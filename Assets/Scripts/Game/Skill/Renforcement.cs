using UnityEngine;
using System.Collections;

public class Renforcement : GameSkill
{
	public Renforcement()
	{

	}
	
	public override void launch()
	{
		Debug.Log("Je lance renforcement");
		GameController.instance.lookForTarget("Choisir une cible pour Renforcement", "Lancer Renforcement");
	}
	
	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().Power;
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé renforcement \n +" 
			+ amount 
			+ " " 
			+ convertStatToString(ModifierStat.Stat_Attack)
			+ " au prochain tour");

		int targetID = args [0];
		GameController.instance.getCard(targetID).modifiers.Add(new StatModifier(amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, 1, false,-1,"","",""));
		GameController.instance.reloadCard(targetID);

	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
}
