using UnityEngine;
using System.Collections.Generic;

public class AttaqueCirculaire : GameSkill
{
	public AttaqueCirculaire(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.lookForValidation ();
	}
	
	public override void resolve(List<int> targetsPCC)
	{
		int myPlayerID = GameController.instance.currentPlayingCard;
		GameController.instance.displaySkillEffect(myPlayerID, "Attaque Circulaire", 3, 2);
		
//		for (int i = 0 ; i < targets.Length ; i++){
//			targetPCC = GameController.instance.getPCC(this.targets[i]);
//			damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(targetPCC.card);
//			degats = (GameController.instance.getCurrentSkill().ManaCost*GameController.instance.getCurrentCard().Attack/100)*(100+damageBonusPercentage)/100;
//			
//			if (args[i] > GameController.instance.getCard(targets[i]).GetEsquive()){
//				GameController.instance.addModifier(targets[i], degats, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
//				GameController.instance.displaySkillEffect(targets[i], degats+" dégats", 3, 1);
//			}
//			else{
//				GameController.instance.displaySkillEffect(targets[i], "Esquive", 3, 0);
//			}
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
}
