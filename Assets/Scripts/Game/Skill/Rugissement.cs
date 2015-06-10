using UnityEngine;
using System.Collections.Generic;

public class Rugissement : GameSkill
{
	public Rugissement()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance rugissement");
		GameController.instance.lookForAdjacentTarget("Choisir une cible à attaquer", "Lancer rug.");
	}
	
	public override void resolve(int[] args)
	{
		int targetID = GameController.instance.currentPlayingCard;
		int debut = 0 ;
		
		if (targetID > 4){
			debut = 5 	;
		}
		
		int bonus = GameController.instance.getCurrentSkill().ManaCost;
		
		string message = GameController.instance.getCurrentCard().Title+" ajoute "+bonus+" a l'attaque de tous ses alliés";
			
		for (int i = debut ; i < debut+5 ; i++){
			GameController.instance.addModifier(i, bonus, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Attack);
		}
		
		GameController.instance.play(message);	
	}
	
	public override bool isLaunchable(Skill s){
		return true;
	}
}
