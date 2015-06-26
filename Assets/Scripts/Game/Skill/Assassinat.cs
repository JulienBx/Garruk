using UnityEngine;
using System.Collections.Generic;

public class Assassinat : GameSkill
{
	public Assassinat(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameController.instance.displayAdjacentOpponentsTargets();
		GameController.instance.displayMyControls("Assassinat");
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.startPlayingSkill();
		int target = targetsPCC[0];
		int successType = 1 ;
		
		int successChances = GameController.instance.getCurrentSkill().ManaCost;
		
		if (Random.Range(1,101) > GameController.instance.getCard(target).GetEsquive())
		{                             
			if (Random.Range(1,101) <= successChances)
			{ 
				GameController.instance.applyOn(target);
				successType = 0 ;
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
		int currentLife = GameController.instance.getCard(target).GetLife();
		GameController.instance.addCardModifier(target, currentLife, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		GameController.instance.displaySkillEffect(target, "Assassiné", 3, 1);
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
		int probaEsquive = c.GetEsquive();
		int probaHit = GameController.instance.getCurrentSkill().ManaCost;
		int proba ;
		
		h.addInfo("Assassinat",2);
		
		string s = "HIT : ";
		if (probaEsquive!=0){
			proba = probaHit-probaEsquive;
			s+=proba+"% : "+probaHit+"%(ASS) - "+probaEsquive+"%(ESQ)";
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
			i=0;
		}
		
		h.addInfo(s,i);
		
		return h ;
	}
	
	public override string getSuccessText(){
		return "A lancé assassinat" ;
	}
	
	public override string getFailureText(){
		return "Assassinat a échoué" ;
	}
}
