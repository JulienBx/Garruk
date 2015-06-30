using UnityEngine;
using System.Collections.Generic;

public class Celerite : GameSkill
{
	public Celerite()
	{
		this.numberOfExpectedTargets = 1 ; 
	}

	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameController.instance.displayAllysButMeTargets();
		GameController.instance.displayMyControls("Célérité");
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.startPlayingSkill();
		int target = targetsPCC[0];
		int successType = 0 ;
		
		int successChances = GameController.instance.getCurrentSkill().ManaCost;
		
		if (Random.Range(1,101) > GameController.instance.getCard(target).GetMagicalEsquive())
		{                             
			if (Random.Range(1,101) <= successChances)
			{ 
				GameController.instance.applyOn(target);
				successType = 1 ;
			}
			else{
				GameController.instance.failedToCastOnSkill(target, 2);
			}
		}
		else{
			GameController.instance.failedToCastOnSkill(target, 1);
		}
		GameController.instance.playSkill(successType);
		GameController.instance.play();
	}
	
	public override void applyOn(int target){
		GameController.instance.rankNext(target);
		GameController.instance.addCardModifier(target, 0, ModifierType.Type_Celerite, ModifierStat.Stat_No, 1, 13, "CELERITE", "Joue au prochain tour", "Actif jusqu'au tour suivant");
		GameController.instance.displaySkillEffect(target, "+ CELERITE", 5, 0);
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		//GameController.instance.displaySkillEffect(target, GameController.instance.castFailures.getFailure(indexFailure), 5, 1);
	}
	
	public override bool isLaunchable(Skill s){
		return GameController.instance.canLaunchAllysButMe();
	}
	
	public override HaloTarget getTargetPCCText(Card c){
		
		HaloTarget h  = new HaloTarget(0); 
		int i ;
		int probaEsquive = c.GetMagicalEsquive();
		int probaHit = GameController.instance.getCurrentSkill().ManaCost;
		int proba ;
		
		h.addInfo("+ Célérité",2);
		
		string s = "HIT : ";
		if (probaEsquive!=0){
			proba = probaHit-probaEsquive;
			s+=proba+"% : "+probaHit+"%(CEL) - "+probaEsquive+"%(RES)";
		}
		else{
			proba = probaHit;
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
		return "A lancé célérité" ;
	}
	
	public override string getFailureText(){
		return "Célérité a échoué" ;
	}
	
	
}
