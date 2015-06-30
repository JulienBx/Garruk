using UnityEngine;
using System.Collections.Generic;

public class Furtivite : GameSkill
{
	public Furtivite()
	{
		this.numberOfExpectedTargets = 0 ; 
	}
	
	public override void launch()
	{
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	                     
		GameController.instance.startPlayingSkill();
		GameController.instance.applyOn();
		
		GameController.instance.playSkill(1);
		GameController.instance.play();
	}
	
	public override void applyOn(){
//		int attackBonus = GameController.instance.getCurrentSkill().ManaCost;
//		int target = GameController.instance.currentPlayingCard ;
//		
//		GameController.instance.addCardModifier(target, 0, ModifierType.Type_Intouchable, ModifierStat.Stat_No, 2, 0, "Invisible", "Ne peut pas etre ciblé par une attaque ou compétence", "Actif 2 tours");
//		GameController.instance.addCardModifier(target, attackBonus, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, 2, 9, "Renforcement", "Attaque augmentée de "+attackBonus, "Actif 2 tours");
//		GameController.instance.displaySkillEffect(target, "INVISIBLE\n+"+attackBonus+" ATK", 3, 0);
//		GameController.instance.getCurrentSkill().nbLeft--;
//		GameController.instance.getCurrentSkill().Description="Ne peut etre utilisé qu'une fois par combat. Déjà utilisé";
//		GameController.instance.showMyPlayingSkills(target);
	}
	
	public override bool isLaunchable(Skill s){
		return (s.nbLeft>0) ;
	}
	
	public override string getSuccessText(){
		return "A lancé furtivité" ;
	}
}
