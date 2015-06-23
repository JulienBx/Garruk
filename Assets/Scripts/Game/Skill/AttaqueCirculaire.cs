using UnityEngine;
using System.Collections.Generic;

public class AttaqueCirculaire : GameSkill
{
	public AttaqueCirculaire(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		int[] targets ; 
		
		GameController.instance.startPlayingSkill();
		
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
				if (GameController.instance.getPCC(tempInt).canBeTargeted())	
				{
					targets = new int[1];
					targets[0] = tempInt;
					if (Random.Range(1,100) > GameController.instance.getCard(tempInt).GetEsquive())
					{                             
						GameController.instance.applyOn(targets);
					}
					else{
						GameController.instance.failedToCastOnSkill(targets);
					}
				}
			}
			i++;
		}
		
		GameController.instance.playSkill();
		GameController.instance.play();
	}
	
	public override void applyOn(int[] targets){
		Card targetCard ;
		int currentLife ;
		int damageBonusPercentage ;
		int amount ;
		int bouclier ;
		for (int i = 0 ; i < targets.Length ; i++){
			targetCard = GameController.instance.getCard(targets[i]);
			bouclier = targetCard.GetBouclier();
			currentLife = targetCard.GetLife();
			damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(targetCard);
			amount = (GameController.instance.getCurrentCard().GetAttack()*GameController.instance.getCurrentSkill().ManaCost/100)*(100+damageBonusPercentage)/100;
			amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
			GameController.instance.addCardModifier(targets[i], amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
			GameController.instance.displaySkillEffect(targets[i], "Inflige "+amount, 3, 1);
		}
	}
	
	public override void failedToCastOn(int[] targets){
		for (int i = 0 ; i < targets.Length ; i++){
			GameController.instance.displaySkillEffect(targets[i], "L'attaque échoue", 3, 0);
		}
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
				if (GameController.instance.getPCC(tempInt).canBeTargeted())	
				{
					isLaunchable = true ;
				}
			}
			i++;
		}
		return isLaunchable ;
	}
	
	public override string getPlayText(){
		return "Attaque circulaire" ;
	}
}
