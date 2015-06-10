using UnityEngine;
using System.Collections.Generic;

public class AttaquePrecise : GameSkill
{
	public AttaquePrecise()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance attaque précise");
		GameController.instance.lookForAdjacentTarget("Choisir une cible à attaquer", "Lancer att. pré.");
	}
	
	public override void resolve(int[] args)
	{
		int targetID = args [0];
		
		int a = -1*GameController.instance.getCurrentSkill().ManaCost / 2;
		int attack = GameController.instance.getCurrentCard().Attack / 2 ;
		
		string message = GameController.instance.getCurrentCard().Title+" porte une attaque précise sur "+GameController.instance.getCard(targetID).Title;
		
		if (Random.Range(1, 100) > GameController.instance.getCard(targetID).GetEsquive()){
			message += "\n"+GameController.instance.getCurrentCard().Title+" inflige "+attack+" dégats et diminue de "+a+" l'attaque de "+GameController.instance.getCard(targetID).Title;
			GameController.instance.addModifier(targetID, attack, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
			GameController.instance.addModifier(targetID, a, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Attack, 1);
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