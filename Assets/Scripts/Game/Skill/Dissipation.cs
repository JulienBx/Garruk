using UnityEngine;
using System.Collections.Generic;

public class Dissipation : GameSkill
{
	public Dissipation()
	{
	}
	
	public override void launch()
	{
		Debug.Log("Je lance dissipation");
		GameController.instance.lookForTarget("Choisir une cible pour Dissipation", "Lancer Dissipation");
	}

	public override void resolve(int[] args)
	{
		if (Random.Range(0, 100) < GameController.instance.getCurrentSkill().ManaCost)
		{
			int targetID = args [0];
			GameController.instance.getCard(targetID).clearBuffs();
			GameController.instance.reloadSortedList();
			GameController.instance.reloadDestinationTiles();
			GameController.instance.reloadCard(targetID);
			GameController.instance.play(GameController.instance.getCurrentCard().Title + 
				" a lancé dissipation ");
		} else
		{
			GameController.instance.play(GameController.instance.getCurrentCard().Title + 
				" a tenté de lancer dissipation \n " 
				+ "mais a échoué");
		}
	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
