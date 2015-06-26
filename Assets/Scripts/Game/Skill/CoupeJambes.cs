﻿using UnityEngine;
using System.Collections.Generic;

public class CoupeJambes : GameSkill
{
	public CoupeJambes(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameController.instance.displayAdjacentOpponentsTargets();
		GameController.instance.displayMyControls("Coupe-jambes");
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.startPlayingSkill();
		int target = targetsPCC[0];
		int successType = 0 ;
		
		if (Random.Range(1,101) > GameController.instance.getCard(target).GetEsquive())
		{                             
			GameController.instance.applyOn(target);
			successType = 1 ;
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 1);
		}
		GameController.instance.playSkill(successType);
		GameController.instance.play();
	}
	
	public override void applyOn(int target){
		Card targetCard = GameController.instance.getCard(target);
		int bouclier = targetCard.GetBouclier();
		int currentLife = targetCard.GetLife();
		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(targetCard);
		int manacost = GameController.instance.getCurrentSkill().ManaCost;
		int deplacement = GameController.instance.getCard(target).GetMove();
		int bonusDeplacement = manacost*deplacement/100;
		if (bonusDeplacement>deplacement){
			bonusDeplacement = deplacement - 1 ;
		}
		int attack = GameController.instance.getCurrentCard().GetAttack()/2;
		int amount = attack*(100+damageBonusPercentage)/100;
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		
		GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		GameController.instance.addCardModifier(target, -1*bonusDeplacement, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, 1, 8, "Lenteur", "-"+bonusDeplacement+" MOV", "Actif 1 tour");
		GameController.instance.displaySkillEffect(target, "-"+amount+" PV\n-"+bonusDeplacement+" MOV", 3, 1);
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		GameController.instance.displaySkillEffect(target, GameController.instance.castFailures.getFailure(indexFailure), 5, 1);
	}
	
	public override bool isLaunchable(Skill s){
		return GameController.instance.canLaunchAdjacentOpponents();
	}
	
	public override HaloTarget getTargetPCCText(Card c){
		
		HaloTarget h  = new HaloTarget(0); 
		int i ;
		
		int bouclier = c.GetBouclier();
		int currentLife = c.GetLife();
		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(c);
		int manacost = GameController.instance.getCurrentSkill().ManaCost;
		int deplacement = c.GetMove();
		int bonusDeplacement = manacost*deplacement/100;
		int attack = (GameController.instance.getCurrentCard().GetAttack()/2);
		int amount = attack*(100+damageBonusPercentage)/100;
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		
		h.addInfo("-"+amount+" PV",0);
		h.addInfo("-"+bonusDeplacement+" MOV",0);
		
		int probaEsquive = c.GetEsquive();
		int proba ;
		string s = "HIT : ";
		if (probaEsquive!=0){
			proba = 100-probaEsquive;
			s+=proba+"% : "+100+"%(ATT) - "+probaEsquive+"%(ESQ)";
		}
		else{
			proba = 100;
			s+=proba+"%";
		}
		
		if(proba==100){
			i=2;
		}
		else if(proba>=50){
			i=1;
		}
		else{
			i=0;
		}
		
		h.addInfo(s,i);
		
		return h ;
	}
	
	public override string getSuccessText(){
		return "A lancé coupe-jambes" ;
	}
	
	public override string getFailureText(){
		return "Coupe-jambes a échoué" ;
	}
}
