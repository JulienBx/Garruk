using UnityEngine;
using System.Collections.Generic;

public class RayonEnergie : GameSkill
{
	public RayonEnergie()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameView.instance.displayOpponentsTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		int target = targetsPCC[0];
		int successType = 0 ;
		
		if (Random.Range(1,101) > GameView.instance.getCard(target).GetMagicalEsquive())
		{                             
			GameController.instance.applyOn(target);
			successType = 1 ;
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 1);
		}
		GameController.instance.play();
	}
	
	public override void applyOn(){
		
//		Card targetCard = GameController.instance.getCard(target);
//		int currentLife = targetCard.GetLife();
//		int amount = GameController.instance.getCurrentSkill().ManaCost;
//		amount = Mathf.Min(currentLife,amount);
//		
//		GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
//		
//		GameController.instance.displaySkillEffect(target, "-"+amount+" PV", 3, 1);
	}
	
	public override string isLaunchable(){
		return "";
	}
	
	public override HaloTarget getTargetPCCText(Card c){
		
		HaloTarget h  = new HaloTarget(0); 
//		int i ;
//		
//		int currentLife = c.GetLife();
//		int amount = GameController.instance.getCurrentSkill().ManaCost;
//		amount = Mathf.Min(currentLife,amount);
//		
//		h.addInfo("-"+amount+" PV",0);
//		
//		int proba ;
//		int probaEsquive = c.GetMagicalEsquive();
//		
//		string s = "HIT : ";
//		if (probaEsquive!=0){
//			proba = 100-probaEsquive;
//			s+=proba+"% : "+100+"%(LEN) - "+probaEsquive+"%(RES)";
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
//			i = 0;
//		}
//		
//		h.addInfo(s,i);
		
		return h ;
	}
}
