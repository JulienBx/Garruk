using UnityEngine;
using System.Collections.Generic;

public class PiegeALoups : GameSkill
{
	public PiegeALoups()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance piege a loups");
		GameController.instance.lookForEmptyAdjacentTile("Choisir une case à piéger", "Lancer piège à loups");
	}
	
	public override void resolve(int[] args)
	{
		int tileX = args [0];
		int tileY = args [1];
		
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		string message = GameController.instance.getCurrentCard()+" pose un piège à loups";
		
		GameController.instance.addTrap(tileX, tileY, 0, amount);	
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
			if (tempInt==-1)
			{
				isLaunchable = true ;
			}
			i++;
		}
		return isLaunchable ;
	}
}
