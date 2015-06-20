using UnityEngine;
using System.Collections.Generic;

public class Lenteur : GameSkill
{
	public Lenteur()
	{
		this.idSkill = 7 ; 
		this.numberOfExpectedTargets = 1 ; 
	}
	
	public override void launch()
	{
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		GameController.instance.displayAllTargets();
		GameController.instance.displayMyControls("Lenteur");
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		int[] targets = new int[1];
		targets[0] = targetsPCC[0];
		
		if (Random.Range(1,100) > GameController.instance.getCard(targetsPCC[0]).GetEsquive())
		{                             
			GameController.instance.applyOn(this.idSkill, targets);
		}
		else{
			GameController.instance.failedToCastOnSkill(this.idSkill, targets);
		}
		GameController.instance.playSkill(this.idSkill);
		GameController.instance.play();
	}
	
	public override void applyOn(int[] targets){
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		int deplacement ;
		int baseD ;
		
		for (int i = 0 ; i < targets.Length ; i++){
			baseD = GameController.instance.getCard(targets[i]).GetMove();
			deplacement = (amount)*baseD/100;
			GameController.instance.addCardModifier(targets[i], -1*deplacement, ModifierType.Type_BonusMalus, ModifierStat.Stat_Move, 1, 8, "Lenteur", "Déplacement diminué de "+deplacement, "Actif 1 tour");
			GameController.instance.displaySkillEffect(targets[i], "MOV : "+baseD+" -> "+(baseD-deplacement), 3, 1);
		}
	}
	
	public override void failedToCastOn(int[] targets){
		for (int i = 0 ; i < targets.Length ; i++){
			GameController.instance.displaySkillEffect(targets[i], "Lenteur échoue", 3, 1);
		}
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
	
	public override HaloTarget getTargetPCCText(Card c){
		
		HaloTarget h  = new HaloTarget(0); 
		int i ;
		
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		int baseD = c.GetMove();
		int deplacement = (amount)*baseD/100;
		
		h.addInfo("MOV : "+baseD+" -> "+(baseD-deplacement),0);
		
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
		h.addInfo("HIT% : "+probaHit,i);
		
		return h ;
	}
	
	public override string getPlayText(){
		return "Lenteur" ;
	}
}
