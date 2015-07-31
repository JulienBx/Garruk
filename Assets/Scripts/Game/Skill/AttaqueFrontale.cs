using UnityEngine;
using System.Collections.Generic;

public class AttaqueFrontale : GameSkill
{
	public AttaqueFrontale(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameController.instance.displayAdjacentOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		int target = targetsPCC[0];
		int successType = 0 ;
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetEsquive())
		{                             
			GameController.instance.applyOn(target);
			successType = 1 ;
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 1);
		}
		GameController.instance.play();
	}
	
	public override void applyOn(int target){
//		Card targetCard = GameController.instance.getCard(target);
//		int myCurrentLife = GameController.instance.getCurrentCard().GetLife();
//		int currentLife = targetCard.GetLife();
//		
//		int myBouclier = GameController.instance.getCurrentCard().GetBouclier();
//		int bouclier = targetCard.GetBouclier();
//		
//		int amount = GameController.instance.getCurrentCard().GetAttack()*150/100;
//		
//		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(targetCard);
//		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
//		damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(GameController.instance.getCurrentCard());
//		
//		int myAttack = GameController.instance.getCurrentSkill().ManaCost;
//		int myAmount = (myAttack)*(100+damageBonusPercentage)/100;
//		myAmount = Mathf.Min(currentLife,myAmount-(myBouclier*myAmount/100));
//		
//		GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
//		//GameController.instance.addCardModifier(GameController.instance.currentPlayingCard, myAmount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
//		
//		GameController.instance.displaySkillEffect(target, "-"+amount+" PV", 3, 1);
//		//GameController.instance.displaySkillEffect(GameController.instance.currentPlayingCard, "- "+myAmount+" PV", 3, 1);
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		//GameController.instance.displaySkillEffect(target, GameController.instance.castFailures.getFailure(indexFailure), 5, 1);
	}
	
	public override string isLaunchable(){
		return "" ;
		//return GameController.instance.canLaunchAdjacentOpponents();
	}
	
	public override HaloTarget getTargetPCCText(Card c){
		
		HaloTarget h  = new HaloTarget(0); 
//		int i ;
//		
//		int myCurrentLife = GameController.instance.getCurrentCard().GetLife();
//		int currentLife = c.GetLife();
//		
//		int myBouclier = GameController.instance.getCurrentCard().GetBouclier();
//		int bouclier = c.GetBouclier();
//		
//		int amount = GameController.instance.getCurrentCard().GetAttack()*150/100;
//		
//		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(c);
//		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
//		damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(GameController.instance.getCurrentCard());
//		
//		int myAttack = GameController.instance.getCurrentSkill().ManaCost;
//		int myAmount = (myAttack)*(100+damageBonusPercentage)/100;
//		myAmount = Mathf.Min(currentLife,myAmount-(myBouclier*myAmount/100));
//		
//		h.addInfo("-"+amount+" PV",0);
//		
//		int probaEsquive = c.GetEsquive();
//		int proba ;
//		string s = "HIT : ";
//		if (probaEsquive!=0){
//			proba = 100-probaEsquive;
//			s+=proba+"% : "+100+"%(ATT) - "+probaEsquive+"%(ESQ)";
//		}
//		else{
//			proba = 100;
//			s+=proba+"%";
//		}
//		
//		if(proba==100){
//			i=2;
//		}
//		else if(proba>=50){
//			i=1;
//		}
//		else{
//			i=0;
//		}
		
//		h.addInfo(s,i);
		
		return h ;
	}
}
