using UnityEngine;
using System.Collections.Generic;

public class ForetDeLianes : GameSkill
{
	public ForetDeLianes()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance Foret de Lianes");
		GameController.instance.lookForTileTarget("Choisir une cible pour Foret de Lianes", "Lancer Foret de Lianes");
	}
	
	public override void resolve(int[] args)
	{
		int amount = GameController.instance.getCurrentSkill().Power;

		GameController.instance.play(GameController.instance.getCurrentCard().Title + 
			" a lancé Foret de Lianes \n -" +
			amount + " pour les déplacamements sur la case");
		
		int targetID = args [0];
		
		int decade = targetID / 10;

		TileController tileController = GameController.instance.getTile(decade, targetID - decade * 10);
		//tileController.tile.StatModifier = new StatModifier(-amount, ModifierType.Type_Multiplier, ModifierStat.Stat_Move);
		tileController.addForetIcon();
		foreach (Tile til in tileController.tile.getImmediateNeighbouringTiles())
		{
			TileController tc = GameController.instance.getTile(til.x, til.y);
			tc.addForetIcon();
			//tc.tile.StatModifier = new StatModifier(-amount, ModifierType.Type_Multiplier, ModifierStat.Stat_Move);
			if (GameController.instance.getTile(til.x, til.y).characterID != -1)
			{
				GameController.instance.getPCC(GameController.instance.getTile(til.x, til.y).characterID).card.TileModifier = tc.tile.StatModifier;
			}
		}

	}
	
	public override bool isLaunchable(Skill s)
	{
		return true;
	}
}
