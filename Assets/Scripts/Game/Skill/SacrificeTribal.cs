using UnityEngine;
using System.Collections.Generic;

public class SacrificeTribal : GameSkill
{
	public SacrificeTribal(){
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameController.instance.displayAdjacentAllyTargets();
		GameController.instance.displayMyControls("Sacrifice tribal");
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		GameController.instance.startPlayingSkill();
		int target = targetsPCC[0];
		int successType = 0 ;
		
		if (Random.Range(1,101) > GameController.instance.getCard(target).GetEsquive())
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
		Card targetCard = GameController.instance.getCard(target);;
		int currentLife = targetCard.GetLife() ;
		int currentAttack = targetCard.GetAttack() ;
		
		int manacost = GameController.instance.getCurrentSkill().ManaCost ;
		
		int bonusL = currentLife*manacost/100 ;
		int bonusA = currentAttack*manacost/100 ;
		
//		GameController.instance.addCardModifier(GameController.instance.currentPlayingCard, -1*bonusL, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
//		GameController.instance.addCardModifier(GameController.instance.currentPlayingCard, bonusA, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, -1, "Tribalité", "Bonus d'attaque : +"+bonusA, "Permanent");
//		GameController.instance.displaySkillEffect(GameController.instance.currentPlayingCard, "+"+bonusL+" PV\n+"+bonusA+" ATK", 3, 0);
//		GameController.instance.addCardModifier(target, currentLife, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
//		GameController.instance.displaySkillEffect(target, "Sacrifié !", 3, 1);
	}
	
	public override void failedToCastOn(int target, int indexFailure){
		//GameController.instance.displaySkillEffect(target, GameController.instance.castFailures.getFailure(indexFailure), 5, 1);
	}
	
	public override bool isLaunchable(Skill s){
		return GameController.instance.canLaunchAdjacentAllys();
	}
	
	public override HaloTarget getTargetPCCText(Card c){
		
		HaloTarget h  = new HaloTarget(0); 
		int i ;
		
		h.addInfo("Sacrifie",0);
		
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
		return "A lancé sacrifice tribal" ;
	}
	
	public override string getFailureText(){
		return "Sacrifice tribal a échoué" ;
	}
}
