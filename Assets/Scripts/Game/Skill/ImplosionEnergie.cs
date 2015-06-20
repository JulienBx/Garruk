using UnityEngine;
using System.Collections.Generic;

public class ImplosionEnergie : GameSkill
{
	public ImplosionEnergie()
	{
	
	}
	
	public override void launch()
	{
		//GameController.instance.lookForValidation(true, "Choisir une cible à attaquer", "Lancer implosion");
	}
	
	public override void resolve(List<int> targetsPCC)
	{
		Tile t = GameController.instance.getCurrentPCC().tile ;
		int x = t.x;
		int y = t.y;
		int width = GameController.instance.boardWidth ;
		int height = GameController.instance.boardHeight ;
		
		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(new Card());
		int amount = GameController.instance.getCurrentSkill().ManaCost*(100+damageBonusPercentage)/100;
		int amountSelfDamage = GameController.instance.getCurrentCard().GetLife();
		
		int myPlayerID = GameController.instance.currentPlayingCard;
		
		GameController.instance.displaySkillEffect(myPlayerID, "Implosion d'energie", 3, 2);
		List<int> targetsToHit = new List<int>();
		
		if (x>0){
			if (y>0){
				if (GameController.instance.getTile(x-1,y-1).characterID!=-1){
					targetsToHit.Add(GameController.instance.getTile(x-1,y-1).characterID);
				}
			}
			if (y<height-1){
				if (GameController.instance.getTile(x-1,y+1).characterID!=-1){
					targetsToHit.Add(GameController.instance.getTile(x-1,y+1).characterID);
				}
			}
			if (GameController.instance.getTile(x-1,y).characterID!=-1){
				targetsToHit.Add(GameController.instance.getTile(x-1,y).characterID);
			}
		}
		if (x<width-1){
			if (y>0){
				if (GameController.instance.getTile(x+1,y-1).characterID!=-1){
					targetsToHit.Add(GameController.instance.getTile(x+1,y-1).characterID);
				}
			}
			if (y<height-1){
				if (GameController.instance.getTile(x+1,y+1).characterID!=-1){
					targetsToHit.Add(GameController.instance.getTile(x+1,y+1).characterID);
				}
			}
			if (GameController.instance.getTile(x+1,y).characterID!=-1){
				targetsToHit.Add(GameController.instance.getTile(x+1,y).characterID);
			}
		}
		if (y>0){
			if (GameController.instance.getTile(x,y-1).characterID!=-1){
				targetsToHit.Add(GameController.instance.getTile(x,y-1).characterID);
			}
		}
		if (y<height-1){
			if (GameController.instance.getTile(x,y+1).characterID!=-1){
				targetsToHit.Add(GameController.instance.getTile(x,y+1).characterID);
			}
		}
		if (GameController.instance.getTile(x,y).characterID!=-1){
			targetsToHit.Add(GameController.instance.getTile(x,y).characterID);
		}
		
		//GameController.instance.addModifier(myPlayerID, amountSelfDamage, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
		
		PlayingCardController pcc ;
		for (int i = 0 ; i < targetsToHit.Count ; i++){
			pcc = GameController.instance.getPCC(targetsToHit[i]) ;
			if (!pcc.isDead && pcc.cannotBeTargeted==-1){
				if (Random.Range(1, 100) > GameController.instance.getCard(targetsToHit[i]).GetEsquive())
				{                             
					//GameController.instance.addModifier(targetsToHit[i], amount, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
					GameController.instance.displaySkillEffect(targetsToHit[i], "prend "+amount+" dégats", 3, 1);
				}
				else{
					GameController.instance.displaySkillEffect(targetsToHit[i], "Esquive", 3, 0);
				}
			}
		}
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
}
