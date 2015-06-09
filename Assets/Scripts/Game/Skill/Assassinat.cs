using UnityEngine;
using System.Collections.Generic;

public class Assassinat : GameSkill
{
	public Assassinat()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance assassinat");
		GameController.instance.lookForAdjacentTarget("Choisir une cible à attaquer", "Lancer assassinat");
	}
	
	public override void resolve(int[] args)
	{
		int targetID = args [0];
		
		int killpercentage = GameController.instance.getCurrentSkill().ManaCost;
		int attack = GameController.instance.getCard(targetID).GetLife(); ;
		
		string message = GameController.instance.getCurrentCard().Title+" tente d'assassiner "+GameController.instance.getCard(targetID).Title;
		
		if (Random.Range(1, 100) > GameController.instance.getCard(targetID).GetEsquive()){
			if (Random.Range(1, 100) <= killpercentage){
				message += "\n"+"Le héros est assassiné";
				GameController.instance.addModifier(targetID, attack, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
			}
			else{
				message += "\n"+"L'assassinat échoue";
			}
		}
		else{
			message += "\n"+GameController.instance.getCard(targetID).Title+" esquive l'attaque";
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
