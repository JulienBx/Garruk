using UnityEngine;
using System.Collections.Generic;

public class Frenesie : GameSkill
{
	public Frenesie()
	{
		
	}
	
	public override void launch()
	{
		Debug.Log("Je lance frénésie");
		GameController.instance.displayAdjacentTargets();
	}
	
	public override void resolve(List<int> targetsPCC)
	{
		int targetID = GameController.instance.currentPlayingCard;
		
		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus(new Card());
		int degats = GameController.instance.getCurrentSkill().ManaCost*(100+damageBonusPercentage)/100;
		int bonus = GameController.instance.getCurrentSkill().ManaCost;
		
		string message = GameController.instance.getCurrentCard().Title+" s'inflige "+degats+" dégats eu augmente son attaque de "+degats;
			
		GameController.instance.addCardModifier(targetID, degats, ModifierType.Type_BonusMalus, ModifierStat.Stat_Dommage,-1,-1,"", "", "");
		GameController.instance.addCardModifier(targetID, bonus, ModifierType.Type_BonusMalus, ModifierStat.Stat_Attack, -1, -1, "", "", "");
	
		GameController.instance.play();	
	}
	
	public override bool isLaunchable(Skill s){
		return true;
	}
}
