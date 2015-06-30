using UnityEngine;
using System.Collections.Generic;

public class TirALarc : GameSkill
{
	public TirALarc()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameController.instance.displayOpponentsTargets();
		GameController.instance.displayMyControls("Tir à l'arc");
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		int target = targetsPCC[0];
		GameController.instance.startPlayingSkill();
		int success = 0 ;
		int nbHitMax = Random.Range(1,3);
		
		int arg = 0;
		
		for (int i = 0 ; i < nbHitMax ; i++){
			if (Random.Range(1,101) > GameController.instance.getCard(targetsPCC[0]).GetEsquive())
			{                             
				arg++;
			}
		}
		
		if (arg!=0){
			GameController.instance.applyOn(target, arg);
			success=1;
		}
		else{
			GameController.instance.failedToCastOnSkill(target,1);
		}
		
		GameController.instance.playSkill(success);
		GameController.instance.play();
	}
	
	public override void applyOn(int target, int arg){
		
		Card targetCard = GameController.instance.getCard(target);
		int currentLife = targetCard.GetLife();
		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(targetCard);
		int bouclier = targetCard.GetBouclier();
		int amount = GameController.instance.getCurrentSkill().ManaCost*(100+damageBonusPercentage)/100;
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		
		for (int i = 0 ; i < arg ; i++){
			GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		}
		if (arg>1){
			GameController.instance.displaySkillEffect(target, arg+" HITS\n"+"Inflige "+amount+" dégats", 3, 1);
		}
		else{
			GameController.instance.displaySkillEffect(target, arg+" HIT\n"+"Inflige "+amount+" dégats", 3, 1);
		}
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		//GameController.instance.displaySkillEffect(target, GameController.instance.castFailures.getFailure(indexFailure), 5, 1);
	}
	
	public override bool isLaunchable(Skill s){
		return GameController.instance.canLaunchOpponents();
	}
	
	public override HaloTarget getTargetPCCText(Card c){
		
		HaloTarget h  = new HaloTarget(0); 
		int i ;
		int proba;
		int probaEsquive = c.GetEsquive();
		
		int currentLife = c.GetLife();
		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(c);
		int bouclier = c.GetBouclier();
		int amount = GameController.instance.getCurrentSkill().ManaCost*(100+damageBonusPercentage)/100;
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		
		h.addInfo("PV : -"+Mathf.Min(currentLife,amount)+"-"+Mathf.Min(currentLife,(2*amount)),0);
		
		string s = "HIT : ";
		if (probaEsquive!=0){
			proba = 100-probaEsquive;
			s+=proba+"% : "+100+"%(ARM) - "+probaEsquive+"%(RES)";
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
		return "A lancé tir à l'arc" ;
	}
	
	public override string getFailureText(){
		return "Tir à l'arc a échoué" ;
	}
}
