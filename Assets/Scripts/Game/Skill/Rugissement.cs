using UnityEngine;
using System.Collections.Generic;

public class Rugissement : GameSkill
{
	public Rugissement()
	{
		this.numberOfExpectedTargets = 0 ; 
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	
		int[] targets ;
		GameController.instance.startPlayingSkill();
		
		int debut = 0 ;
		if(!GameController.instance.isFirstPlayer){
			debut = GameController.instance.limitCharacterSide	;
		}
		
		for(int i = debut ; i < debut+GameController.instance.limitCharacterSide;i++){
			if(!GameController.instance.getPCC(i).isDead){
				targets = new int[1];
				targets[0] = i;
				GameController.instance.applyOn(targets);
			}
		}
		
		GameController.instance.playSkill();
		GameController.instance.play();
	}
	
	public override void applyOn(int[] targets){
		int amount = GameController.instance.getCurrentSkill().ManaCost;
		int attack ;
		
		for (int i = 0 ; i < targets.Length ; i++){
			attack = GameController.instance.getCard(targets[i]).GetAttack();
			GameController.instance.addCardModifier(targets[i], amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, 1, 9, "Renforcement", "Attaque augmentée de "+amount, "Actif 1 tour");
			GameController.instance.displaySkillEffect(targets[i], "ATK : "+attack+" -> "+(attack+amount), 3, 0);
		}
	}
	
	public override void failedToCastOn(int[] targets){
		for (int i = 0 ; i < targets.Length ; i++){
			GameController.instance.displaySkillEffect(targets[i], "Echec", 3, 1);
		}
	}
	
	public override bool isLaunchable(Skill s){
		return true ;
	}
	
	public override string getPlayText(){
		return "Frénésie" ;
	}
}
