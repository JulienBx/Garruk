using UnityEngine;
using System.Collections.Generic;

public class Estoc : GameSkill
{
	public Estoc(){
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
			GameController.instance.applyOn(target);
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 0);
		}
		GameController.instance.play();
	}
	
	public override void applyOn(int target){
		Card targetCard = GameView.instance.getCard(target);
		int currentAttack = targetCard.GetAttack();
		int bouclier = targetCard.GetBouclier();
		int currentLife = targetCard.GetLife();
		int damageBonusPercentage = base.card.GetDamagesPercentageBonus(targetCard);
		int bonusAttack = base.skill.ManaCost;
		int amount = (base.card.GetAttack()/2)*(100+damageBonusPercentage)/100;
		if (base.card.isLache()){
			if(GameController.instance.getIsFirstPlayer()==GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
				if(GameView.instance.getPlayingCardTile(target).y==GameView.instance.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()).y-1){
					amount = (100+base.card.getPassiveManacost())*amount/100;
				}
			}
			else{
				if(GameView.instance.getPlayingCardTile(target).y-1==GameView.instance.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()).y){
					amount = (100+base.card.getPassiveManacost())*amount/100;
				}
			}
		}
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		
		GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		GameController.instance.addCardModifier(target, -1*bonusAttack, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, 1, 5, "Affaibli", "-"+bonusAttack+" ATK pendant 1 tour", "Actif 1 tour");
		
		if(currentLife!=amount){
			GameView.instance.displaySkillEffect(target, "HIT\n-"+amount+" PV\n-"+bonusAttack+" ATK", 5);
		}
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		GameView.instance.displaySkillEffect(target, "ESQUIVE", 4);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentOpponents();
	}
	
	public override string getTargetText(int id, Card targetCard){
		
		int currentLife = targetCard.GetLife();
		int damageBonusPercentage = this.card.GetDamagesPercentageBonus(targetCard);
		int currentAttack = targetCard.GetAttack();
		int bonusAttack = base.skill.ManaCost;
		
		int bouclier = targetCard.GetBouclier();
		int amount = (base.card.GetAttack()/2)*(100+damageBonusPercentage)/100;
		
		string text = "";
		if (base.card.isLache()){
			if(GameController.instance.getIsFirstPlayer()==GameView.instance.getIsMine(GameController.instance.getCurrentPlayingCard())){
				if(GameView.instance.getPlayingCardTile(id).y==GameView.instance.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()).y-1){
					amount = (100+base.card.getPassiveManacost())*amount/100;
					text="LACHE\n";
				}
			}
			else{
				if(GameView.instance.getPlayingCardTile(id).y-1==GameView.instance.getPlayingCardTile(GameController.instance.getCurrentPlayingCard()).y){
					amount = (100+base.card.getPassiveManacost())*amount/100;
					text="LACHE\n";
				}
			}
		}
		
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		
		text += "PV : "+currentLife+"->"+(currentLife-amount)+"\n";
		text += "ATK : "+currentAttack+"->"+Mathf.Max(0,currentAttack-bonusAttack)+"\n";
		
		int probaEsquive = targetCard.GetEsquive();
		int probaHit = Mathf.Max(0,100-probaEsquive) ;
		
		text += "HIT% : "+probaHit;
		
		return text ;
	}
}
