using UnityEngine;
using System.Collections.Generic;

public class AttaqueRapide : GameSkill
{
	public AttaqueRapide()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameController.instance.displayAdjacentOpponentsTargets();
		GameController.instance.displayMyControls("Attaque rapide");
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		int target = targetsPCC[0];
		GameController.instance.startPlayingSkill();
		int success = 0 ;
		int nbHitMax = Random.Range(1,5);
		
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
		
		Debug.Log(target+","+arg);
		Card targetCard = GameController.instance.getCard(target);
		int currentLife = targetCard.GetLife();
		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(targetCard);
		int bouclier = targetCard.GetBouclier();
		int amount = arg*(GameController.instance.getCurrentSkill().ManaCost*GameController.instance.getCurrentCard().GetAttack()/100)*(100+damageBonusPercentage)/100;
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		GameController.instance.addCardModifier(target, amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		
		if (arg>1){
			GameController.instance.displaySkillEffect(target, arg+"HITS\n"+"-"+amount+" PV", 3, 1);
		}
		else{
			GameController.instance.displaySkillEffect(target, arg+"HIT\n"+"-"+amount+" PV", 3, 1);
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
		
		int currentLife = c.GetLife();
		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(c);
		int bouclier = c.GetBouclier();
		int amount = (GameController.instance.getCurrentSkill().ManaCost*GameController.instance.getCurrentCard().GetAttack()/100)*(100+damageBonusPercentage)/100;
		int amount1 = Mathf.Min(currentLife, amount-(bouclier*amount/100));
		int amount2 = Mathf.Min(currentLife, 4*(amount-(bouclier*amount/100)));
		
		h.addInfo("- "+amount1+"-"+amount2+" PV",0);
		
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
		return "A lancé attaque rapide" ;
	}
	
	public override string getFailureText(){
		return "Attaque rapide a échoué" ;
	}
}
