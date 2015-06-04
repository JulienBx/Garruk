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
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé temple sacré \n ");
		
		int targetID = args [0];
	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
