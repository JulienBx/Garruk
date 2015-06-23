﻿using UnityEngine;
using System.Collections.Generic;

public class PiegeDormeur : GameSkill
{
	public PiegeDormeur(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initTileTargetHandler(numberOfExpectedTargets);
		GameController.instance.displayAdjacentTileTargets();
		GameController.instance.displayMyControls("Piège à loups");
	}
	
	public override void resolve(List<Tile> targetsTile)
	{	
		int[] targets = new int[2];
		targets[0] = targetsTile[0].x;
		targets[1] = targetsTile[0].y;
		GameController.instance.startPlayingSkill();
		GameController.instance.applyOn(targets);
		
		GameController.instance.playSkill();
		GameController.instance.play();
	}
	
	public override void applyOn(int[] targets){
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		
		GameController.instance.addTileModifier(new Tile(targets[0], targets[1]), amount, ModifierType.Type_SleepingTrap, ModifierStat.Stat_No, -1, 2, "Piège endormissant", "Endort l'adversaire. "+amount+"% de chances de se réveiller chaque tour", "Permanent. Non visible du joueur adverse");
		GameController.instance.displaySkillEffect(targets[0], "Piège posé", 3, 2);
	}
	
	public override void activateTrap(int[] targets, int[] args){
		GameController.instance.addCardModifier(targets[0], args[0], ModifierType.Type_Sleeping, ModifierStat.Stat_No, -1, 12, "Endormi", "Le héros ne peut ni se déplacer ni utiliser une compétence", args[0]+"% de chances de se réveiller à chaque tour");
		GameController.instance.displaySkillEffect(targets[0], "Déclenche le piège et endort", 3, 1);
	}
	
	public override bool isLaunchable(Skill s){
		bool isLaunchable = false ;
		List<Tile> neighbourTiles = GameController.instance.getCurrentPCC().tile.getImmediateNeighbouringTiles();
		int playerID;
		foreach (Tile t in neighbourTiles)
		{
			playerID = GameController.instance.getTile(t.x, t.y).characterID;
			if (playerID == -1)
			{
				if (!GameController.instance.getTile(t.x, t.y).tile.isStatModifier)
				{
					isLaunchable = true ;
				}
			}
		}
		return isLaunchable ;
	}
	
	public override HaloTarget getTargetPCCText(Card c){
		
		HaloTarget h  = new HaloTarget(0); 
		
		int degats = GameController.instance.getCurrentSkill().ManaCost;
		
		h.addInfo("Pose un piège endormissant",0);
		
		return h ;
	}
	
	public override string getPlayText(){
		return "Piège endormissant" ;
	}
}