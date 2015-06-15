using UnityEngine;
using System.Collections.Generic;

public class Lenteur : GameSkill
{
	public Lenteur()
	{

	}
	
	public override void launch()
	{
		Debug.Log("Je lance Lenteur");
		GameController.instance.lookForTarget("Choisir une cible pour Lenteur", "Lancer Lenteur");
	}
	
	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().Power * -1;
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé lenteur \n " 
			+ amount 
			+ " " 
			+ convertStatToString(ModifierStat.Stat_Move)
			+ " au prochain tour");

		int targetID = args [0];
		GameController.instance.getCard(targetID).modifiers.Add(new StatModifier(amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, 1, false,-1,"","",""));
		GameController.instance.reloadDestinationTiles();
		GameController.instance.reloadCard(targetID);

	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
}
