﻿using UnityEngine;
using System.Collections.Generic;

public class AttaqueRapide : GameSkill
{
	public AttaqueRapide()
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
		int nbHitMax = Random.Range(1,5);
		int arg = 0;
		
		for (int i = 0 ; i < nbHitMax ; i++){
			if (Random.Range(1,101) > GameView.instance.getCard(target).GetEsquive())
			{                             
				arg++;
			}
		}
		
		if (arg!=0){
			GameController.instance.applyOn(target, arg);
		}
		else{
			GameController.instance.failedToCastOnSkill(target,1);
		}
		GameController.instance.play();
	}
	
	public override void applyOn(int target, int arg){
		
		Card targetCard = GameView.instance.getCard(target);
		int currentLife = targetCard.GetLife();
		int damageBonusPercentage = base.card.GetDamagesPercentageBonus(targetCard);
		int bouclier = targetCard.GetBouclier();
		int amount = arg*(base.skill.ManaCost*base.card.GetAttack()/100)*(100+damageBonusPercentage)/100;
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		
		if(currentLife!=(amount)){
			GameView.instance.displaySkillEffect(target, "HIT X"+arg+"\n-"+(amount)+" PV", 5);
		}
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		GameView.instance.displaySkillEffect(target, "ESQUIVE", 4);
	}
	
	public override string isLaunchable(){
		return GameView.instance.canLaunchAdjacentOpponents();
	}
	
	public override string getTargetText(Card targetCard){
		
		int proba;
		int probaEsquive = targetCard.GetEsquive();
		int currentLife = targetCard.GetLife();
		int bouclier = targetCard.GetBouclier();
		
		int damageBonusPercentage = GameView.instance.getCard(GameController.instance.getCurrentPlayingCard()).GetDamagesPercentageBonus(targetCard);
		int amount = base.card.GetAttack()*this.skill.ManaCost*(100+damageBonusPercentage)/10000;
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		string text ;
		
		if(currentLife-Mathf.Min(currentLife,amount)==0){
			text = "PV : "+currentLife+"->0";
		}
		else{
			text = "PV : "+currentLife+"->"+(currentLife-Mathf.Min(currentLife,amount))+"-"+(currentLife-Mathf.Min(currentLife,(4*amount)));
		}
		
		text += "\nHIT : ";
		if (probaEsquive!=0){
			proba = 100-probaEsquive;
			text+=proba+"% : "+100+"%(ATT) - "+probaEsquive+"%(ESQ)";
		}
		else{
			proba = 100;
			text+=proba+"%";
		}
		
		return text ;
	}
}
