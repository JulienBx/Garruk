using UnityEngine;
using System.Collections.Generic;

public class RayonEnergie : GameSkill
{
	public RayonEnergie()
	{
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameController.instance.displayAllTargets();
		GameController.instance.displayMyControls("Rayon d'énergie");
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		int[] targets = new int[1];
		targets[0] = targetsPCC[0];
		GameController.instance.startPlayingSkill();
		
		if (Random.Range(1,101) > GameController.instance.getCard(targetsPCC[0]).GetEsquive())
		{ 
			GameController.instance.applyOn(targets);
		}
		else{
			GameController.instance.failedToCastOnSkill(targets);
		}
		
		GameController.instance.playSkill();
		GameController.instance.play();
	}
	
	public override void applyOn(int[] targets){
		
		Card targetCard = GameController.instance.getCard(targets[0]);
		int currentLife = targetCard.GetLife();
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		amount = Mathf.Min(currentLife,amount);
		
		GameController.instance.addCardModifier(targets[0], amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
		
		GameController.instance.displaySkillEffect(targets[0], "Inflige "+amount+" dégats", 3, 1);
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
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		amount = Mathf.Min(currentLife,amount);
		
		h.addInfo("PV : "+currentLife+" -> "+Mathf.Max(0,(currentLife-amount)),0);
		
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
		return "Rayon Energie" ;
	}
}
