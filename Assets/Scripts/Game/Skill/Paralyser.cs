using UnityEngine;
using System.Collections.Generic;

public class Paralyser : GameSkill
{
	public Paralyser()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance paralyser");
		//GameController.instance.lookForAdjacentTarget("Choisir une cible à attaquer", "Lancer paralyser");
	}
	
	public override void resolve(List<int> targetsPCC)
	{
//		int targetID = args [0];
//		
//		int amount = GameController.instance.getCurrentSkill().ManaCost;
//		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(new Card());
//		int attack = (GameController.instance.getCurrentCard().GetAttack() / 2)*(100+damageBonusPercentage)/100 ;
//		int myPlayerID = GameController.instance.currentPlayingCard;
//		string myPlayerName = GameController.instance.getCurrentCard().Title;
//		string hisPlayerName = GameController.instance.getCard(targetID).Title;
//		GameController.instance.displaySkillEffect(myPlayerID, "paralyser", 3, 2);
//		
//		if (Random.Range(1, 100) > GameController.instance.getCard(targetID).GetEsquive()){
//			//GameController.instance.addModifier(targetID, attack, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
//			
//			if (Random.Range(1, 100) <= amount){
//				GameController.instance.displaySkillEffect(targetID, "prend "+attack+" dégats et est paralysé", 3, 1);
//				GameController.instance.setParalyzed(targetID, 1);
//			}
//			else{
//				GameController.instance.displaySkillEffect(targetID, "prend "+attack+" dégats", 3, 1);
//			}
//		}
//		else{
//			GameController.instance.displaySkillEffect(targetID, hisPlayerName+" esquive", 3, 0);
//		}
//	
//		GameController.instance.play();	
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
