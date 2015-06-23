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
		GameController.instance.displayAllTargets();
		GameController.instance.displayMyControls("Attaque rapide");
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		int[] targets = new int[1];
		targets[0] = targetsPCC[0];
		GameController.instance.startPlayingSkill();
		
		int nbHitMax = Random.Range(1,5);
		
		int[] args = new int[1];
		args[0] = 0 ;
		
		for (int i = 0 ; i < nbHitMax ; i++){
			if (Random.Range(1,101) > GameController.instance.getCard(targetsPCC[0]).GetEsquive())
			{                             
				args[0]++;
			}
		}
		
		if (args[0]!=0){
			GameController.instance.applyOn(targets, args);
		}
		else{
			GameController.instance.failedToCastOnSkill(targets);
		}
		
		GameController.instance.playSkill();
		GameController.instance.play();
	}
	
	public override void applyOn(int[] targets, int[] args){
		
		Card targetCard = GameController.instance.getCard(targets[0]);
		int currentLife = targetCard.GetLife();
		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(targetCard);
		int bouclier = targetCard.GetBouclier();
		int amount = GameController.instance.getCurrentSkill().ManaCost*(100+damageBonusPercentage)/100;
		amount = Mathf.Min(currentLife,amount-(bouclier*amount/100));
		
		for (int i = 0 ; i < args[0] ; i++){
			GameController.instance.addCardModifier(targets[0], amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		}
		GameController.instance.displaySkillEffect(targets[0], "Touché "+args[0]+"fois\n"+"PV : "+currentLife+" -> "+Mathf.Max(0,(currentLife-args[0]*amount)), 3, 1);
	}
	
	public override void failedToCastOn(int[] targets){
		for (int i = 0 ; i < targets.Length ; i++){
			GameController.instance.displaySkillEffect(targets[i], "Echec", 3, 1);
		}
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
	
	public override HaloTarget getTargetPCCText(Card c){
		
		HaloTarget h  = new HaloTarget(0); 
		int i ;
		
		int currentLife = c.GetLife();
		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(c);
		int bouclier = c.GetBouclier();
		int amount = GameController.instance.getCurrentSkill().ManaCost*(100+damageBonusPercentage)/100;
		amount = amount-(bouclier*amount/100);
		
		h.addInfo("PV : "+currentLife+" -> "+Mathf.Max(0,(currentLife-4*amount))+"-"+Mathf.Max(0,(currentLife-amount)),0);
		
		int probaHit = 100 - c.GetEsquive();
		if (probaHit>=80){
			i = 2 ;
		}
		else if (probaHit>=20){
			i = 1 ;
		}
		else{
			i = 0 ;
		}
		h.addInfo("HIT% : "+probaHit+"% / HIT",i);
		
		return h ;
	}
	
	public override string getPlayText(){
		return "Attaque rapide" ;
	}
}
