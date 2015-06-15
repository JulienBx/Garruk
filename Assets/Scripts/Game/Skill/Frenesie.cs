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
		GameController.instance.lookForAdjacentTarget("Choisir une cible à attaquer", "Lancer frénésie");
	}
	
	public override void resolve(int[] args)
	{
		int targetID = GameController.instance.currentPlayingCard;
		
		int damageBonusPercentage = GameController.instance.getCurrentCard().GetDamagesPercentageBonus();
		int degats = GameController.instance.getCurrentSkill().ManaCost*(100+damageBonusPercentage)/100;
		int bonus = GameController.instance.getCurrentSkill().ManaCost;
		
		string message = GameController.instance.getCurrentCard().Title+" s'inflige "+degats+" dégats eu augmente son attaque de "+degats;
			
		GameController.instance.addModifier(targetID, degats, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Dommage);
		GameController.instance.addModifier(targetID, bonus, (int)ModifierType.Type_BonusMalus, (int)ModifierStat.Stat_Attack);
	
		GameController.instance.play(message);	
	}
	
	public override bool isLaunchable(Skill s){
		return true;
	}
}
