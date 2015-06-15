using UnityEngine;
using System.Collections.Generic;

public class FontaineDeJouvence : GameSkill
{
	public FontaineDeJouvence()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance Fontaine De Jouvence");
		GameController.instance.lookForTileTarget("Choisir une cible pour Fontaine De Jouvence", "Lancer Fontaine De Jouvence", HaloSkill.Void_Halo);
	}
	
	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		string pluriel = amount > 1 ? "s" : "";
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé Fontaine De Jouvence \n + "
			+ amount +
			" point" + pluriel + " de vie par tour");
		int targetX = args [0];
		int targetY = args [1];
		GameController.instance.addTileModifier(3, amount, targetX, targetY);
	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
