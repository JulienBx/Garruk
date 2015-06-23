using UnityEngine;
using System.Collections.Generic;

public class ForetDeLianes : GameSkill
{
	public ForetDeLianes()
	{
		
	}
	
	public override void launch()
	{
		//GameController.instance.lookForTileTarget("Choisir une cible pour Foret de Lianes", "Lancer Foret de Lianes", HaloSkill.Void_Halo);
	}
	
	public override void resolve(List<int> targetsPCC)
	{
//		int amount = GameController.instance.getCurrentSkill().ManaCost;
//
//		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
//			" a lancé Foret de Lianes \n -" +
//			amount + "% pour les déplacamements sur la case");
//		
//		int targetX = args [0];
//		int targetY = args [1];
//		GameController.instance.addTileModifier(1, amount, targetX, targetY);

	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
