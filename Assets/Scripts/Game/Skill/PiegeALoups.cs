using UnityEngine;
using System.Collections.Generic;

public class PiegeALoups : GameSkill
{
	public PiegeALoups()
	{
		
	}
	
	public override void launch()
	{
		GameController.instance.lookForEmptyAdjacentTile("Choisir une case à piéger", "Lancer piège à loups");
	}
	
	public override void resolve(List<int> targetsPCC)
	{
//		int tileX = args [0];
//		int tileY = args [1];
//		
//		int myPlayerID = GameController.instance.currentPlayingCard;
//		string myPlayerName = GameController.instance.getCurrentCard().Title;
//		
//		int amount = GameController.instance.getCurrentSkill().ManaCost;
//		
//		GameController.instance.addTrap(tileX, tileY, 0, amount);	
//		GameController.instance.displaySkillEffect(myPlayerID, "Piège à loups", 3, 0);
	}
	
	public override bool isLaunchable(Skill s){
		List<Tile> tempTiles;
		Tile t = GameController.instance.getCurrentPCC().tile;
		int myPlayerID = GameController.instance.currentPlayingCard;
		string myPlayerName = GameController.instance.getCurrentCard().Title;
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
