using UnityEngine;
using System.Collections.Generic;

public class ToutDonner : GameSkill
{
	public ToutDonner()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayAdjacentOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	                     
		if (GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
			GameView.instance.hideTargets();
		}
		
		int target = targetsPCC[0];
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetEsquive())
		{                             
			GameController.instance.applyOn(target,0);
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 0);
		}
		GameController.instance.play();
		
		if (base.card.isGiant()){
			if (Random.Range(1,101) <= base.card.getPassiveManacost()){
				List<Tile> opponents = GameView.instance.getOpponentImmediateNeighbours(GameView.instance.getPlayingCardTile(target));
				if(opponents.Count>1){
					int ran = Random.Range(0,opponents.Count);
					target = GameView.instance.getTileCharacterID(opponents[ran].x, opponents[ran].y) ;
					
					if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
					{
						GameController.instance.applyOn(target,1);
					}
				}
			}
		}
	}
	
//	public override void applyOn(int target, int arg){
//		Card targetCard = GameView.instance.getCard(target);
//		int bouclier = targetCard.GetBouclier();
//		int currentLife = targetCard.GetLife();
//		int damageBonusPercentage = base.card.GetDamagesPercentageBonus(targetCard);
//		int amount = (this.card.GetAttack()*base.skill.ManaCost/100)*(100+damageBonusPercentage)/100;
//		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
//		GameController.instance.addCardModifier(GameController.instance.getCurrentPlayingCard(), amount, ModifierType.Type_Paralized, ModifierStat.Stat_No, -2, 2, "Paralisé", "Ne peut rien faire au prochain tour", "Actif 1 tour");
//		GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
//		if(currentLife!=amount){
//			if(arg==0){
//				GameView.instance.displaySkillEffect(target, "-"+amount+" PV", 5);
//			}
//			else{
//				GameView.instance.displaySkillEffect(target, "GEANT\n-"+amount+" PV", 5);
//			}
//		}
//	}
//	
//	public override void failedToCastOn(int target, int indexFailure){
//		GameView.instance.displaySkillEffect(target, "ESQUIVE", 4);
//	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentOpponents();
	}
	
	public override string getTargetText(int i, Card targetCard){
		
		int currentLife = targetCard.GetLife();
		int damageBonusPercentage = this.card.GetDamagesPercentageBonus(targetCard);
		
		int bouclier = targetCard.GetBouclier();
		int amount = (this.card.GetAttack()*base.skill.ManaCost/100)*(100+damageBonusPercentage)/100;
		amount = Mathf.Min(currentLife, amount-(bouclier*amount/100));
		
		string text = "PV : "+currentLife+"->"+(currentLife-amount)+"\n";
		int probaEsquive = targetCard.GetEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "HIT% : "+probaHit;
		
		return text ;
	}
}
