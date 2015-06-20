using UnityEngine;
using System.Collections.Generic;

public class AttaqueRapide : GameSkill
{
	public AttaqueRapide()
	{
		this.idSkill = 14 ; 
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.displayAdjacentTargets();
	}

	public override void resolve(List<int> targetsPCC)
	{
//		int targetID = args [0];
//		
//		int myPlayerID = GameController.instance.currentPlayingCard;
//		string myPlayerName = GameController.instance.getCurrentCard().Title;
//		string hisPlayerName = GameController.instance.getCard(targetID).Title;
//	
//		int amount = GameController.instance.getCurrentSkill().ManaCost;
//		int nbSuccessfullAttacks = 0 ;
//		int attack = GameController.instance.getCurrentCard().GetAttack() * amount / 100 ;
//		int totalAmount = 0 ;
//		int nbCoups = Random.Range(2, 4);
//		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(new Card());
//		
//		GameController.instance.displaySkillEffect(myPlayerID,"Attaque rapide", 3, 2);
//		
//		for (int i = 0 ; i < nbCoups ; i++){
//			if (Random.Range(1, 100) > GameController.instance.getCard(targetID).GetEsquive()){
//				totalAmount+=attack ;
//				nbSuccessfullAttacks++;
//			}
//		}
//		if(totalAmount>0){
//			totalAmount = totalAmount*(100+damageBonusPercentage)/100;
//			GameController.instance.addModifier(targetID, totalAmount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage,-1,-1,"","","");	
//		}
//		GameController.instance.displaySkillEffect(targetID, "touché "+nbSuccessfullAttacks+" fois. "+totalAmount+ " dégats", 3, 1);
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
