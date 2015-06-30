using UnityEngine;
using System.Collections.Generic;

public class AppositionDesMains : GameSkill
{
	public AppositionDesMains()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.displayAdjacentAllyTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		
		//int myPlayerID = GameController.instance.currentPlayingCard;
		//GameController.instance.displaySkillEffect(myPlayerID, "Apposition des mains", 3, 2);
		
//		if (args[0] > GameController.instance.getCard(this.targets[0]).GetEsquive())
//		{                             
//			GameController.instance.addModifier(this.targets[0], amount*-1, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
//			GameController.instance.displaySkillEffect(this.targets[0], "SOIN : +"+amount+" PV", 3, 0);
//		}
//		else{
//			GameController.instance.displaySkillEffect(this.targets[0], "Esquive", 3, 1);
//		}
	}
	
	public override bool isLaunchable(Skill s)
	{
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
//				if (GameController.instance.getPCC(tempInt).cannotBeTargeted==-1)
//				{
//					isLaunchable = true ;
//				}
			}
			i++;
		}
		return isLaunchable ;
	}
	
	public override HaloTarget getTargetPCCText(Card c){
		
		HaloTarget h  = new HaloTarget(0); 
		int i ;
		
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		h.addInfo("SOIN : +"+amount+"PV",2);
		
		int probaHit = 100 - c.GetEsquive();
		if (probaHit>=80){
			i = 2 ;
		}
		else if (probaHit>=20){
			i = 1 ;
		}
		else{
			i = 0 ;
		}
		h.addInfo("HIT% : "+probaHit,i);
		
		return h ;
	}
}
