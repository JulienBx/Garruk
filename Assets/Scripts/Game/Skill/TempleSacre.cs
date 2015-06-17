using UnityEngine;
using System.Collections.Generic;

public class TempleSacre : GameSkill
{
	public TempleSacre()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance temple sacré");
		GameController.instance.lookForTileTarget("Choisir une cible pour Temple sacré", "Lancer Temple sacré", HaloSkill.Void_Halo);
	}
	
	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().ManaCost;

		int targetX = args [0];
		int targetY = args [1];
		GameController.instance.addTileModifier(0, amount, targetX, targetY);

		GameController.instance.addGameEvent(GameController.instance.getCurrentSkill().Action, "");

		string pluriel = amount > 1 ? "s" : "";
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé temple sacré \n +"
			+ amount 
			+ " point" + pluriel + " d'attaque sur la case");
	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
