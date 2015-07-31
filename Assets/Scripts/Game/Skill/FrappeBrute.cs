using UnityEngine;
using System.Collections.Generic;

public class FrappeBrute : GameSkill
{
	public FrappeBrute()
	{
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
//		int successType = 0 ;
//		
//		if (Random.Range(1,101) > GameController.instance.getCard(target).GetEsquive())
//		{                             
//			int arg = Random.Range(1,GameController.instance.getCurrentSkill().ManaCost+1)*GameController.instance.getCurrentCard().GetAttack()/100;
//			GameController.instance.applyOn(target, arg);
//			successType = 1 ;
//		}
//		else{
//			GameController.instance.failedToCastOnSkill(target, 1);
//		}
//		GameController.instance.playSkill(successType);
//		GameController.instance.play();
	}
	
	public override void applyOn(int target, int arg){
		GameController.instance.addCardModifier(target, arg, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		GameView.instance.displaySkillEffect(target, "-"+arg+" PV", 1);
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
//		int currentLife = c.GetLife();
//		int amount = GameController.instance.getCurrentSkill().ManaCost*GameController.instance.getCurrentCard().GetAttack()/100;
//		
//		h.addInfo("- 1"+"-"+amount+" PV",0);
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
//		
//		h.addInfo(s,i);
		
		return h ;
	}
}
