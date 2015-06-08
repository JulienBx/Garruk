﻿using UnityEngine;
using System.Collections.Generic;

public class Attack : GameSkill
{
	public Attack()
	{
	
	}
	
	public override void launch()
	{
		Debug.Log("Je lance attack");
		GameController.instance.lookForAdjacentTarget("Choisir une cible à attaquer", "Lancer attaque");
	}
	
	public override void resolve(int[] args)
	{
		int targetID = args [0];
		string message = GameController.instance.getCurrentCard().Title+" attaque "+GameController.instance.getCard(targetID).Title+"\n";
		
		int amount = GameController.instance.getCurrentCard().Attack;
		if (Random.Range(1, 100) > GameController.instance.getCard(targetID).GetEsquive())
		{
			                             
			GameController.instance.getCard(targetID).modifiers.Add(new StatModifier(amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage));
			
			if (GameController.instance.getCard(targetID).GetLife() <= 0)
			{
				GameController.instance.getPCC(targetID).kill();
				GameController.instance.reloadTimeline();
			}
			GameController.instance.reloadCard(targetID);
			
			message+="L'attaque touche la cible et inflige "+amount+" degats"+"\n";
		}
		else{
			message+=GameController.instance.getCard(targetID).Title+" esquive l'attaque"+"\n";
		}
		
		GameController.instance.play(message);
	}

	public override bool isLaunchable(Skill s){
		List<Tile> tempTiles;
		Tile t = GameController.instance.getCurrentPCC().tile;
		
		tempTiles = t.getImmediateNeighbouringTiles();
		bool isLaunchable = false ;
		int i = 0 ;
		int tempInt ; 

		while (!isLaunchable && i<tempTiles.Count){
			t = tempTiles[i];
			tempInt = GameController.instance.getTile(t.x, t.y).characterID;
			if (tempInt!=-1)
			{
				if (GameController.instance.getPCC(tempInt).cannotBeTargeted==-1)
				{
					isLaunchable = true ;
				}
			}
			i++;
		}
		return isLaunchable ;
	}
}
