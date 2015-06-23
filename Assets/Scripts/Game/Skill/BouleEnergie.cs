using UnityEngine;
using System.Collections.Generic;

public class BouleEnergie : GameSkill
{
	public BouleEnergie()
	{
	
	}
	
	public override void launch()
	{
		GameController.instance.lookForAnyTile();
	}
	
	public override void resolve(List<int> targetsPCC)
	{
//		int x = args [0];
//		int y = args [1];
//		int width = GameController.instance.boardWidth ;
//		int height = GameController.instance.boardHeight ;
//		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(new Card());
//		int amount = GameController.instance.getCurrentSkill().ManaCost*(100+damageBonusPercentage)/100;
//		
//		int myPlayerID = GameController.instance.currentPlayingCard;
//		
//		GameController.instance.displaySkillEffect(myPlayerID, "Boule d'energie", 3, 2);
//		List<int> targetsToHit = new List<int>();
//		
//		if (x>0){
//			if (y>0){
//				if (GameController.instance.getTile(x-1,y-1).characterID!=-1){
//					targetsToHit.Add(GameController.instance.getTile(x-1,y-1).characterID);
//				}
//			}
//			if (y<height-1){
//				if (GameController.instance.getTile(x-1,y+1).characterID!=-1){
//					targetsToHit.Add(GameController.instance.getTile(x-1,y+1).characterID);
//				}
//			}
//			if (GameController.instance.getTile(x-1,y).characterID!=-1){
//				targetsToHit.Add(GameController.instance.getTile(x-1,y).characterID);
//			}
//		}
//		if (x<width-1){
//			if (y>0){
//				if (GameController.instance.getTile(x+1,y-1).characterID!=-1){
//					targetsToHit.Add(GameController.instance.getTile(x+1,y-1).characterID);
//				}
//			}
//			if (y<height-1){
//				if (GameController.instance.getTile(x+1,y+1).characterID!=-1){
//					targetsToHit.Add(GameController.instance.getTile(x+1,y+1).characterID);
//				}
//			}
//			if (GameController.instance.getTile(x+1,y).characterID!=-1){
//				targetsToHit.Add(GameController.instance.getTile(x+1,y).characterID);
//			}
//		}
//		if (y>0){
//			if (GameController.instance.getTile(x,y-1).characterID!=-1){
//				targetsToHit.Add(GameController.instance.getTile(x,y-1).characterID);
//			}
//		}
//		if (y<height-1){
//			if (GameController.instance.getTile(x,y+1).characterID!=-1){
//				targetsToHit.Add(GameController.instance.getTile(x,y+1).characterID);
//			}
//		}
//		if (GameController.instance.getTile(x,y).characterID!=-1){
//			targetsToHit.Add(GameController.instance.getTile(x,y).characterID);
//		}
//		
//		PlayingCardController pcc ;
//		
//		for (int i = 0 ; i < targetsToHit.Count ; i++){
//			pcc = GameController.instance.getPCC(targetsToHit[i]) ;
//			if (!pcc.isDead && pcc.cannotBeTargeted==-1){
//				if (Random.Range(1, 100) > GameController.instance.getCard(targetsToHit[i]).GetEsquive())
//				{                             
//					GameController.instance.addModifier(targetsToHit[i], amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "","");
//					GameController.instance.displaySkillEffect(targetsToHit[i], "prend "+amount+" dégats", 3, 1);
//				}
//				else{
//					GameController.instance.displaySkillEffect(targetsToHit[i], "Esquive", 3, 0);
//				}
//			}
//		}
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
}
