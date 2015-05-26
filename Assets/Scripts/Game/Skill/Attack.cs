using UnityEngine;
using System.Collections.Generic;

public class Attack : GameSkill
{
	public Attack()
	{
	}
	
	public override void launch(Skill skill)
	{
		Debug.Log("Je lance attack");
		this.skill = skill;
		GameController.instance.lookForTarget(this);
	}
	
	public override void setTarget(PlayingCardController pcc)
	{
		StatModifier modifier = new StatModifier(-skill.Power, ModifierType.Type_BonusMalus, ModifierStat.Stat_Life);
		pcc.card.modifiers.Add(modifier);
		GameController.instance.setCurrentModifier(modifier);
	}
}
