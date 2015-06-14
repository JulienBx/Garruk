using UnityEngine;
using System.Collections.Generic;

public class BouleEnergie : GameSkill
{
	public BouleEnergie()
	{
	
	}
	
	public override void launch()
	{
		GameController.instance.lookForAnyTile("Choisir une cible à attaquer", "Lancer boule");
	}
	
	public override void resolve(int[] args)
	{
		int x = args [0];
		int y = args [1];
		int width = GameController.instance.boardWidth ;
		int height = GameController.instance.boardHeight ;
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		
		int myPlayerID = GameController.instance.currentPlayingCard;
		
		GameController.instance.displaySkillEffect(myPlayerID, "Boule d'energie", 3, 2);
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
		if (x<this.boardWidth-1){
			if (y>0){
				this.tiles[x+1,y-1].GetComponent<TileController>().activateEffectZoneHalo();
			}
			if (y<this.boardHeight-1){
				this.tiles[x+1,y+1].GetComponent<TileController>().activateEffectZoneHalo();
			}
			this.tiles[x+1,y].GetComponent<TileController>().activateEffectZoneHalo();
		}
		if (y>0){
			this.tiles[x,y-1].GetComponent<TileController>().activateEffectZoneHalo();
		}
		if (y<this.boardHeight-1){
			this.tiles[x,y+1].GetComponent<TileController>().activateEffectZoneHalo();
		}
		this.tiles[x,y].GetComponent<TileController>().activateEffectZoneHalo();
		
		if (Random.Range(1, 100) > GameController.instance.getCard(targetID).GetEsquive())
		{                             
			GameController.instance.addModifier(targetID, amount, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
			GameController.instance.displaySkillEffect(targetID, "prend "+amount+" dégats", 3, 1);
		}
		else{
			GameController.instance.displaySkillEffect(targetID, hisPlayerName+" esquive", 3, 0);
		}
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
}
