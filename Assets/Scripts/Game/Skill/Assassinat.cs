using UnityEngine;
using System.Collections.Generic;

public class Assassinat : GameSkill
{
	public Assassinat()
	{
		
	}
	
	public override void launch()
	{
		GameController.instance.lookForAdjacentTarget("Choisir une cible à attaquer", "Lancer assassinat");
	}
	
	public override void resolve(int[] args)
	{
		int targetID = args [0];
		
		int killpercentage = GameController.instance.getCurrentSkill().ManaCost;
		int attack = GameController.instance.getCard(targetID).GetLife(); ;
		int myPlayerID = GameController.instance.currentPlayingCard;
		string myPlayerName = GameController.instance.getCurrentCard().Title;
		string hisPlayerName = GameController.instance.getCard(targetID).Title;
		
		GameController.instance.displaySkillEffect(myPlayerID, "Assassinat", 3, 2);
		
		if (Random.Range(1, 100) > GameController.instance.getCard(targetID).GetEsquive()){
			if (Random.Range(1, 100) <= killpercentage){
				GameController.instance.addModifier(targetID, attack, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
				GameController.instance.displaySkillEffect(targetID, hisPlayerName+" meurt !", 3, 1);
			}
			else{
				GameController.instance.displaySkillEffect(targetID, hisPlayerName+" ne meurt pas !", 3, 0);
			}
		}
		else{
			GameController.instance.displaySkillEffect(targetID, hisPlayerName+" esquive", 3, 0);
		}
		
		GameController.instance.play();	
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
