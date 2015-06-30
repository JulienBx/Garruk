using UnityEngine;
using System.Collections.Generic;

public class Terreur : GameSkill
{
	public Terreur(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameController.instance.displayAdjacentOpponentsTargets();
		GameController.instance.displayMyControls("Terreur");
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.startPlayingSkill();
		int target = targetsPCC[0];
		int successType = 0 ;
		
		if (Random.Range(1,101) > GameController.instance.getCard(target).GetEsquive())
		{                             
			int arg = Random.Range(1,101);
			GameController.instance.applyOn(target, arg);
			successType = 1 ;
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 1);
		}
		GameController.instance.playSkill(successType);
		GameController.instance.play();
	}
	
	public override void applyOn(int target, int arg){
		Card targetCard = GameController.instance.getCard(target);
		int bouclier = targetCard.GetBouclier();
		int currentLife = targetCard.GetLife();
		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(targetCard);
		int manacost = GameController.instance.getCurrentSkill().ManaCost;
		int amount = (GameController.instance.getCurrentCard().GetAttack()/2)*(100+damageBonusPercentage)/100;
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		
		GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		if (arg<=manacost){
			GameController.instance.addCardModifier(target, amount, ModifierType.Type_Paralized, ModifierStat.Stat_No, 1, 2, "Paralisé", "Ne peut plus utiliser ses compétences", "Actif 1 tour");
			GameController.instance.displaySkillEffect(target, "-"+amount+" PV\nParalisye", 3, 1);
		}
		else{
			GameController.instance.displaySkillEffect(target, "-"+amount+" PV\nEchec paralysie", 3, 1);
		}	
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		//GameController.instance.displaySkillEffect(target, GameController.instance.castFailures.getFailure(indexFailure), 5, 1);
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
		int amount = (GameController.instance.getCurrentCard().GetAttack()/2)*(100+damageBonusPercentage)/100;
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		
		h.addInfo("-"+amount+" PV",0);
		h.addInfo("Par."+manacost+"%",0);
		
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
		return "Terreur" ;
	}
}
