using UnityEngine;
using System.Collections.Generic;

public class Lenteur : GameSkill
{
	public Lenteur()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameController.instance.displayOpponentsTargets();
		GameController.instance.displayMyControls("Lenteur");
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.startPlayingSkill();
		int target = targetsPCC[0];
		int successType = 0 ;
		
		if (Random.Range(1,101) > GameController.instance.getCard(target).GetMagicalEsquive())
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
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		
		int baseD = GameController.instance.getCard(target).GetMove();
		int deplacement = (amount)*baseD/100;
		
		if (deplacement >= baseD){
			deplacement = baseD - 1 ;
		}
		
		GameController.instance.addCardModifier(target, -1*deplacement, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, 1, 8, "Lenteur", "-"+deplacement+"MOV", "Actif 1 tour");
		GameController.instance.displaySkillEffect(target, "-"+deplacement+"MOV", 3, 1);
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		GameController.instance.displaySkillEffect(target, GameController.instance.castFailures.getFailure(indexFailure), 5, 1);
	}
	
	public override bool isLaunchable(Skill s){
		return GameController.instance.canLaunchOpponents();
	}
	
	public override HaloTarget getTargetPCCText(Card c){
		
		HaloTarget h  = new HaloTarget(0); 
		int i ;
		int baseD = c.GetMove();
		int amount = GameController.instance.getCurrentSkill().ManaCost*baseD/100;
		if (amount >= baseD){
			amount = baseD - 1 ;
		}
		
		h.addInfo("-"+amount+"MOV",2);
		
		int proba ;
		int probaEsquive = c.GetMagicalEsquive();
		
		string s = "HIT : ";
		if (probaEsquive!=0){
			proba = 100-probaEsquive;
			s+=proba+"% : "+100+"%(LEN) - "+probaEsquive+"%(RES)";
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
			i = 0;
		}
		
		h.addInfo(s,i);
		
		return h ;
	}
	
	public override string getSuccessText(){
		return "A lancé lenteur" ;
	}
	
	public override string getFailureText(){
		return "Lenteur a échoué" ;
	}
}
