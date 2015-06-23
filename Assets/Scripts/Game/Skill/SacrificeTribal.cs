using UnityEngine;
using System.Collections.Generic;

public class SacrificeTribal : GameSkill
{
	public SacrificeTribal()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance sacrifice tribal");
		GameController.instance.lookForAdjacentAllyTarget("Choisir une cible à attaquer", "Lancer sac. tri.");
	}
	
	public override void resolve(List<int> targetsPCC)
	{
//		int targetID = args [0];
//		
//		int manaCost = GameController.instance.getCurrentSkill().ManaCost;
//		int degats = GameController.instance.getCard(targetID).GetLife();
//		int bonusA = GameController.instance.getCard(targetID).GetAttack()*manaCost/100 ;
//		int bonusL = -1*GameController.instance.getCard(targetID).GetLife()*manaCost/100 ;
//		
//		string message = GameController.instance.getCurrentCard().Title+" mange "+GameController.instance.getCard(targetID).Title;
//		
//		//GameController.instance.addModifier(targetID, degats, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
//		GameController.instance.displaySkillEffect(targetID, "Sacrifié", 3, 1);
//		
//		//GameController.instance.addModifier(GameController.instance.currentPlayingCard, bonusA, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Attack);
//		//GameController.instance.addModifier(GameController.instance.currentPlayingCard, bonusL, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
//		GameController.instance.setSacrificeTribal (bonusA, bonusL, -1);
//		
//		GameController.instance.displaySkillEffect(GameController.instance.currentPlayingCard, "Sacrifice tribal", 3, 0);
//		
//		GameController.instance.play();	
	}
	
	public override bool isLaunchable(Skill s){
		int myPlayer = GameController.instance.currentPlayingCard ; 
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
					if (myPlayer<5){
						if(tempInt < 5){
							isLaunchable = true ;
						}
					}
					else{
						if(tempInt > 4){
							isLaunchable = true ;
						}
					}
				}
			}
			i++;
		}
		return isLaunchable ;
	}
}