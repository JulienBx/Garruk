using UnityEngine;
using System.Collections.Generic;

public class AttaqueCirculaire : GameSkill
{
	public AttaqueCirculaire()
	{
		this.numberOfExpectedTargets = 0 ; 
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		List<Tile> tempTiles;
		Tile t = GameView.instance.getPlayingCardTile(GameController.instance.getCurrentPlayingCard());
		tempTiles = t.getImmediateNeighbourTiles();
		
		bool isLaunchable = false ;
		int i = 0 ;
		int tempInt ; 
		
		while (!isLaunchable && i<tempTiles.Count){
			t = tempTiles[i];
			tempInt = GameView.instance.getTileCharacterID(t.x, t.y);
			if (tempInt!=-1)
			{
				if (GameView.instance.getPCC(tempInt).canBeTargeted())	
				{
					if (Random.Range(1,101) > GameView.instance.getCard(tempInt).GetEsquive())
					{                             
						GameController.instance.applyOn(tempInt);
					}
					else{
						GameController.instance.failedToCastOnSkill(tempInt, 0);
					}
				}
			}
			i++;
		}
		
		GameController.instance.play();
	}
	
	public override void applyOn(int target){
		Card targetCard = GameView.instance.getCard(target);
		int currentLife ;
		int damageBonusPercentage ;
		int amount ;
		int bouclier ;
		bouclier = targetCard.GetBouclier();
		currentLife = targetCard.GetLife();
		damageBonusPercentage = base.card.GetDamagesPercentageBonus(targetCard);
		amount = (base.card.GetAttack()*base.skill.ManaCost/100)*(100+damageBonusPercentage)/100;
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		
		if(currentLife!=amount){
			GameView.instance.displaySkillEffect(target, "-"+amount+" PV", 5);
		}
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		GameView.instance.displaySkillEffect(target, "ESQUIVE", 4);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentOpponents();
	}
}
