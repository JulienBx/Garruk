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
		GameController.instance.initPCCTargetHandler(numberOfExpectedTargets);
		this.resolve(new List<int>());
	}
	
	public override void resolve(List<int> targetsPCC)
	{	                     
		GameController.instance.startPlayingSkill();
		GameController.instance.applyOn(null);
		
		GameController.instance.playSkill();
		GameController.instance.play();
	}
	
	public override void applyOn(int[] targets){
		int attackBonus = GameController.instance.getCurrentSkill().ManaCost;
		int target = GameController.instance.currentPlayingCard ;
		int attack = GameController.instance.getCurrentCard().GetAttack();
		
		GameController.instance.addCardModifier(target, 0, ModifierType.Type_Intouchable, ModifierStat.Stat_No, 2, 0, "Invisible", "Ne peut pas etre ciblé par un sort ou une compétence", "Actif 2 tours");
		GameController.instance.addCardModifier(target, attackBonus, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, 2, 9, "Renforcement", "Attaque augmentée de "+attackBonus, "Actif 2 tours");
		GameController.instance.displaySkillEffect(target, "Devient invisible\nATK : "+attack+" -> "+(attack+attackBonus), 3, 0);
	}
	
	public override bool isLaunchable(Skill s){
		return (GameController.instance.nbMyPlayersAlive()>1) ;
	}
	
	public override string getPlayText(){
		return "Furtivité" ;
	}
}
