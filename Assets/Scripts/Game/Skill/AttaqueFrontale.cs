using UnityEngine;
using System.Collections.Generic;

public class AttaqueFrontale : GameSkill
{
	public AttaqueFrontale()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.displayAdjacentTargets();
	}

	public override void resolve(List<int> targetsPCC)
	{
//		PlayingCardController targetPCC = GameController.instance.getPCC(this.targets[0]);
//		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(targetPCC.card);
//		int attack = GameController.instance.getCurrentSkill().ManaCost*(100+damageBonusPercentage)/100 ;
//		int degats = (GameController.instance.getCurrentSkill().ManaCost / 2)*(100+damageBonusPercentage)/100;
//		int myPlayerID = GameController.instance.currentPlayingCard;
//		
//		GameController.instance.displaySkillEffect(myPlayerID, "Attaque frontale -"+degats+" PV", 3, 1);
//		
//		if (args[0] > GameController.instance.getCard(targets[0]).GetEsquive()){
//			GameController.instance.addModifier(myPlayerID, degats, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
//			GameController.instance.addModifier(this.targets[0], attack, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
//			GameController.instance.displaySkillEffect(this.targets[0], attack+" dégats", 3, 1);
//		}
//		else{
//			GameController.instance.displaySkillEffect(this.targets[0], "Esquive", 3, 0);
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
		
		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(c);
		int amount = (GameController.instance.getCurrentSkill().ManaCost/2)*(100+damageBonusPercentage)/100;
		int attack = GameController.instance.getCurrentSkill().ManaCost*(100+damageBonusPercentage)/100;
		
		h.addInfo("DMG : "+amount,0);
		h.addInfo("ATK : -"+attack,0);
		
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
