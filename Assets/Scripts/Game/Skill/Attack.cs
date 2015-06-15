using UnityEngine;
using System.Collections.Generic;

public class Attack : GameSkill
{
	public Attack()
	{
	
	}
	
	public override void launch()
	{
		GameController.instance.lookForAdjacentTarget("Choisir une cible à attaquer", "Lancer attaque");
	}
	
	public override void resolve(int[] args)
	{
		int targetID = args [0];
		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus();
		int amount = GameController.instance.getCurrentCard().GetAttack()*(100+damageBonusPercentage)/100;
		
		int myPlayerID = GameController.instance.currentPlayingCard;
		string myPlayerName = GameController.instance.getCurrentCard().Title;
		string hisPlayerName = GameController.instance.getCard(targetID).Title;
		
		GameController.instance.displaySkillEffect(myPlayerID, "Attaque", 3, 2);
		//yield return new WaitForSeconds(0.50f);
		
		if (Random.Range(1, 100) > GameController.instance.getCard(targetID).GetEsquive())
		{                             
			GameController.instance.addModifier(targetID, amount, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
			GameController.instance.displaySkillEffect(targetID, "prend "+amount+" dégats", 3, 1);
		}
		else{
			GameController.instance.displaySkillEffect(targetID, hisPlayerName+" esquive", 3, 0);
		}
		GameController.instance.play();
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
