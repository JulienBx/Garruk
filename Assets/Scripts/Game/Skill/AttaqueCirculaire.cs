using UnityEngine;
using System.Collections.Generic;

public class AttaqueCirculaire : GameSkill
{
	public AttaqueCirculaire(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.startPlayingSkill();
		
		List<Tile> tempTiles;
		Tile t = GameController.instance.getCurrentPCC().tile;
		
		tempTiles = t.getImmediateNeighbouringTiles();
		bool isLaunchable = false ;
		int success = 0 ;
		int i = 0 ;
		int tempInt ; 
		
		while (!isLaunchable && i<tempTiles.Count){
			t = tempTiles[i];
			tempInt = GameController.instance.getTile(t.x, t.y).characterID;
			if (tempInt!=-1)
			{
				if (GameController.instance.getPCC(tempInt).canBeTargeted())	
				{
					if (Random.Range(1,101) > GameController.instance.getCard(tempInt).GetEsquive())
					{                             
						GameController.instance.applyOn(tempInt);
						success = 1 ;
					}
					else{
						GameController.instance.failedToCastOnSkill(tempInt, 1);
					}
				}
			}
			i++;
		}
		
		GameController.instance.playSkill(success);
		GameController.instance.play();
	}
	
	public override void applyOn(int target){
		Card targetCard ;
		int currentLife ;
		int damageBonusPercentage ;
		int amount ;
		int bouclier ;
		targetCard = GameController.instance.getCard(target);
		bouclier = targetCard.GetBouclier();
		currentLife = targetCard.GetLife();
		damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(targetCard);
		amount = (GameController.instance.getCurrentCard().GetAttack()*GameController.instance.getCurrentSkill().ManaCost/100)*(100+damageBonusPercentage)/100;
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		GameController.instance.displaySkillEffect(target, "-"+amount+" PV", 3, 1);
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		//GameController.instance.displaySkillEffect(target, GameController.instance.castFailures.getFailure(indexFailure), 5, 1);
	}
	
	public override bool isLaunchable(Skill s){
		return GameController.instance.canLaunchAdjacentOpponents();
	}
	
	public override string getSuccessText(){
		return "A lancé attaque circulaire" ;
	}
	
	public override string getFailureText(){
		return "Attaque circulaire a échoué" ;
	}
}
