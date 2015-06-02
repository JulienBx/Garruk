using UnityEngine;
using System.Collections.Generic;

public class Agilite : GameSkill
{
	public Agilite()
	{
	}
	
	public override void launch()
	{
		Debug.Log("Je lance agilité");
		//GameController.instance.lookForTarget("Choisir une cible pour Dissipation", "Lancer Dissipation");
	}

	public override void resolve(int[] args)
	{
		
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
}
