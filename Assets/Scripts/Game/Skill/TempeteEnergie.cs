using UnityEngine;
using System.Collections.Generic;

public class TempeteEnergie : GameSkill
{
	public TempeteEnergie()
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
		int[] args;
		int manacost = GameController.instance.getCurrentSkill().ManaCost;
		GameController.instance.startPlayingSkill();
		
		int debut = 0 ;
		if(!GameController.instance.isFirstPlayer){
			debut = GameController.instance.limitCharacterSide	;
		}
		
		for(int i = 0 ; i < GameController.instance.getNbPlayingCards();i++){
			targets = new int[1];
			targets[0] = i;
			args = new int[1];
			args[0] = (Random.Range(1,101)*(manacost-5)/100)+5;
			GameController.instance.applyOn(targets, args);
		}
		
		GameController.instance.playSkill();
		GameController.instance.play();
	}
	
	public override void applyOn(int[] targets, int[] args){
		int amount = args[0];
		
		for (int i = 0 ; i < targets.Length ; i++){
			GameController.instance.addCardModifier(targets[i], amount, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage, -1, -1, "", "", "");
			GameController.instance.displaySkillEffect(targets[i], "Inflige "+amount+" dégats", 3, 1);
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
		return "Tempete d'énergie" ;
	}
}
