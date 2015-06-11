using UnityEngine;
using System.Collections.Generic;

public class AttaquePrecise : GameSkill
{
	public AttaquePrecise()
	{
		
	}
	
	public override void launch()
	{
		GameController.instance.lookForAdjacentTarget("Choisir une cible à attaquer", "Lancer att. pré.");
	}
	
	public override void resolve(int[] args)
	{
		int targetID = args [0];
		
		int a = -1*GameController.instance.getCurrentSkill().ManaCost / 2;
		int attack = GameController.instance.getCurrentCard().Attack / 2 ;
		int myPlayerID = GameController.instance.currentPlayingCard;
		string myPlayerName = GameController.instance.getCurrentCard().Title;
		string hisPlayerName = GameController.instance.getCard(targetID).Title;
		
		GameController.instance.displaySkillEffect(myPlayerID, myPlayerName+" attaque", 3, 2);
		
		if (Random.Range(1, 100) > GameController.instance.getCard(targetID).GetEsquive()){
			GameController.instance.addModifier(targetID, attack, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
			GameController.instance.addModifier(targetID, a, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Attack, 1);
			GameController.instance.displaySkillEffect(targetID, "prend "+attack+" dégats et perd "+a+" ATK", 3, 1);
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