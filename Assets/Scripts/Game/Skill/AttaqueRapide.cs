﻿using UnityEngine;
using System.Collections.Generic;

public class AttaqueRapide : GameSkill
{
	public AttaqueRapide()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance attaque rapide");
		GameController.instance.lookForAdjacentTarget("Choisir une cible à attaquer", "Lancer att. rap.");
	}
	
	public override void resolve(int[] args)
	{
		int targetID = args [0];
		
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		int attack = GameController.instance.getCurrentCard().Attack * amount / 100 ;
		int totalAmount = 0 ;
		int nbCoups = Random.Range(2, 4);
		string message = nbCoups+" attaques lancées"+"\n";
		
		for (int i = 0 ; i < nbCoups ; i++){
			if (Random.Range(1, 100) > GameController.instance.getCard(targetID).GetEsquive()){
				message += "L'attaque N°"+(i+1)+" a réussi et inflige "+attack+" dégats"+"\n";
				totalAmount+=attack ;
			}
			else{
				message += GameController.instance.getCard(targetID).Title+" esquive l'attaque N°"+(i+1)+"\n";
			}
		}
		
		if(totalAmount>0){
			GameController.instance.addModifier(targetID, totalAmount, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);	
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
