using UnityEngine;
using System.Collections.Generic;

public class ArmeEnchantee : GameSkill
{
	public ArmeEnchantee()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameController.instance.displayAllysButMeTargets();
		GameController.instance.displayMyControls("Arme enchantée");
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.startPlayingSkill();
		int target = targetsPCC[0];
		int successType = 0 ;
		int maxBonus = GameController.instance.getCurrentSkill().ManaCost;
		
		if (Random.Range(1,101) > GameController.instance.getCard(target).GetMagicalEsquive())
		{                             
			int arg = Random.Range(1,maxBonus+1);
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
		GameController.instance.addCardModifier(target, arg, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, 9, "Arme enchantée", "+"+arg+"ATK", "Permanent");
		GameController.instance.displaySkillEffect(target, "+"+arg+" ATK", 3, 0);
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
		int proba;
		int probaEsquive = c.GetMagicalEsquive();
		int manacost = GameController.instance.getCurrentSkill().ManaCost;
		
		h.addInfo("+ 1-"+(manacost)+" ATK",2);
		
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
		return "A lancé arme enchantée" ;
	}
	
	public override string getFailureText(){
		return "Arme enchantée a échoué" ;
	}
}
