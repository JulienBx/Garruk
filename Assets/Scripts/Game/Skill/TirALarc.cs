using UnityEngine;

public class TirALarc : GameSkill
{
	public override void launch()
	{
		Debug.Log("Je lance tir à l'arc");
		GameController.instance.lookForTarget("", "");
	}
	
	public override void setTarget(PlayingCardController pcc)
	{
		//pcc.card.modifiers.Add(new StatModifier(-skill.Power, ModifierType.Type_BonusMalus, ModifierStat.Stat_Life));
	}
}
