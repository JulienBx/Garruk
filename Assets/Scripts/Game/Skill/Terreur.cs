using UnityEngine;
using System.Collections.Generic;

public class Terreur : GameSkill
{
	public Terreur()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance paralyser");
		GameController.instance.lookForAdjacentTarget("Choisir une cible à attaquer", "Lancer Terreur");
	}
	
	public override void resolve(int[] args)
	{
		int targetID = args [0];
		
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		int attack = GameController.instance.getCurrentCard().Attack / 2 ;
		
		string message = GameController.instance.getCurrentCard().Title+" attaque "+GameController.instance.getCard(targetID).Title;
		
		if (Random.Range(1, 100) > GameController.instance.getCard(targetID).GetEsquive()){
			message += "\n"+GameController.instance.getCurrentCard().Title+" inflige "+attack+" dégats";
			GameController.instance.addModifier(targetID, attack, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
			
			if (Random.Range(1, 100) <= amount){
				message += "\n"+GameController.instance.getCard(targetID).Title+" est paralysé";
				GameController.instance.setParalyzed(targetID, 1);
			}
			else{
				message += "\n"+GameController.instance.getCurrentCard().Title+" n'a pas réussi à paralyser "+GameController.instance.getCard(targetID).Title;
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
