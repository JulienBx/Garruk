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
		GameController.instance.lookForTileTarget("Choisir une cible pour Fontaine De Jouvence", "Lancer Fontaine De Jouvence");
	}
	
	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().Power;
		string pluriel = amount > 1 ? "s" : "";
		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé Fontaine De Jouvence \n + "
			+ amount +
			" point" + pluriel + " de vie par tour");
		
		int targetID = args [0];
		
		int decade = targetID / 10;
		GameController.instance.getTile(decade, targetID - decade * 10).addFontaine(amount);
	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
