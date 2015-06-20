using UnityEngine;
using System.Collections.Generic;

public class Assassinat : GameSkill
{
	public Assassinat()
	{
		this.idSkill = 12 ; 
		this.numberOfExpectedTargets = 1 ;
	}
	
	public override void launch()
	{
		GameController.instance.displayAdjacentTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{
		int killpercentage = GameController.instance.getCurrentSkill().ManaCost;
		//int attack = GameController.instance.getCard(targets[0]).GetLife(); ;
		int myPlayerID = GameController.instance.currentPlayingCard;
		
		GameController.instance.displaySkillEffect(myPlayerID, "Assassinat", 3, 2);
		
//		if (args[0] > GameController.instance.getCard(targets[0]).GetEsquive()){
//			if (args[1] <= killpercentage){
//				GameController.instance.addModifier(targets[0], attack, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
//				GameController.instance.displaySkillEffect(targets[0], "MORT", 3, 1);
//			}
//			else{
//				GameController.instance.displaySkillEffect(targets[0], "Ne meurt pas", 3, 0);
//			}
//		}
//		else{
//			GameController.instance.displaySkillEffect(targets[0], "Esquive", 3, 0);
//		}
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
	
	public override HaloTarget getTargetPCCText(Card c){
		
		HaloTarget h  = new HaloTarget(0); 
		int i ;
		
		int amount = c.GetLife();
		h.addInfo("DMG : "+amount,0);
		
		int probaHit = 100 - c.GetEsquive();
		int probaKill = 100 - GameController.instance.getCurrentSkill().ManaCost;
		if (probaHit>=80){
			i = 2 ;
		}
		else if (probaHit>=20){
			i = 1 ;
		}
		else{
			i = 0 ;
		}
		
		h.addInfo("HIT% : "+probaHit*probaKill/100,i);
		return h ;
	}
}
