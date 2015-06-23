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
		GameController.instance.displayAllTargets();
		GameController.instance.displayMyControls("Bouclier");
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		int[] targets = new int[1];
		targets[0] = targetsPCC[0];
		int successChances = GameController.instance.getCurrentSkill().ManaCost;
		GameController.instance.startPlayingSkill();
		
		if (Random.Range(1,100) > GameController.instance.getCard(targetsPCC[0]).GetEsquive())
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
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		
		for (int i = 0 ; i < targets.Length ; i++){
			GameController.instance.getCard(targets[i]).emptyModifiers();
			GameController.instance.addCardModifier(targets[i], amount, ModifierType.Type_Bouclier, ModifierStat.Stat_No, -1, 10, "Bouclier", "Prévient "+amount+ "% des dégats subis", "Permanent");
			GameController.instance.displaySkillEffect(targets[i], "Bouclier ajouté", 3, 0);
		}
	}
	
	public override void failedToCastOn(int[] targets){
		for (int i = 0 ; i < targets.Length ; i++){
			GameController.instance.displaySkillEffect(targets[i], "Bouclier échoue", 3, 1);
		}
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
	
	public override HaloTarget getTargetPCCText(Card c){
		
		HaloTarget h  = new HaloTarget(0); 
		int i ;
		
		int hitPercentage = GameController.instance.getCurrentSkill().ManaCost;
		
		h.addInfo("Ajoute un bouclier",2);
		
		int probaHit = hitPercentage*(100 - c.GetEsquive())/100;
		if (probaHit>=80){
			i = 2 ;
		}
		else if (probaHit>=20){
			i = 1 ;
		}
		else{
			i = 0 ;
		}
		h.addInfo("HIT% : "+probaHit,i);
		
		return h ;
	}
	
	public override string getPlayText(){
		return "Bouclier" ;
	}
}
