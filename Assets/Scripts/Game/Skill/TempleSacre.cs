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
		GameController.instance.lookForTileTarget("Choisir une cible pour Temple sacré", "Lancer Temple sacré");
	}
	
	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().Power;
		string pluriel = amount > 1 ? "s" : "";

		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé temple sacré \n +"
			+ amount 
			+ " point" + pluriel + " d'attaque sur la case");
		
		int targetID = args [0];

		int decade = targetID / 10;
		GameController.instance.getTile(decade, targetID - decade * 10).addTemple(amount);

	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
