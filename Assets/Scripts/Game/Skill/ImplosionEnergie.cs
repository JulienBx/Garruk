using UnityEngine;
using System.Collections.Generic;

public class ImplosionEnergie : GameSkill
{
	public ImplosionEnergie(){
		this.numberOfExpectedTargets = 0 ; 
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		int[] targets ; 
		
		GameController.instance.startPlayingSkill();
		
		List<Tile> tempTiles;
		//Tile t = GameController.instance.getCurrentPCC().tile;
		
		//tempTiles = t.getImmediateNeighbouringTiles();
		bool isLaunchable = false ;
		int i = 0 ;
		int tempInt ; 
		
//		while (!isLaunchable && i<tempTiles.Count){
//			t = tempTiles[i];
//			tempInt = GameController.instance.getTile(t.x, t.y).characterID;
//			if (tempInt!=-1)
//			{
//				if (GameController.instance.getPCC(tempInt).canBeTargeted())	
//				{
//					targets = new int[1];
//					targets[0] = tempInt;
//					if (Random.Range(1,101) > GameController.instance.getCard(tempInt).GetEsquive())
//					{                             
//						GameController.instance.applyOn(targets);
//					}
//					else{
//						//GameController.instance.failedToCastOnSkill(targets);
//					}
//				}
//			}
//			i++;
//		}
		targets = new int[1];
		//targets[0] = GameController.instance.currentPlayingCard;
		GameController.instance.applyOn(targets);
		
		//GameController.instance.playSkill();
		GameController.instance.play();
	}
	
	public override void applyOn(int[] targets){
		Card targetCard ;
		int currentLife ;
		int amount ;
		//int bouclier ;
		
//		if(targets[0]==GameController.instance.currentPlayingCard){
//			amount = GameController.instance.getCurrentCard().GetLife();
//		}
//		else{
//			amount = GameController.instance.getCurrentSkill().ManaCost;
//		}
		targetCard = GameController.instance.getCard(targets[0]);
		currentLife = targetCard.GetLife();
		//GameController.instance.addCardModifier(targets[0], amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		//GameController.instance.displaySkillEffect(targets[0], "Inflige "+amount, 3, 1);
	}
	
	public override void failedToCastOn(int[] targets, int[] args){
		for (int i = 0 ; i < targets.Length ; i++){
			GameController.instance.displaySkillEffect(targets[i], "L'attaque échoue", 3, 0);
		}
	}
	
	public override bool isLaunchable(Skill s){
//		List<Tile> tempTiles;
//		Tile t = GameController.instance.getCurrentPCC().tile;
//		
//		tempTiles = t.getImmediateNeighbouringTiles();
		bool isLaunchable = false ;
//		int i = 0 ;
//		int tempInt ; 
//		
//		while (!isLaunchable && i<tempTiles.Count){
//			t = tempTiles[i];
//			tempInt = GameController.instance.getTile(t.x, t.y).characterID;
//			if (tempInt!=-1)
//			{
//				if (GameController.instance.getPCC(tempInt).canBeTargeted())	
//				{
//					isLaunchable = true ;
//				}
//			}
//			i++;
//		}
		return isLaunchable ;
	}
	
	public override string getSuccessText(){
		return "Implosion Energie" ;
	}
}
