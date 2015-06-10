using UnityEngine;
using System.Collections.Generic;

public class AttaqueFrontale : GameSkill
{
	public AttaqueFrontale()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance attaque frontale");
		GameController.instance.lookForAdjacentTarget("Choisir une cible à attaquer", "Lancer att. fro.");
	}
	
	public override void resolve(int[] args)
	{
		int targetID = args [0];
		
		int degats = GameController.instance.getCurrentSkill().ManaCost / 2;
		int attack = GameController.instance.getCurrentSkill().ManaCost ;
		
		string message = GameController.instance.getCurrentCard().Title+" porte une attaque frontale sur "+GameController.instance.getCard(targetID).Title;
		
		if (Random.Range(1, 100) > GameController.instance.getCard(targetID).GetEsquive()){
			message += "\n"+GameController.instance.getCurrentCard().Title+" inflige "+attack+" dégats à "+GameController.instance.getCard(targetID).Title;
			message += "\n"+GameController.instance.getCurrentCard().Title+" s'inflige "+attack+" dégats ";
			
			GameController.instance.addModifier(targetID, attack, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
			GameController.instance.addModifier(targetID, degats, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
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
