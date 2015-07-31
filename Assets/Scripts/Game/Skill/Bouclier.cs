using UnityEngine;
using System.Collections.Generic;

public class Bouclier : GameSkill
{
	public Bouclier()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameController.instance.displayAllysButMeTargets();
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
	
	public override void applyOn(int target){
//		int amount = GameController.instance.getCurrentSkill().ManaCost;
//		
//		GameController.instance.addCardModifier(target, amount, ModifierType.Type_Bouclier, ModifierStat.Stat_No, -1, 10, "Bouclier", "Dommages physiques : -"+amount+"%", "Permanent");
//		GameController.instance.displaySkillEffect(target, "+ BOUCLIER "+amount+"%", 3, 0);
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		//GameController.instance.displaySkillEffect(target, GameController.instance.castFailures.getFailure(indexFailure), 5, 1);
	}
	
	public override string isLaunchable(){
		return "";
	}
	
	public override HaloTarget getTargetPCCText(Card c){
		
		HaloTarget h  = new HaloTarget(0); 
//		int i ;
//		int proba;
//		int probaEsquive = c.GetMagicalEsquive();
//		int amount = GameController.instance.getCurrentSkill().ManaCost;
//		
//		h.addInfo("Bouclier +"+amount+"%",2);
//		
//		string s = "HIT : ";
//		if (probaEsquive!=0){
//			proba = 100-probaEsquive;
//			s+=proba+"% : "+100+"%(BOU) - "+probaEsquive+"%(RES)";
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
