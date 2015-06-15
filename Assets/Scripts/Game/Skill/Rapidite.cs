using UnityEngine;
using System.Collections.Generic;

public class Rapidite : GameSkill
{
	public Rapidite()
	{

	}
	
	public override void launch()
	{
		Debug.Log("Je lance rapidite");
		GameController.instance.lookForTarget("Choisir une cible pour Rapidite", "Lancer Rapidite");
	}
	
	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().Power;
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé rapidite \n +" 
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
