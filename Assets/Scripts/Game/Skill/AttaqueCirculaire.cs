using UnityEngine;
using System.Collections.Generic;

public class AttaqueCirculaire : GameSkill
{
	public AttaqueCirculaire()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance attaque circulaire");
		GameController.instance.lookForAdjacentTarget("Choisir une cible à attaquer", "Lancer att. cir.");
	}
	
	public override void resolve(int[] args)
	{
		List<Tile> tempTiles;
		Tile t = GameController.instance.getCurrentPCC().tile;
		
		tempTiles = t.getImmediateNeighbouringTiles();
		List<int> targets = new List<int>();
		
		int i = 0 ;
		int tempInt ; 
		string message = "" ;
		
		while (i<tempTiles.Count){
			t = tempTiles[i];
			tempInt = GameController.instance.getTile(t.x, t.y).characterID;
			if (tempInt!=-1)
			{
				if (GameController.instance.getPCC(tempInt).cannotBeTargeted==-1)
				{
					targets.Add(tempInt);
				}
			}
			i++;
		}
		
		int degats = GameController.instance.getCurrentSkill().ManaCost*GameController.instance.getCurrentCard().Attack/100;
		message += "Attaque circulaire";
		
		for (int j = 0 ; j < targets.Count ; j++){
			if (Random.Range(1, 100) > GameController.instance.getCard(targets[j]).GetEsquive()){
				message += "\n"+GameController.instance.getCurrentCard().Title+" inflige "+degats+" dégats à "+GameController.instance.getCard(targets[j]).Title;
				GameController.instance.addModifier(targets[j], degats, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
			}
			else{
				message += "\n"+GameController.instance.getCard(targets[j]).Title+" esquive l'attaque";
			}
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
